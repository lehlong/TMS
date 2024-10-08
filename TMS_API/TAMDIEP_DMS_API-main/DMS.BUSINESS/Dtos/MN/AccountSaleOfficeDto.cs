using AutoMapper;
using DMS.BUSINESS.Dtos.AD;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DMS.CORE.Entities.MN;
using Common;

namespace DMS.BUSINESS.Dtos.MN
{
    public class AccountSaleOfficeDto : IMapFrom, IDto
    {
        [Key]
        public string UserName { get; set; }

        [Key]
        public int PartnerId { get; set; }

        public virtual AccountDto Account { get; set; }

        [JsonIgnore]
        public virtual PartnerSaleOfficeDto Partner { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnAccountSaleOffice, AccountSaleOfficeDto>().ReverseMap();
        }
    }

    public class AccountSaleOfficeUpdateDto
    {
        public string UserName { get; set; }

        public int[] PartnerIds { get; set; }
    }
}
