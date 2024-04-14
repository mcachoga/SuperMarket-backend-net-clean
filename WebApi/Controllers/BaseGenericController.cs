using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SuperMarket.WebApi.Controllers
{
    [ApiController]
    public class BaseGenericController<T> : ControllerBase
    {
        private ISender _sender;
        private ILogger<T> _loggerInstance;

        public ISender MediatorSender => _sender ??= HttpContext.RequestServices.GetService<ISender>();
        
        protected ILogger<T> Logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
    }
}