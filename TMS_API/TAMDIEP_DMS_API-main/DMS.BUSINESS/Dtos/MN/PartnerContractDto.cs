using AutoMapper;
using DMS.CORE.Entities.MN;
using Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DMS.CORE.Entities.MD;

namespace DMS.BUSINESS.Dtos.MN
{
    public class PartnerContractDto : IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public int PartnerId { get; set; }

        public string ContractId { get; set; }

        public ContractDto Contract { get; set; }   

        public PartnerDto Partner { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnPartnerContract, PartnerContractDto>().ReverseMap();
        }
    }

    public class PartnerContractWithPartnerDto : IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public int PartnerId { get; set; }

        public PartnerDto Partner { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnPartnerContract, PartnerContractWithPartnerDto>().ReverseMap();
        }
    }
}
