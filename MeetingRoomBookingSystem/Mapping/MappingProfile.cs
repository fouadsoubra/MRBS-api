using AutoMapper;
using MeetingRoomBookingSystem.Resources;
using MRBS.Core.Models;

namespace MeetingRoomBookingSystem.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Resource
            CreateMap<Company, CompanyResource>();
            CreateMap<User, UserResource>();
            CreateMap<Room, RoomResource>();
            CreateMap<Reservation, ReservationResource>();

            // Resource to Domain
            CreateMap<CompanyResource, Company>();
            CreateMap<SaveCompanyResource, Company>();

            CreateMap<UserResource, User>();
            CreateMap<SaveUserResource, User>();
            CreateMap<SaveUserCredentialResource, User>();
            CreateMap<UserLoginRequest, User>();

            CreateMap<RoomResource, Room>();
            CreateMap<SaveRoomResource, Room>();

            CreateMap<ReservationResource, Reservation>();
            CreateMap<SaveReservationResource, Reservation>();


        }
    }
}
