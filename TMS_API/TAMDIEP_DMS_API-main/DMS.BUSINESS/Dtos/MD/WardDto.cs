using AutoMapper;
using Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DMS.CORE.Entities.MD
{
    public class WardDto : BaseMdDto, IMapFrom, IDto
    {
        [JsonIgnore]
        [Description("Stt")]
        public int OrdinalNumber { get; set; }

        [Key]
        [Description("Mã")]
        public int Id { get; set; }

        [Description("Tên")]
        public string Name { get; set; }

        [Description("Số thứ tự")]
        public int OrderNumber { get; set; }

        public int? DistrictId { get; set; }

        [JsonIgnore]
        public virtual DistrictDto? District { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdWard, WardDto>().ReverseMap();
        }
    }
}
