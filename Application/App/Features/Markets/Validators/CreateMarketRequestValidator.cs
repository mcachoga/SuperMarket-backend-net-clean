﻿using FluentValidation;
using SuperMarket.Shared.Requests.Catalog;

namespace SuperMarket.Application.Features.Markets.Validators
{
    public class CreateMarketRequestValidator : AbstractValidator<CreateMarketRequest>
    {
        public CreateMarketRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                    .WithMessage("{PropertyName} is required.")
                .MaximumLength(60)
                    .WithMessage("{PropertyName} can not be greater than {MaxLength} characters long.");
        }
    }
}