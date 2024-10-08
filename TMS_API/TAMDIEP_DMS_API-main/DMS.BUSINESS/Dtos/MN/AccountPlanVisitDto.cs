using DMS.CORE.Entities.MN;
using System.ComponentModel.DataAnnotations;
using DMS.BUSINESS.Dtos.AD;
using AutoMapper;
using Common;

namespace DMS.BUSINESS.Dtos.MN
{
    public class AccountPlanVisitDto : BaseDto, IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public string PlanName { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public virtual AccountDto Account { get; set; }

        public List<AccountPlanVisitStoreDto> AccountPlanVisitStores { get; set; }

        public List<AccountCareStoreDto> AccountCareStores { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnAccountPlanVisit, AccountPlanVisitDto>().ReverseMap();
        }
    }

    public class AccountPlanVisitCreateDto : IMapFrom, IDto
    {
        public string UserName { get; set; }

        public string PlanName { get; set; }

        [Range(1970, int.MaxValue, ErrorMessage = "Year must be greater than or equal to 1970")]
        public int Year { get; set; }

        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12")]
        public int Month { get; set; }

        public int[] PartnerIds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AccountPlanVisitDto, AccountPlanVisitCreateDto>().ReverseMap();
        }
    }

    public class AccountPlanVisitUpdateDto : IMapFrom, IDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string PlanName { get; set; }

        [Range(1970, int.MaxValue, ErrorMessage = "Year must be greater than or equal to 1970")]
        public int Year { get; set; }

        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12")]
        public int Month { get; set; }

        public int[] PartnerIds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AccountPlanVisitDto, AccountPlanVisitUpdateDto>().ReverseMap();
        }
    }

    public class AccountPlanVisitLittleDto : BaseDto, IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public string PlanName { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnAccountPlanVisit, AccountPlanVisitLittleDto>().ReverseMap();
        }
    }

    public class AccountPlanVisitDeleteDto : IMapFrom, IDto
    {
        public int[] ListId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnAccountPlanVisit, AccountPlanVisitDeleteDto>().ReverseMap();
        }
    }
}
