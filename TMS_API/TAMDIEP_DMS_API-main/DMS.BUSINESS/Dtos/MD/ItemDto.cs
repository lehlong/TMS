using AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel;
using Common;

namespace DMS.CORE.Entities.MD
{
    public class ItemDto : BaseMdDto, IMapFrom, IDto
    {
        [JsonIgnore]
        [Description("STT")]
        public int OrdinalNumber { get; set; }

        [Key]
        [Description("Mã sản phẩm")]
        public string Code { get; set; }

        [Description("Tên sản phẩm")]
        public string Name { get; set; }

        public string? GroupCode { get; set; }

        public string? TypeCode { get; set; }

        [JsonIgnore]
        [Description("Loại sp")]
        public string TypeName { get => Type?.Name; }

        [JsonIgnore]
        [Description("Nhóm sp")]
        public string GroupName { get => Group?.Name; }

        public virtual GroupItemDto Group { get; set; }

        public virtual TypeItemDto Type { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdItem, ItemDto>().ReverseMap();
        }
    }

    public class ItemCreateUpdateDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; }

        public string Name { get; set; }

        public string? GroupCode { get; set; }

        public string? TypeCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdItem, ItemCreateUpdateDto>().ReverseMap();
        }
    }
}
