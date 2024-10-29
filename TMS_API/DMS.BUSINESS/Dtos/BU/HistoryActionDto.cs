using AutoMapper;
using Common;
using DMS.CORE.Entities.BU;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DMS.BUSINESS.Dtos.BU
{
    public class HistoryActionDto : BaseMdDto, IMapFrom, IDto
    {
        [Description("STT")]
        public int OrdinalNumber { get; set; }

        [Key]
        [Description("Code")]
        public string Code { get; set; }

        [Description("Hành động")]
        public string Action { get; set; }

        [Description("Nội dung")]
        public string Contents { get; set; }

        [Description("Trạng thái")]
        public string State { get => this.IsActive == true ? "Đang hoạt động" : "Khóa"; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblBuHistoryAction, HistoryActionDto>().ReverseMap();
        }
    }
}
