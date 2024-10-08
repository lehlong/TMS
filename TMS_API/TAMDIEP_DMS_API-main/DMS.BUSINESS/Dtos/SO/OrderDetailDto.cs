using AutoMapper;
using Common;
using DMS.CORE.Entities.MD;
using DMS.CORE.Entities.SO;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DMS.BUSINESS.Dtos.SO
{
    public class OrderDetailDto : BaseDto, IMapFrom, IDto
    {
        [Key]
        public Guid Id { get; set; }

        public string OrderCode { get; set; }

        public string ItemCode { get; set; }

        public double OrderNumber { get; set; }

        public string UnitCode { get; set; }

        public Guid? ReferenceId { get; set; }

        public virtual ItemDto Item { get; set; }

        public virtual UnitDto Unit { get; set; }

        [JsonIgnore]
        public virtual OrderDto Order { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblSoOrderDetail, OrderDetailDto>().ReverseMap();
        }
    }

    public class OrderDetailCreateDto : IMapFrom, IDto
    {
        public string ItemCode { get; set; }

        public double OrderNumber { get; set; }

        public string UnitCode { get; set; }

        public Guid? ReferenceId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblSoOrderDetail, OrderDetailCreateDto>().ReverseMap();
        }
    }
}
