using AutoMapper;
using TimeReporting.Core.Entities;

namespace TimeReporting.Core.DTOs
{
    public class MapperProfile :Profile

    {
        public MapperProfile()
        {
            CreateMap<DTOUser, User>().ReverseMap();
            CreateMap<DTOLeaveRequest, LeaveRequest>().ReverseMap();
            //CreateMap<DTOWorkLog, WorkLog>().ReverseMap();
        }

    }
}
