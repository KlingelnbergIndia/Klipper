using Klipper.Desktop.Service.EmployeeServices;
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
            AppliedPolicy = EmployeeService.GetWorkTimePolicy(employeeId);
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
        public TimeSpan OutsidePremisesTimeSpan { get; private set; } = TimeSpan.FromMinutes(0);
        public TimeSpan RecreationTimeSpan { get; private set; } = TimeSpan.FromMinutes(0);
        public TimeSpan GymnasiumTimeSpan { get; private set; } = TimeSpan.FromMinutes(0);
        public bool FlaggedForViolation { get; private set; } = false;
        public List<WorkTimeViolation> Violations { get; private set; } = new List<WorkTimeViolation>();
        public TimeSpan TotalDuration
        {
            get
            {
                return LastAccessEvent.EventTime - FirstAccessEvent.EventTime;
            }
        }
        public TimeSpan WorkTime
        {
            get
            {
                return TimeSpan.FromDays(0);
            }
        }
        public bool IsPresent
        {
            get
            {
                return AllAccessEvents.Count > 0;
            }
        }
        public bool NeedsRegularization
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Public methods

        public List<AccessEvent> TotalSwipes(string accessPointName)
        {
            return AllAccessEvents.Where(x => x.AccessPointName == accessPointName).ToList();
        }

        public int TotalSwipeCount(string accessPointName)
        {
            return TotalSwipes(accessPointName).Count;
        }

        #endregion

        #region Private methods

        private void Process()
        {
            throw new NotImplementedException();
        }

        private void CalculateGymnasiumTime()
        {
        }

        private void CalculateRecreationLunchTime()
        {
            throw new NotImplementedException();
        }

        private void CompensateForApprovedLeave()
        {

        }

        #endregion

    }
}


