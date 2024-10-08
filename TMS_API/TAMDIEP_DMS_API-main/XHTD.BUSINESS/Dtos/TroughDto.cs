using AutoMapper;
using Common;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using XHTD.CORE.Entities;

namespace XHTD.BUSINESS.Dtos
{
    public class TroughDto : IMapFrom, IDto
    {
        [JsonIgnore]
        public int? OrdinalNumber { get; set; }

        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public string? Name { get; set; }

        public string? Machine { get; set; }

        public double? Height { get; set; }

        public double? Width { get; set; }

        public double? Long { get; set; }

        public bool? Working { get; set; }

        public bool? Problem { get; set; }

        public bool? State { get; set; }

        public string? DeliveryCodeCurrent { get; set; }

        public double? PlanQuantityCurrent { get; set; }

        public double? CountQuantityCurrent { get; set; }

        public bool? IsPicking { get; set; }

        public string? TransportNameCurrent { get; set; }

        public DateTime? CheckInTimeCurrent { get; set; }

        public bool? IsInviting { get; set; }

        public string? LineCode { get; set; }

        public DateTime? CreateDay { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? UpdateDay { get; set; }

        public string? UpdateBy { get; set; }

        public List<TroughTypeProductDto> TypeProductReferences { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblTrough, TroughDto>().ReverseMap();
        }
    }

    public class TroughCreateUpdateDto : IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; } = null!;

        public string? Name { get; set; }

        public bool? State { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblTrough, TroughCreateUpdateDto>().ReverseMap();
        }
    }

    public class TroughUpdateTypeProductDto : IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; } = null!;

        public virtual List<TypeProductCodeDto> TypeProductReferences { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblTrough, TroughUpdateTypeProductDto>().ReverseMap();
        }
    }

    public class TroughInTroughTypeProductDto : IMapFrom, IDto
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public string? Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblTrough, TroughInTroughTypeProductDto>().ReverseMap();
        }
    }
}
