using AutoMapper;
using IncidentManagment.Data.Models;
using IncidentManagment.DTOs;

internal class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<IncidentCreationDTO, Contact>()
            .ForMember(n => n.Email, opt => opt.MapFrom(t => t.cEmail))
            .ForMember(n => n.FirstName, opt => opt.MapFrom(t => t.cFirstName))
            .ForMember(n => n.LastName, opt => opt.MapFrom(t => t.cLastName))
            .ForMember(n => n.Accounts, opt => opt.MapFrom(t => new List<Account>()));

        CreateMap<IncidentCreationDTO, Incident>()
            .ForMember(n => n.Description, opt => opt.MapFrom(t => t.incidentDescription));

        CreateMap<AccountDTO, Account>().ReverseMap();
        CreateMap<IncidentDTO, Incident>().ReverseMap();
        CreateMap<ContactDTO, Contact>().ReverseMap();    

    }
}