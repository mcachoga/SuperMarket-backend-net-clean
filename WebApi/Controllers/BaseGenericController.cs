﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SuperMarket.WebApi.Controllers
{
    [ApiController]
    public class BaseGenericController<T> : ControllerBase
    {
        private ISender _sender;
        public ISender MediatorSender => _sender ??= HttpContext.RequestServices.GetService<ISender>();

        private ILogger<T> _loggerInstance;
        protected ILogger<T> _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
    }
}