using DMS.CORE.Entities.MD;
using DMS.CORE.Entities.MN;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using System.Text.Json.Serialization;
using Common;

namespace DMS.BUSINESS.Dtos.MN
{
    public class AccountPlanVisitStoreDto : BaseDto, IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public int PlanId { get; set; }

        public int PartnerId { get; set; }

        [JsonIgnore]
        public virtual AccountPlanVisitDto AccountPlanVisit { get; set; }

        public virtual PartnerDto Partner { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnAccountPlanVisitStore, AccountPlanVisitStoreDto>().ReverseMap();
        }
    }

}
