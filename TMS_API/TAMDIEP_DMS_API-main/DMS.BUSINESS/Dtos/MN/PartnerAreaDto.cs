using AutoMapper;
using DMS.CORE.Entities.MN;
using DMS.CORE.Entities.MD;
using System.ComponentModel.DataAnnotations;
using Common;

namespace DMS.BUSINESS.Dtos.MN
{
    public class PartnerAreaDto : BaseDto, IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public int AreaId { get; set; }

        public int PartnerId { get; set; }

        public virtual AreaDto Area { get; set; }

        public virtual PartnerDto Partner { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnPartnerArea, PartnerAreaDto>().ReverseMap();
        }
    }

    public class PartnerAreaWithPartnerDto : IMapFrom, IDto
    {
        public int PartnerId { get; set; }

        public virtual PartnerDto Partner { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnPartnerArea, PartnerAreaWithPartnerDto>().ReverseMap();
        }
    }

    public class PartnerAreaWithAreaDto : IMapFrom, IDto
    {
        public int AreaId { get; set; }

        public virtual AreaDto Area { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnPartnerArea, PartnerAreaWithAreaDto>().ReverseMap();
        }
    }

    public class PartnerAreaWithAreaLiteDto : IMapFrom, IDto
    {
        public int AreaId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnPartnerArea, PartnerAreaWithAreaLiteDto>().ReverseMap();
        }
    }
}
