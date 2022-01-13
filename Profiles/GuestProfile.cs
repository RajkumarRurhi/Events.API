using AutoMapper;
using Events.API.Entities;
using Events.API.Helpers;
using Events.API.Models;

namespace Events.API.Profiles
{
    public class GuestProfile : Profile
    {
        public GuestProfile()
        {
            this.CreateMap<PersonEvent, GuestModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.PersonEmail))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Person.DateOfBirth.GetAge()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Person.FirstName} {src.Person.LastName}"));
                
            this.CreateMap<Person, GuestModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => $"{src.DateOfBirth.GetAge()}"));

            this.CreateMap<GuestForAddingModel, PersonEvent>()
                .ForMember(dest => dest.PersonEmail, opt => opt.MapFrom(src => src.Email));
                //.ForPath(dest => dest.Person.Email, opt => opt.MapFrom(src => src.Email))
                //.ForPath(dest => dest.Person.FirstName, opt => opt.MapFrom(src => src.FirstName))
                //.ForPath(dest => dest.Person.LastName, opt => opt.MapFrom(src => src.LastName))
                //.ForPath(dest => dest.Person.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            this.CreateMap<GuestForAddingModel, Person>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            this.CreateMap<GuestForUpdateModel, PersonEvent>()
                .ForMember(dest => dest.PersonEmail, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.Person.Email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.Person.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForPath(dest => dest.Person.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForPath(dest => dest.Person.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            this.CreateMap<GuestForUpdateModel, Person>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            this.CreateMap<PersonEvent, GuestForUpdateModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Person.Email))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Person.DateOfBirth))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Person.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Person.LastName));

            this.CreateMap<Person, GuestForUpdateModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

            this.CreateMap<PersonEvent, GuestFullDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Person.Email))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Person.DateOfBirth))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Person.LastName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Person.FirstName));

            this.CreateMap<Person, GuestFullDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName));
        }
    }
}
