using AutoMapper;
using Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using XHTD.CORE.Entities;

namespace XHTD.BUSINESS.Dtos
{
    public class TypeProductDto : IMapFrom, IDto
    {
        [JsonIgnore]
        public int? OrdinalNumber { get; set; }

        public int Id { get; set; }

        [Description("Mã")]
        public string Code { get; set; } = null!;

        [Description("Tên")]
        public string? Name { get; set; }

        public bool? State { get; set; }

        public DateTime? CreateDay { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? UpdateDay { get; set; }

        public string? UpdateBy { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblTypeProduct, TypeProductDto>().ReverseMap();
        }
    }

    public class TypeProductCreateDto : IMapFrom, IDto
    {
        public string Code { get; set; } = null!;

        public string Name { get; set; }

        public bool? State { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblTypeProduct, TypeProductCreateDto>().ReverseMap();
        }
    }

    public class TypeProductUpdateDto : IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; }

        public string Name { get; set; }

        public bool? State { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblTypeProduct, TypeProductUpdateDto>().ReverseMap();
        }
    }

    public class TypeProductCodeDto : IMapFrom, IDto
    {
        [Key]
        public string TypeProduct { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblTroughTypeProduct, TypeProductCodeDto>().ReverseMap();
        }
    }

    public class TypeProductInTroughTypeProductDto : IMapFrom, IDto
    {
        public string Code { get; set; } = null!;

        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblTypeProduct, TypeProductInTroughTypeProductDto>().ReverseMap();
        }
    }
}
