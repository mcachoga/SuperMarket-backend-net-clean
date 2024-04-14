using AutoMapper;
using MediatR;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Infrastructure.Framework.Validations;
using SuperMarket.Shared.Requests.Catalog;
using SuperMarket.Shared.Responses.Catalog;

namespace SuperMarket.Application.Features.Orders.Commands
{
    public class UpdateOrderCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public UpdateOrderRequest UpdateRequest { get; set; }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, IResponseWrapper>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var currentEntity = await _unitOfWork.Orders.GetByIdAsync(request.UpdateRequest.Id);

            if (currentEntity is not null)
            {
                currentEntity.OrderDate = request.UpdateRequest.OrderDate;
                currentEntity.Price = request.UpdateRequest.Price;
                currentEntity.MarketId = request.UpdateRequest.MarketId;
                currentEntity.ProductId = request.UpdateRequest.ProductId;
                
                var updatedEntity = await _unitOfWork.Orders.UpdateAsync(currentEntity);
                await _unitOfWork.Commit(cancellationToken);

                var model = _mapper.Map<OrderResponse>(updatedEntity);

                return await ResponseWrapper<OrderResponse>.SuccessAsync(model, "Order updated successfully.");
            }

            return await ResponseWrapper<ProductResponse>.FailAsync("Order does not exist.");
        }
    }
}