using AutoMapper;
using Common;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using XHTD.CORE.Entities;

namespace XHTD.BUSINESS.Dtos
{
    public class TroughTypeProductDto : IMapFrom, IDto
    {
        [JsonIgnore]
        public int? OrdinalNumber { get; set; }

        public int Id { get; set; }

        public string TroughCode { get; set; } = null!;

        public string TypeProduct { get; set; } = null!;

        public DateTime? CreateDay { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? UpdateDay { get; set; }

        public string? UpdateBy { get; set; }

        public virtual TroughInTroughTypeProductDto Trough { get; set; }

        public virtual TypeProductInTroughTypeProductDto TypeProductITem { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblTroughTypeProduct, TroughTypeProductDto>().ReverseMap();
        }
    }

    public class TroughTypeProductCreateUpdateDto : IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public string TroughCode { get; set; } = null!;

        public string TypeProduct { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblTroughTypeProduct, TroughTypeProductCreateUpdateDto>().ReverseMap();
        }
    }
}
