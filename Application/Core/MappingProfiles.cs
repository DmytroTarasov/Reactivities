using System.Linq;
using Application.Activities;
using Application.Comments;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // we need to get access to the name of the current user (but in the same time, we cannot
            // simply pass arguments inside a constructor - it`s not allowed)
            string currentUsername = null;
            CreateMap<Activity, Activity>();
            CreateMap<Activity, ActivityDto>()
                .ForMember(ad => ad.HostUsername, o => o.MapFrom(a => 
                    a.Attendees.FirstOrDefault(aa => aa.IsHost).AppUser.UserName)); // o - options
            CreateMap<ActivityAttendee, AttendeeDto>()
                .ForMember(p => p.DisplayName, o => o.MapFrom(aa => aa.AppUser.DisplayName))
                .ForMember(p => p.Username, o => o.MapFrom(aa => aa.AppUser.UserName))
                .ForMember(p => p.Bio, o => o.MapFrom(aa => aa.AppUser.Bio))
                .ForMember(p => p.Image, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.AppUser.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.AppUser.Followings.Count))
                .ForMember(d => d.Following, 
                    o => o.MapFrom(s => s.AppUser.Followers.Any(x => x.Observer.UserName == currentUsername)));;
            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(p => p.Image, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
                .ForMember(d => d.Following, 
                    o => o.MapFrom(s => s.Followers.Any(x => x.Observer.UserName == currentUsername)));
            CreateMap<Comment, CommentDto>()
                .ForMember(p => p.DisplayName, o => o.MapFrom(aa => aa.Author.DisplayName))
                .ForMember(p => p.Username, o => o.MapFrom(aa => aa.Author.UserName))
                .ForMember(p => p.Image, o => o.MapFrom(s => s.Author.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}