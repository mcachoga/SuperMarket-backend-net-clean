using FluentValidation;

namespace SuperMarket.Application.Validations.Rules
{
    public static class DateCanNotBeFutureValidatorExtensions
    {
        public static IRuleBuilderOptions<T, DateTime> CanNotBeFuture<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new DateCanNotBeFutureValidator<T>());
        }
    }
}