using AutoMapper;
using DMS.CORE.Entities.MN;
using DMS.CORE.Entities.MD;
using System.ComponentModel.DataAnnotations;
using Common;

namespace DMS.BUSINESS.Dtos.MN
{
    public class AreaItemDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public string ItemCode { get; set; }

        public int AreaId { get; set; }

        public virtual AreaDto Area { get; set; }

        public virtual ItemDto Item { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnAreaItem, AreaItemDto>().ReverseMap();
        }
    }

    public class AreaItemWithAreaDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public int AreaId { get; set; }

        public virtual AreaDto Area { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnAreaItem, AreaItemWithAreaDto>().ReverseMap();
        }
    }

    public class AreaItemWithItemDto : BaseMdDto, IMapFrom, IDto
    {
        [Key]
        public int Id { get; set; }

        public string ItemCode { get; set; }

        public virtual ItemDto Item { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnAreaItem, AreaItemWithItemDto>().ReverseMap();
        }
    }

    public class AreaItemCreateWithItemDto : BaseMdDto, IMapFrom, IDto
    {
        public string ItemCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblMnAreaItem, AreaItemCreateWithItemDto>().ReverseMap();
        }
    }
}
