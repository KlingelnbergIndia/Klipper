using Models.Core.HR.Attendance;
using System;
using System.Collections.Generic;

namespace Klipper.Desktop.Service.WorkTime.Policies
{
    public enum WorkTimeViolationType
    {
        Unknown = 0,
        TimeDurationViolation_TotalDurationLessThan6Hours,
        TimeDurationViolation_TotalDurationLessThan9Hours,
        TimeDurationViolation_RecreationLunchTimeMoreThan45Min,
        TimeSlotViolation_RecreationLunchTime,
        TimeSlotViolation_GymnasiumUsedDuringWorkHours,
        OddAccessEvents_Recreation,
        OddAccessEvents_Gymnasium,
        OddAccessEvents_PremisesEntry,
        WorkingOnLeaveDay,
        WorkingOnWeekend,
        WorkingOnHoliday,
        AbsentWithoutLeave,
        AbsentWithHalfDayLeave,
        LateEntry,
        EarlyLeaving,
        StayingLate,
        StayingVeryLate,
    };

    public enum ViolationLevel
    {
        Unknown,
        Severe,
        Medium,
        Low,
        Ignorable,
        Strange
    };

    public class WorkTimeViolation : PolicyViolation
    {
        #region Properties

        public WorkTimeViolationType ViolationType { get; set; } = WorkTimeViolationType.Unknown;
        public string Description { get; set; } = "";
        public ViolationLevel Level { get; set; } = ViolationLevel.Unknown;
        public List<AccessEvent> AccessEvents { get; set; } = null;

        #endregion

        #region Fields

        static private Dictionary<WorkTimeViolationType, Tuple<string, ViolationLevel>> _violations 
            = new Dictionary<WorkTimeViolationType, Tuple<string, ViolationLevel>>()
        {
            { WorkTimeViolationType.Unknown, new Tuple<string, ViolationLevel>("Unknown work-time violation", ViolationLevel.Unknown)},
            { WorkTimeViolationType.TimeDurationViolation_TotalDurationLessThan6Hours, new Tuple<string, ViolationLevel>("Time duration violation: Total work-time less than 6 hours.", ViolationLevel.Severe)},
            { WorkTimeViolationType.TimeDurationViolation_TotalDurationLessThan9Hours, new Tuple<string, ViolationLevel>("Time duration violation: Total work-time less than 8.5 hours.", ViolationLevel.Severe)},
            { WorkTimeViolationType.TimeDurationViolation_RecreationLunchTimeMoreThan45Min, new Tuple<string, ViolationLevel>("Time duration violation: Recreation lunch time more than 45 hours.", ViolationLevel.Medium) },
            { WorkTimeViolationType.TimeSlotViolation_RecreationLunchTime, new Tuple<string, ViolationLevel>("Time slot violation: REcreation lunch time used outside stipulated slot.", ViolationLevel.Medium) },
            { WorkTimeViolationType.TimeSlotViolation_GymnasiumUsedDuringWorkHours, new Tuple<string, ViolationLevel>("Time slot violation: Gymnasium used before 5 p.m.", ViolationLevel.Low) },
            { WorkTimeViolationType.OddAccessEvents_Recreation, new Tuple<string, ViolationLevel>("Odd number of accesses for recreation room.", ViolationLevel.Medium) },
            { WorkTimeViolationType.OddAccessEvents_Gymnasium, new Tuple<string, ViolationLevel>("Odd number of accesses for Gymnasium.", ViolationLevel.Medium) },
            { WorkTimeViolationType.OddAccessEvents_PremisesEntry,new Tuple<string, ViolationLevel>("Odd number of accesses for premises entry gate.", ViolationLevel.Medium) },
            { WorkTimeViolationType.WorkingOnLeaveDay, new Tuple<string, ViolationLevel>("Working on leave day.", ViolationLevel.Strange) },
            { WorkTimeViolationType.WorkingOnWeekend, new Tuple<string, ViolationLevel>("Working on weekend.", ViolationLevel.Strange) },
            { WorkTimeViolationType.WorkingOnHoliday, new Tuple<string, ViolationLevel>("Working on holiday.", ViolationLevel.Strange) },
            { WorkTimeViolationType.AbsentWithoutLeave, new Tuple<string, ViolationLevel>("Absent without leave.", ViolationLevel.Severe) },
            { WorkTimeViolationType.AbsentWithHalfDayLeave, new Tuple<string, ViolationLevel>("Absent with half day leave.", ViolationLevel.Severe) },
            { WorkTimeViolationType.LateEntry, new Tuple<string, ViolationLevel>("Late entry.", ViolationLevel.Medium) },
            { WorkTimeViolationType.EarlyLeaving, new Tuple<string, ViolationLevel>("Early leaving.", ViolationLevel.Medium) },
            { WorkTimeViolationType.StayingLate, new Tuple<string, ViolationLevel>("Staying late.", ViolationLevel.Medium) },
            { WorkTimeViolationType.StayingVeryLate, new Tuple<string, ViolationLevel>("Staying very late.", ViolationLevel.Severe) },
        };

        #endregion

        #region Constructor

        public WorkTimeViolation(WorkTimeViolationType type, List<AccessEvent> accessEvents = null)
        {
            ViolationType = type;
            AccessEvents = accessEvents;
        }

        #endregion

        #region Public methods

        static public WorkTimeViolation GetViolation(WorkTimeViolationType type, List<AccessEvent> accessEvents = null)
        {
            return new WorkTimeViolation(type, accessEvents)
            {
                Description = _violations[type].Item1,
                Level = _violations[type].Item2
            };
        }

        #endregion
    }
}

