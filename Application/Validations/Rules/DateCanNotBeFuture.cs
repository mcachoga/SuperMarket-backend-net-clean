using FluentValidation;
using FluentValidation.Validators;

namespace SuperMarket.Application.Validations.Rules
{
    public class DateCanNotBeFutureValidator<T> : PropertyValidator<T, DateTime>
    {
        public override string Name => "DateCanNotBeFutureValidator";

        public override bool IsValid(ValidationContext<T> context, DateTime value)
        {
            var valueUtc = DateTime.SpecifyKind(value, DateTimeKind.Utc);

            if (valueUtc <= DateTime.UtcNow)
            {
                return true;
            }

            context.AddFailure("Date can not be future.");
            return false;
        }
    }
}