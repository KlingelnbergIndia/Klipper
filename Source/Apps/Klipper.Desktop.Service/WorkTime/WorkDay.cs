using Klipper.Desktop.Service.Employees;
using Models.Core.HR.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using Klipper.Desktop.Service.WorkTime.Policies;

namespace Klipper.Desktop.Service.WorkTime
{
    public class WorkDay
    {
        #region Static Methods

        static public WorkDay GetWorkDay(int employeeId,
            DateTime day,
            List<AccessEvent> dayAccessEvents,
            List<Regularization> regularizations = null,
            Leave approvedLeave = null)
        {
            var workDay = new WorkDay(employeeId, day, dayAccessEvents, regularizations, approvedLeave);
            workDay.AppliedPolicy.SetWorkdayContext(workDay);
            workDay.Process();
            return workDay;
        }

        #endregion

        #region Constructor

        public WorkDay(
            int employeeId, 
            DateTime date, 
            List<AccessEvent> accessEvents, 
            List<Regularization> regularizations = null, 
            Leave approvedLeave = null)
        {
            EmployeeID = employeeId;
            Date = date;
            AllAccessEvents = accessEvents.OrderBy(x => x.EventTime).ToList();
            AllRegularizations = regularizations;
            ApprovedLeave = approvedLeave;
            AppliedPolicy = EmployeeService.Instance.GetWorkTimePolicy(employeeId);
        }

        #endregion

        #region Properties

        public int EmployeeID { get; set; } = -1;
        public IWorkTimePolicy AppliedPolicy { get; set; } = null;
        public DateTime Date { get; set; }
        public AccessEvent FirstAccessEvent
        {
            get
            {
                if(AllAccessEvents.Count == 0)
                {
                    return null;
                }
                return AllAccessEvents.First();
            }
        }
        public AccessEvent LastAccessEvent
        {
            get
            {
                if (AllAccessEvents.Count == 0)
                {
                    return null;
                }
                return AllAccessEvents.Last();
            }
        }
        public List<AccessEvent> AllAccessEvents { get; set; }
        public List<Regularization> AllRegularizations { get; private set; }
        public Leave ApprovedLeave { get; set; } = null;
        public TimeSpan LeaveTimeCompensation { get; private set; } = TimeSpan.FromMinutes(0);
        public TimeSpan BlockRegularizationCompensation { get; private set; } = TimeSpan.FromMinutes(0);
        public TimeSpan OutsidePremisesTimeSpan { get; private set; } = TimeSpan.FromMinutes(0);
        public TimeSpan RecreationTimeSpan { get; private set; } = TimeSpan.FromMinutes(0);
        public TimeSpan GymnasiumTimeSpan { get; private set; } = TimeSpan.FromMinutes(0);
        public bool FlaggedForSevereViolation { get; private set; } = false;
        public List<WorkTimeViolation> Violations { get; private set; } = new List<WorkTimeViolation>();
        public TimeSpan WorkTime { get; private set; } = TimeSpan.FromDays(0);
        public bool IsPresent { get { return AllAccessEvents.Count > 0;  }  }
        public bool NeedsRegularization { get; private set; } = true;
        public bool IsWeekEnd { get; private set; } = false;
        public bool IsHoliday { get; private set; } = false;
        public TimeSpan TotalDuration
        {
            get
            {
                return LastAccessEvent.EventTime - FirstAccessEvent.EventTime;
            }
        }
        #endregion

        #region Public methods

        public List<AccessEvent> SwipesAtAccessPoint(string accessPointName)
        {
            var swipes = AllAccessEvents.Where(x => x.AccessPointName == accessPointName).ToList();
            return swipes.OrderBy(x => x.EventTime).ToList();
        }

        public int SwipeCountAtAccessPoint(string accessPointName)
        {
            return SwipesAtAccessPoint(accessPointName).Count;
        }

        #endregion

        #region Private methods

        private void Process()
        {
            NeedsRegularization = false;

            IsHoliday = CheckIfHoliday();
            IsWeekEnd = AppliedPolicy.IsWeekend(Date);
            if (IsHoliday && IsPresent) Violations.Add(WorkTimeViolation.GetViolation(WorkTimeViolationType.WorkingOnHoliday, AllAccessEvents));
            if (IsWeekEnd && IsPresent) Violations.Add(WorkTimeViolation.GetViolation(WorkTimeViolationType.WorkingOnWeekend, AllAccessEvents));
            if (IsHoliday || IsWeekEnd)
            {
                return;
            }

            if(IsPresent)
            {
                CalculateTimeSpan_Gym();
                CalculateTimeSpan_Recreation();
                CalculateTimeSpan_OutsidePremises();
                CompensateForApprovedLeave();
                CompensateForBlockRegularization();

                //Calculation of work time should be after all the other calculations
                CalculateTimeSpan_Work();

                AppliedPolicy.Validate(WorkTimeRules.GymnasiumUsageRule);
                AppliedPolicy.Validate(WorkTimeRules.RecreationUsageRule);
                AppliedPolicy.Validate(WorkTimeRules.TotalWorkHoursPerDayRule);
                AppliedPolicy.Validate(WorkTimeRules.WorkStartEndTimingRule);
            }
            else
            {
                if (ApprovedLeave == null)
                {
                    //No leave applied yet and is absent 
                    Violations.Add(WorkTimeViolation.GetViolation(WorkTimeViolationType.AbsentWithoutLeave));
                }
                if (ApprovedLeave != null && ApprovedLeave.IsHalfDay)
                {
                    //Half day leave applied and is absent 
                    Violations.Add(WorkTimeViolation.GetViolation(WorkTimeViolationType.AbsentWithHalfDayLeave));
                }
            }
            FlaggedForSevereViolation = Violations.Where(v => v.Level == ViolationLevel.Severe).ToList().Count > 0;
        }

        private void CalculateTimeSpan_OutsidePremises()
        {
            var swipes = SwipesAtAccessPoint("Main Entry").OrderBy(x => x.EventTime).ToList();
            if (swipes.Count % 2 == 1)
            {
                Violations.Add(WorkTimeViolation.GetViolation(WorkTimeViolationType.OddAccessEvents_PremisesEntry, swipes));
            }
            else
            {
                TimeSpan outPremisesTime = TimeSpan.FromDays(0);
                if (swipes.Count > 2)
                {
                    for (int i = 1; i < swipes.Count; i += 2)
                    {
                        var a = swipes[i].EventTime;
                        var b = swipes[i + 1].EventTime;
                        outPremisesTime += (b - a);
                    }
                }
                OutsidePremisesTimeSpan = outPremisesTime;
            }
        }

        private void CalculateTimeSpan_Recreation()
        {
            var swipes = SwipesAtAccessPoint("Recreation").OrderBy(x => x.EventTime).ToList();
            if (swipes.Count % 2 == 1)
            {
                Violations.Add(WorkTimeViolation.GetViolation(WorkTimeViolationType.OddAccessEvents_Recreation, swipes));
            }
            else
            {
                TimeSpan recreationTime = TimeSpan.FromDays(0);
                for (int i = 1; i < swipes.Count; i += 2)
                {
                    var a = swipes[i].EventTime;
                    var b = swipes[i + 1].EventTime;
                    recreationTime += (b - a);
                }
                RecreationTimeSpan = recreationTime;
            }
        }

        private void CalculateTimeSpan_Gym()
        {
            var swipes = SwipesAtAccessPoint("Gym").OrderBy(x => x.EventTime).ToList();
            if (swipes.Count % 2 == 1)
            {
                Violations.Add(WorkTimeViolation.GetViolation(WorkTimeViolationType.OddAccessEvents_Gymnasium, swipes));
            }
            else
            {
                TimeSpan gymnasiumTime = TimeSpan.FromDays(0);
                for (int i = 1; i < swipes.Count; i += 2)
                {
                    var a = swipes[i].EventTime;
                    var b = swipes[i + 1].EventTime;
                    gymnasiumTime += (b - a);
                }
                GymnasiumTimeSpan = gymnasiumTime;
            }
        }

        private void CompensateForApprovedLeave()
        {
            if(ApprovedLeave == null)
            {
                LeaveTimeCompensation = TimeSpan.FromHours(0);
                return;
            }
            if(ApprovedLeave.IsHalfDay)
            {
                LeaveTimeCompensation = TimeSpan.FromHours(4.5);
            }
            else
            {
                LeaveTimeCompensation = TimeSpan.FromHours(9.0);
                if(IsPresent)
                {
                    Violations.Add(WorkTimeViolation.GetViolation(WorkTimeViolationType.WorkingOnLeaveDay, AllAccessEvents));
                }
            }
        }

        private void CompensateForBlockRegularization()
        {
            BlockRegularizationCompensation = TimeSpan.FromHours(0);
            if (AllRegularizations.Count == 0)
            {
                return;
            }
            var regularizationsWithBlockEntry = AllRegularizations.Where(r => r.RegularizationType == RegularizationType.BlockTimeSpanEntry).ToList();

            if (regularizationsWithBlockEntry.Count > 0)
            {
                foreach(var r in regularizationsWithBlockEntry)
                {
                    BlockRegularizationCompensation += r.BlockTimeSpanEntry;
                }
            }
        }

        private void CalculateTimeSpan_Work()
        {
            var violationsNeedingRegularizations = Violations.Where(v => v.Level == ViolationLevel.Severe || v.Level == ViolationLevel.Medium).ToList();
            if(violationsNeedingRegularizations.Count > 0)
            {
                NeedsRegularization = true;
            }

            var combinedDurationOfLunchAndRecreation = OutsidePremisesTimeSpan + RecreationTimeSpan;
            var excess = combinedDurationOfLunchAndRecreation - TimeSpan.FromMinutes(45.0);
            if(excess < TimeSpan.FromMinutes(0))
            {
                excess = TimeSpan.FromMinutes(0);
            }
            WorkTime = TotalDuration + LeaveTimeCompensation + BlockRegularizationCompensation - (excess + GymnasiumTimeSpan);
        }

        private bool CheckIfHoliday()
        {
            var holidays = WorkTimeService.Instance.GetHolidaysByYear(Date.Year);
            foreach (var h in holidays)
            {
                if (h.Year == Date.Year && h.Month == Date.Month && h.Day == Date.Day)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

    }
}
