using AutoMapper;
using DMS.CORE.Entities.MN;
using Common;
using System.ComponentModel.DataAnnotations;

namespace DMS.BUSINESS.Dtos.MN
{
    public class ContractDto :  IMapFrom, IDto
    {
        [Key]
        public string ContractId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnContract, ContractDto>().ReverseMap();
        }
    }
}
