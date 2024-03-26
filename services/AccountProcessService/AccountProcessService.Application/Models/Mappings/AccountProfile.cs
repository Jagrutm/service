using AccountProcessService.Domain.Entities;
using AutoMapper;

namespace AccountProcessService.Application.Models.Mappings
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<CreateAccountDto, Account>();
            CreateMap<UpdateAccountDto, Account>();
        }
    }
}
