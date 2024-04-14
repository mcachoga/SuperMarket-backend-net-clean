using AutoMapper;
using MediatR;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Domain;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Infrastructure.Framework.Validations;
using SuperMarket.Shared.Requests.Catalog;
using SuperMarket.Shared.Responses.Catalog;

namespace SuperMarket.Application.Features.Orders.Commands
{
    public class CreateOrderCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CreateOrderRequest CreateRequest { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, IResponseWrapper>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<Order>(request.CreateRequest);
            var newEntity = await _unitOfWork.Orders.InsertAsync(model);
            await _unitOfWork.Commit(cancellationToken);

            if (newEntity.Id > 0)
            {
                var newModel = _mapper.Map<OrderResponse>(newEntity);
                return await ResponseWrapper<OrderResponse>.SuccessAsync(newModel, "Order created Successfully.");
            }

            return await ResponseWrapper.FailAsync("Failed to create order entry.");
        }
    }
}