using DMS.CORE.Entities.MD;
using DMS.CORE.Entities.MN;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using System.Text.Json.Serialization;
using Common;

namespace DMS.BUSINESS.Dtos.MN
{
    public class AccountCareStoreDto : BaseDto, IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public int PlanId { get; set; }

        public int PartnerId { get; set; }

        public string Contents { get; set; }

        public DateTime ActionDate { get; set; }

        public Guid RefrenceId { get; set; }

        public virtual PartnerDto Partner { get; set; }

        public virtual AccountPlanVisitLittleDto AccountPlanVisit { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnAccountCareStore, AccountCareStoreDto>().ReverseMap();
        }
    }

    public class AccountCareStoreCreateDto : IMapFrom, IDto
    {
        public int PlanId { get; set; }

        public int PartnerId { get; set; }

        public string Contents { get; set; }

        public DateTime ActionDate { get; set; }

        [JsonIgnore]
        public Guid RefrenceId { get; set; } = Guid.NewGuid();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnAccountCareStore, AccountCareStoreCreateDto>().ReverseMap();
        }
    }

    public class AccountCareStoreUpdateDto : IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public int PlanId { get; set; }

        public int PartnerId { get; set; }

        public string Contents { get; set; }

        public DateTime ActionDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnAccountCareStore, AccountCareStoreUpdateDto>().ReverseMap();
        }
    }
}
