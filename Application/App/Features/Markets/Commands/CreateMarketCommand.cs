using AutoMapper;
using MediatR;
using SuperMarket.Application.Services.Contracts;
using SuperMarket.Domain;
using SuperMarket.Infrastructure.Framework.Responses;
using SuperMarket.Infrastructure.Framework.Validations;
using SuperMarket.Shared.Requests.Catalog;
using SuperMarket.Shared.Responses.Catalog;

namespace SuperMarket.Application.Features.Markets.Commands
{
    public class CreateMarketCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CreateMarketRequest CreateRequest { get; set; }
    }

    public class CreateMarketCommandHandler : IRequestHandler<CreateMarketCommand, IResponseWrapper>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateMarketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResponseWrapper> Handle(CreateMarketCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Market>(request.CreateRequest);
            var newEntity = await _unitOfWork.Markets.InsertAsync(entity);
            await _unitOfWork.Commit(cancellationToken);

            if (newEntity.Id > 0)
            {
                var newModel = _mapper.Map<MarketResponse>(newEntity);
                return await ResponseWrapper<MarketResponse>.SuccessAsync(newModel, "Market created Successfully.");
            }

            return await ResponseWrapper.FailAsync("Failed to create market entry.");
        }
    }
}