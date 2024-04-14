using AutoMapper;
using SuperMarket.Domain.Auth;
using SuperMarket.Shared.Responses.Identity;

namespace SuperMarket.Application.Features.Auth.Mappings
{
    internal class MappingAuthProfile : Profile
    {
        public MappingAuthProfile()
        {
            CreateMap<TokenAuth, TokenResponse>();
            CreateMap<TokenResponse, TokenAuth>();
        }
    }
}