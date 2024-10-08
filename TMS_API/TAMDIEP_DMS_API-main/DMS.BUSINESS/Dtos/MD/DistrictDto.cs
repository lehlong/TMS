using AutoMapper;
using Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DMS.CORE.Entities.MD
{
    public class DistrictDto : BaseMdDto, IMapFrom, IDto
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

        public int? ProvineId { get; set; }

        [Description("Tổng số xã")]
        public int TotalWard { get => Wards?.Count ?? 0; }

        [JsonIgnore]
        public virtual ProvineDto? Provine { get; set; }

        [JsonIgnore]
        public virtual List<TblMdWard>? Wards { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdDistrict, DistrictDto>().ReverseMap();
        }
    }
}
