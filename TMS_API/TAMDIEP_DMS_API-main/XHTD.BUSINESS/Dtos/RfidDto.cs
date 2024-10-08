using AutoMapper;
using Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using XHTD.CORE.Entities;

namespace XHTD.BUSINESS.Dtos
{
    public class RfidDto : IMapFrom, IDto
    {
        [JsonIgnore]
        public int? OrdinalNumber { get; set; }

        public int Id { get; set; }

        [Key]
        [Description("Mã")]
        public string Code { get; set; } = null!;

        [Description("BSX")]
        public string? Vehicle { get; set; }

        public DateTime? DayReleased { get; set; }

        public DateTime? DayExpired { get; set; }

        public string? Note { get; set; }

        public bool? State { get; set; }

        public DateTime? LastEnter { get; set; }

        public DateTime? CreateDay { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? UpdateDay { get; set; }

        public string? UpdateBy { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblRfid, RfidDto>().ReverseMap();
        }
    }

    public class RfidCreateDto : IMapFrom, IDto
    {
        public string Code { get; set; } = null!;

        public string? Vehicle { get; set; }

        public DateTime? DayReleased { get; set; }

        public DateTime? DayExpired { get; set; }

        public string? Note { get; set; }

        public bool? State { get; set; }

        public DateTime? LastEnter { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblRfid, RfidCreateDto>().ReverseMap();
        }
    }

    public class RfidUpdateDto : IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; } = null!;

        public string? Vehicle { get; set; }

        public DateTime? DayReleased { get; set; }

        public DateTime? DayExpired { get; set; }

        public string? Note { get; set; }

        public bool? State { get; set; }

        public DateTime? LastEnter { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblRfid, RfidUpdateDto>().ReverseMap();
        }
    }
}
