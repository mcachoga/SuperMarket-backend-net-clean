using AutoMapper;
using MediatR;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Infrastructure.Framework.Validations;
using SuperMarket.Shared.Requests.Catalog;
using SuperMarket.Shared.Responses.Catalog;

namespace SuperMarket.Application.Features.Markets.Commands
{
    public class UpdateMarketCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public UpdateMarketRequest UpdateRequest { get; set; }
    }

    public class UpdateMarketCommandHandler : IRequestHandler<UpdateMarketCommand, IResponseWrapper>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateMarketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(UpdateMarketCommand request, CancellationToken cancellationToken)
        {
            var currentEntity = await _unitOfWork.Markets.GetByIdAsync(request.UpdateRequest.Id);

            if (currentEntity is not null)
            {
                currentEntity.Name = request.UpdateRequest.Name;

                var updatedEntity = await _unitOfWork.Markets.UpdateAsync(currentEntity);
                await _unitOfWork.Commit(cancellationToken);
                var model = _mapper.Map<MarketResponse>(updatedEntity);

                return await ResponseWrapper<MarketResponse>.SuccessAsync(model, "Market updated successfully.");
            }

            return await ResponseWrapper<MarketResponse>.FailAsync("Market does not exist.");
        }
    }
}