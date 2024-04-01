using AutoMapper;
using MediatR;
using SuperMarket.Application.Services;
using SuperMarket.Application.Validations.Pipelines;
using SuperMarket.Common.Requests.Markets;
using SuperMarket.Common.Responses.Markets;
using SuperMarket.Common.Responses.Wrappers;
using SuperMarket.Domain;

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
            var model = _mapper.Map<Market>(request.CreateRequest);
            var newEntity = await _unitOfWork.Markets.InsertAsync(model);
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