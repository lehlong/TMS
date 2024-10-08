using AutoMapper;
using Common;
using System.ComponentModel.DataAnnotations;
using XHTD.CORE.Entities;

namespace XHTD.BUSINESS.Dtos
{
    public class SystemParameterDto : IMapFrom, IDto
    {
        public int? OrdinalNumber { get; set; }

        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public string? Name { get; set; }

        public string? Value { get; set; }

        public DateTime? CreateDay { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? UpdateDay { get; set; }

        public string? UpdateBy { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblSystemParameter, SystemParameterDto>().ReverseMap();
        }
    }

    public class SystemParameterCreateUpdateDto : IMapFrom, IDto
    {
        [Key]
        public string Code { get; set; } = null!;

        public string? Name { get; set; }

        public string? Value { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblSystemParameter, SystemParameterCreateUpdateDto>().ReverseMap();
        }
    }
}
