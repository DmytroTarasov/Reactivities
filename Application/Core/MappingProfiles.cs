using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Activities;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>();
            CreateMap<Activity, ActivityDto>()
                .ForMember(ad => ad.HostUsername, o => o.MapFrom(a => 
                    a.Attendees.FirstOrDefault(aa => aa.IsHost).AppUser.UserName)); // o - options
            CreateMap<ActivityAttendee, AttendeeDto>()
                .ForMember(p => p.DisplayName, o => o.MapFrom(aa => aa.AppUser.DisplayName))
                .ForMember(p => p.Username, o => o.MapFrom(aa => aa.AppUser.UserName))
                .ForMember(p => p.Bio, o => o.MapFrom(aa => aa.AppUser.Bio))
                .ForMember(p => p.Image, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url));;
            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(p => p.Image, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}