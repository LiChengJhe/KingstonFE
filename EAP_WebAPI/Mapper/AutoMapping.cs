using AutoMapper;
using EAP_Library.DTO;
using EAP_WebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EAP_WebAPI.Mapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<EqpInfo, EqpInfoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EqpId))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.EqpType)).ReverseMap();

            CreateMap<EqpAlarm, EqpAlarmInfoDTO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AlarmId))
               .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.AlarmText))
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.AlarmType)).ReverseMap();

            CreateMap<EqpStatus, EqpStatusInfoDTO>()
              .ForMember(dest => dest.CurType, opt => opt.MapFrom(src => src.EqpStatusType))
              .ForMember(dest => dest.CurTime, opt => opt.MapFrom(src => src.EqpStatusTime)).ReverseMap();

           CreateMap<EqpWip, EqpWipDTO>().ReverseMap();
        }
    }

}
