using System;
using System.ComponentModel.DataAnnotations;

namespace AvondaleCollegeClinic.Validation
{
    // Not in the past (date OR datetime)
    public sealed class NotPastAttribute : ValidationAttribute
    {
        public NotPastAttribute() : base("Date/time cannot be in the past.") { }
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            var now = DateTime.Now;
            return value switch
            {
                DateTime dt => dt >= now,
                DateOnly d => d.ToDateTime(new TimeOnly(0, 0)) >= now.Date,
                _ => true
            };
        }
    }

    // Not in the future (date OR datetime)
    public sealed class NotFutureAttribute : ValidationAttribute
    {
        public NotFutureAttribute() : base("Date/time cannot be in the future.") { }
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            var now = DateTime.Now;
            return value switch
            {
                DateTime dt => dt <= now,
                DateOnly d => d.ToDateTime(new TimeOnly(23, 59, 59)) <= now,
                _ => true
            };
        }
    }

    // Must be within N days ahead (inclusive)
    public sealed class WithinNextDaysAttribute : ValidationAttribute
    {
        private readonly int _days;
        public WithinNextDaysAttribute(int days) : base($"Date/time must be within the next {days} days.") => _days = days;
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            var now = DateTime.Now;
            var limit = now.AddDays(_days);
            return value switch
            {
                DateTime dt => dt <= limit,
                DateOnly d => d.ToDateTime(new TimeOnly(23, 59, 59)) <= limit,
                _ => true
            };
        }
    }

    // Reject Saturday/Sunday
    public sealed class NotWeekendAttribute : ValidationAttribute
    {
        public NotWeekendAttribute() : base("Weekends are not allowed.") { }
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            DayOfWeek dow = value switch
            {
                DateTime dt => dt.DayOfWeek,
                DateOnly d => d.DayOfWeek,
                _ => DayOfWeek.Monday
            };
            return dow != DayOfWeek.Saturday && dow != DayOfWeek.Sunday;
        }
    }

    // Age range in years (DOB)
    public sealed class AgeRangeAttribute : ValidationAttribute
    {
        private readonly int _min;
        private readonly int _max; // optional (=int.MaxValue if not set)
        public AgeRangeAttribute(int min) : this(min, int.MaxValue) { }
        public AgeRangeAttribute(int min, int max) : base($"Age must be between {min} and {max} years.") { _min = min; _max = max; }
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            DateTime dob = value switch
            {
                DateTime dt => dt.Date,
                DateOnly d => d.ToDateTime(new TimeOnly(0, 0)),
                _ => DateTime.MinValue
            };
            if (dob == DateTime.MinValue) return true;

            var today = DateTime.Today;
            int age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age >= _min && age <= _max;
        }
    }

    // End >= Start (for ranges)
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NotBeforeAttribute : ValidationAttribute
    {
        public string OtherPropertyName { get; }
        public NotBeforeAttribute(string otherPropertyName) : base("End date cannot be before start date.")
            => OtherPropertyName = otherPropertyName;

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null) return ValidationResult.Success;

            var otherProp = context.ObjectType.GetProperty(OtherPropertyName);
            if (otherProp == null) return ValidationResult.Success;

            var otherVal = otherProp.GetValue(context.ObjectInstance);

            DateTime? thisDate = value switch
            {
                DateTime dt => dt,
                DateOnly d => d.ToDateTime(new TimeOnly(0, 0)),
                _ => null
            };
            DateTime? otherDate = otherVal switch
            {
                DateTime dt => dt,
                DateOnly d => d.ToDateTime(new TimeOnly(0, 0)),
                _ => null
            };

            if (thisDate.HasValue && otherDate.HasValue && thisDate.Value.Date < otherDate.Value.Date)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
