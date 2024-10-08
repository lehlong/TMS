using System.ComponentModel.DataAnnotations;
using AutoMapper;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Common;

namespace DMS.CORE.Entities.MD
{
    public class UnitDto : BaseMdDto, IMapFrom, IDto
    {
        [JsonIgnore]
        [Description("STT")]
        public int OrdinalNumber { get; set; }

        [Key]
        [Description("Mã đơn vị tính")]
        public string Id { get; set; }

        [Description("Tên đơn vị tính")]
        public string Name { get; set; }

        [Description("Trạng thái")]
        public string State { get => this.IsActive == true ? "Đang hoạt động" : "Khóa"; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdUnit, UnitDto>().ReverseMap();
        }
    }
}
