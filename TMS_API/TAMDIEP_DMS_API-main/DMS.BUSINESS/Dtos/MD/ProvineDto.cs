using System.ComponentModel.DataAnnotations;
using AutoMapper;
using System.Text.Json.Serialization;
using System.ComponentModel;
using Common;

namespace DMS.CORE.Entities.MD
{
    public class ProvineDto : BaseMdDto, IMapFrom, IDto
    {
        [JsonIgnore]
        [Description("Số thứ tự")]
        public int OrdinalNumber { get; set; }

        [Key]
        [Description("Mã")]
        public int Id { get; set; }

        [Description("Tên")]
        public string Name { get; set; }

        [Description("Số thứ tự")]
        public int OrderNumber { get; set; }

        [JsonIgnore]
        public List<DistrictDto>? Districts { get; set; }

        [Description("Tổng số huyện")]
        public int TotalDistrict { get => Districts?.Count ?? 0; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdProvine, ProvineDto>().ReverseMap();
        }
    }
}
