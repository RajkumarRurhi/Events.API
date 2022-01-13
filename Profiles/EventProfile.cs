using AutoMapper;
using Events.API.Entities;
using Events.API.Models;

namespace Events.API.Profiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            this.CreateMap<Event, EventModel>().ReverseMap();
            this.CreateMap<Event, EventForUpdateModel>();
            this.CreateMap<EventForCreationModel, Event>()
                .ForMember(dest => dest.Guests, opt => opt.MapFrom(src => src.Guests));
            this.CreateMap<EventForCreationDtoAddress, Event>()
                .ForMember(dest => dest.Guests, opt => opt.MapFrom(src => src.Guests));
            this.CreateMap<EventForUpdateModel, Event>()
                .ForMember(dest => dest.Guests, opt => opt.MapFrom(src => src.Guests));
            this.CreateMap<Event, EventFullDto>();

        }
    }
}
