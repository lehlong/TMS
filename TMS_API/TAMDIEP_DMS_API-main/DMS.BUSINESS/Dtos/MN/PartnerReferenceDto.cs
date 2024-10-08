using AutoMapper;
using DMS.CORE.Entities.MN;
using DMS.CORE.Entities.MD;
using System.ComponentModel.DataAnnotations;
using Common;

namespace DMS.BUSINESS.Dtos.MN
{
    public class PartnerReferenceDto : IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public int PartnerIdBuy { get; set; }

        public int PartnerIdSell { get; set; }

        public virtual PartnerDto PartnerBuy { get; set; }

        public virtual PartnerDto PartnerSell { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnPartnerReference, PartnerReferenceDto>().ReverseMap();
        }
    }

    public class PartnerReferenceBuyCreateDto : IMapFrom, IDto
    {
        public int PartnerIdSell { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnPartnerReference, PartnerReferenceBuyCreateDto>().ReverseMap();
        }
    }

    public class PartnerReferenceSellCreateDto : IMapFrom, IDto
    {
        public int PartnerIdBuy { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnPartnerReference, PartnerReferenceSellCreateDto>().ReverseMap();
        }
    }

    public class PartnerReferenceBuyDto : IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public int PartnerIdSell { get; set; }

        public virtual PartnerLiteDto PartnerSell { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnPartnerReference, PartnerReferenceBuyDto>().ReverseMap();
        }
    }

    public class PartnerReferenceSellDto : IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public int PartnerIdBuy { get; set; }

        public virtual PartnerLiteDto PartnerBuy { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnPartnerReference, PartnerReferenceSellDto>().ReverseMap();
        }
    }
}
