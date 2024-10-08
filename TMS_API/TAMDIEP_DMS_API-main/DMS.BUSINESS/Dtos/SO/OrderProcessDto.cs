using AutoMapper;
using Common;
using DMS.CORE.Entities.SO;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DMS.BUSINESS.Dtos.SO
{
    public class OrderProcessDto : BaseDto, IMapFrom, IDto
    {
        [Key]
        public Guid Id { get; set; }

        public string OrderCode { get; set; }

        public DateTime ProcessDate { get; set; }

        public string ActionCode { get; set; }

        public string PrevState { get; set; }

        public string State { get; set; }

        [JsonIgnore]
        public virtual OrderDto Order { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblSoOrderProcess, OrderProcessDto>().ReverseMap();
        }
    }
}
