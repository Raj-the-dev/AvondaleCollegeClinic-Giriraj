using System;
using System.ComponentModel.DataAnnotations;

namespace AvondaleCollegeClinic.Validation
{
    // Validates that a date or date-time is NOT in the past.
    // Works with both DateTime and DateOnly.
    public sealed class NotPastAttribute : ValidationAttribute
    {
        // Default error message if you do not override it where you use the attribute.
        public NotPastAttribute() : base("Date/time cannot be in the past.") { }

        // Framework calls this to check if the value is valid.
        public override bool IsValid(object value)
        {
            // If the field is empty, let other attributes like [Required] handle it.
            if (value == null) return true;

            // Capture "now" once so the comparison is consistent.
            var now = DateTime.Now;

            // Use a switch expression to handle different supported types.
            // For DateTime: require dt >= now.
            // For DateOnly: convert to a DateTime at midnight and compare to today's date.
            // For any other type: do nothing and pass.
            return value switch
            {
                DateTime dt => dt >= now,
                DateOnly d => d.ToDateTime(new TimeOnly(0, 0)) >= now.Date,
                _ => true
            };
        }
    }

    // Validates that a date or date-time is NOT in the future.
    // Supports DateTime and DateOnly.
    public sealed class NotFutureAttribute : ValidationAttribute
    {
        public NotFutureAttribute() : base("Date/time cannot be in the future.") { }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var now = DateTime.Now;

            // For DateTime: require dt <= now.
            // For DateOnly: convert to the end of that day so "today" passes.
            return value switch
            {
                DateTime dt => dt <= now,
                DateOnly d => d.ToDateTime(new TimeOnly(23, 59, 59)) <= now,
                _ => true
            };
        }
    }

    // Validates that a date is within N days ahead from now.
    // Inclusive of the limit.
    public sealed class WithinNextDaysAttribute : ValidationAttribute
    {
        private readonly int _days;

        // Store the day window and provide a default message that uses the number.
        public WithinNextDaysAttribute(int days) : base($"Date/time must be within the next {days} days.") => _days = days;

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var now = DateTime.Now;
            var limit = now.AddDays(_days);

            // For DateTime: require dt <= limit.
            // For DateOnly: convert to end of day and compare.
            return value switch
            {
                DateTime dt => dt <= limit,
                DateOnly d => d.ToDateTime(new TimeOnly(23, 59, 59)) <= limit,
                _ => true
            };
        }
    }

    // Rejects Saturday and Sunday.
    // Accepts Monday through Friday.
    public sealed class NotWeekendAttribute : ValidationAttribute
    {
        public NotWeekendAttribute() : base("Weekends are not allowed.") { }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            // Read the day of week depending on the type.
            DayOfWeek dow = value switch
            {
                DateTime dt => dt.DayOfWeek,
                DateOnly d => d.DayOfWeek,
                // If it is some other type, treat it as a weekday so validation does not break.
                _ => DayOfWeek.Monday
            };

            // Fail for Saturday or Sunday. Pass otherwise.
            return dow != DayOfWeek.Saturday && dow != DayOfWeek.Sunday;
        }
    }

    // Validates that a date of birth falls within an age range.
    // Computes the age as of today with a correct birthday check.
    public sealed class AgeRangeAttribute : ValidationAttribute
    {
        private readonly int _min;
        private readonly int _max; // If not provided, becomes int.MaxValue.

        // Minimum only constructor.
        public AgeRangeAttribute(int min) : this(min, int.MaxValue) { }

        // Minimum and maximum constructor.
        public AgeRangeAttribute(int min, int max) : base($"Age must be between {min} and {max} years.")
        {
            _min = min;
            _max = max;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            // Convert to a DateTime for calculation. If type is unsupported, bail out and pass.
            DateTime dob = value switch
            {
                DateTime dt => dt.Date,
                DateOnly d => d.ToDateTime(new TimeOnly(0, 0)),
                _ => DateTime.MinValue
            };
            if (dob == DateTime.MinValue) return true;

            // Standard age calculation with birthday check.
            var today = DateTime.Today;
            int age = today.Year - dob.Year;

            // If birthday has not occurred yet this year, subtract one.
            if (dob.Date > today.AddYears(-age)) age--;

            // Pass only if age is within the inclusive range.
            return age >= _min && age <= _max;
        }
    }

    // Validates that the annotated property is NOT before another property.
    // Typical use: EndDate must be >= StartDate.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NotBeforeAttribute : ValidationAttribute
    {
        // Name of the other property to compare to.
        public string OtherPropertyName { get; }

        public NotBeforeAttribute(string otherPropertyName) : base("End date cannot be before start date.")
            => OtherPropertyName = otherPropertyName;

        // We override the ValidationAttribute method that provides context.
        // Context lets us read the other property value using reflection.
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            // If the value is empty, do not block. Let [Required] handle presence.
            if (value == null) return ValidationResult.Success;

            // Try to get the other property by name on the same object.
            var otherProp = context.ObjectType.GetProperty(OtherPropertyName);
            if (otherProp == null) return ValidationResult.Success;

            // Read the other property's value from the current object instance.
            var otherVal = otherProp.GetValue(context.ObjectInstance);

            // Normalize both values to DateTime so we can compare.
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

            // If both dates exist and the current value is earlier than the other date, fail.
            // Compare only the date part to ignore time-of-day noise.
            if (thisDate.HasValue && otherDate.HasValue && thisDate.Value.Date < otherDate.Value.Date)
                return new ValidationResult(ErrorMessage);

            // Otherwise pass.
            return ValidationResult.Success;
        }
    }
}
