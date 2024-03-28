﻿using FluentValidation;
using SuperMarket.Application.Features.Products.Commands;
using SuperMarket.Application.Services;

namespace SuperMarket.Application.Features.Products.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator(IProductService service)
        {
            RuleFor(command => command.UpdateRequest).SetValidator(new UpdateProductRequestValidator(service));
        }
    }
}