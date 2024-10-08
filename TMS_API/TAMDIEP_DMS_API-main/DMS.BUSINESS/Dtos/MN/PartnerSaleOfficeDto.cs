using AutoMapper;
using Common;
using DMS.CORE.Entities.MD;
using System.ComponentModel.DataAnnotations;

namespace DMS.BUSINESS.Dtos.MN
{
    public class PartnerSaleOfficeDto : IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMdPartner, PartnerSaleOfficeDto>().ReverseMap();
        }
    }
}
