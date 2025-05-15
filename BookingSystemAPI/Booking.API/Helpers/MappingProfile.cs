using AutoMapper;
using Booking.Core.DTOs.Authentication;
using Booking.Core.DTOs.Category;
using Booking.Core.DTOs.Event;
using Booking.Core.Enities;
using Booking.Core.Enities.Authentication;

namespace Booking.API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Category, CategoryDTO>()
                .ReverseMap();

            CreateMap<CreateEventDTO, Event>().ForMember(src => src.Image, opt => opt.Ignore());
            CreateMap<UpdateEventDTO, Event>().ForMember(src => src.Image, opt => opt.Ignore());

            CreateMap<RegisterDTO, ApplicationUser>();
        }
    }
}
