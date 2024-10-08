using System.ComponentModel.DataAnnotations;
using AutoMapper;
using System.Text.Json.Serialization;
using System.ComponentModel;
using DMS.BUSINESS.Dtos.MN;
using Common;

namespace DMS.CORE.Entities.MD
{
    public class AreaDto : BaseMdDto, IMapFrom, IDto
    {
        [JsonIgnore]
        [Description("Số thứ tự")]
        public int OrdinalNumber { get; set; }

        [Key]
        [Description("Mã")]
        public int Id { get; set; }

        [Description("Tên")]
        public string Name { get; set; }

        public List<AreaItemWithItemDto>? ItemReferences { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdArea, AreaDto>().ReverseMap();
        }
    }

    public class AreaCreateUpdateDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<AreaItemCreateWithItemDto>? ItemReferences { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdArea, AreaCreateUpdateDto>().ReverseMap();
        }
    }

}
