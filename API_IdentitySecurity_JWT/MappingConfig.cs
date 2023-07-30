using API_IdentitySecurity_JWT.DTO;
using API_IdentitySecurity_JWT.Models;
using AutoMapper;

namespace API_IdentitySecurity_JWT
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Registration,RegistrationDTO>().ReverseMap();
        }
    }
}
