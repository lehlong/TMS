using AutoMapper;
using Common;
using XHTD.CORE.Entities;

namespace XHTD.BUSINESS.Dtos.XHTD
{
    public class StoreOrderOperatingDto : IMapFrom, IDto
    {
        public int Id { get; set; }

        public string? Vehicle { get; set; }

        public string? DriverName { get; set; }

        public string? NameDistributor { get; set; }

        public double? ItemId { get; set; }

        public string? NameProduct { get; set; }

        public string? CatId { get; set; }

        public decimal? SumNumber { get; set; }

        public DateTime? TimeIn33 { get; set; }

        public string? CardNo { get; set; }

        public int? OrderId { get; set; }

        public string? DeliveryCode { get; set; }

        public string? DeliveryCodeParent { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? TypeProduct { get; set; }

        public string? TypeXk { get; set; }

        public DateTime? TimeIn21 { get; set; }

        public DateTime? TimeIn22 { get; set; }

        public int? Confirm1 { get; set; }

        public DateTime? TimeConfirm1 { get; set; }

        public int? Confirm2 { get; set; }

        public DateTime? TimeConfirm2 { get; set; }

        public int? Confirm3 { get; set; }

        public DateTime? TimeConfirm3 { get; set; }

        public int? Confirm4 { get; set; }

        public DateTime? TimeConfirm4 { get; set; }

        public int? Confirm5 { get; set; }

        public DateTime? TimeConfirm5 { get; set; }

        public int? Confirm6 { get; set; }

        public DateTime? TimeConfirm6 { get; set; }

        public int? Confirm7 { get; set; }

        public DateTime? TimeConfirm7 { get; set; }

        public int? Confirm8 { get; set; }

        public DateTime? TimeConfirm8 { get; set; }

        public int? Confirm9 { get; set; }

        public DateTime? TimeConfirm9 { get; set; }

        public string? Confirm9Note { get; set; }

        public string? Confirm9Image { get; set; }

        public int? Confirm10 { get; set; }

        public DateTime? TimeConfirm10 { get; set; }

        /// <summary>
        /// 0: Chưa nhận đơn 
        /// 1: Đã nhận đơn 
        /// 2: Đã vào cổng 
        /// 3: Đã cân vào 
        /// 4: Đang gọi xe 
        /// 5: Đang lấy hàng 
        /// 6: Đã lấy hàng 
        /// 7: Đã cân ra
        /// 8: Đã hoàn thành 
        /// 9: Đã giao hàng
        /// 
        /// </summary>
        public int? Step { get; set; }

        public int? IndexOrder { get; set; }

        public int? IndexOrder1 { get; set; }

        public int? IndexOrder2 { get; set; }

        public int? Trough { get; set; }

        public int? Trough1 { get; set; }

        public int? NumberVoice { get; set; }

        public string? State { get; set; }

        public int? Prioritize { get; set; }

        public DateTime? DayCreate { get; set; }

        public int? IddistributorSyn { get; set; }

        public int? AreaId { get; set; }

        public string? AreaName { get; set; }

        public string? CodeStore { get; set; }

        public string? NameStore { get; set; }

        public string? DriverUserName { get; set; }

        public DateTime? DriverAccept { get; set; }

        public int? IndexOrderTemp { get; set; }

        public int? WeightIn { get; set; }

        public DateTime? WeightInTime { get; set; }

        public int? WeightOut { get; set; }

        public DateTime? WeightOutTime { get; set; }

        public int? WeightInAuto { get; set; }

        public DateTime? WeightInTimeAuto { get; set; }

        public int? WeightOutAuto { get; set; }

        public DateTime? WeightOutTimeAuto { get; set; }

        public string? NoteFinish { get; set; }

        public string? Longitude { get; set; }

        public string? Latitude { get; set; }

        public int? CountReindex { get; set; }

        public bool? IsVoiced { get; set; }

        public string? LocationCode { get; set; }

        public int? TransportMethodId { get; set; }

        public string? TransportMethodName { get; set; }

        public bool? LockInDbet { get; set; }

        public string? LogJobAttach { get; set; }

        public bool? IsSyncedByNewWs { get; set; }

        public string? TroughLineCode { get; set; }

        public bool? IsScaleAuto { get; set; }

        public DateTime? TimeConfirmHistory { get; set; }

        public string? LogHistory { get; set; }

        public string? MoocCode { get; set; }

        public string? LogProcessOrder { get; set; }

        public int? DriverPrintNumber { get; set; }

        public DateTime? DriverPrintTime { get; set; }

        public bool? WarningNotCall { get; set; }

        public string? XiRoiAttatchmentFile { get; set; }

        public string? PackageNumber { get; set; }

        public int? Shifts { get; set; }

        public bool? AutoScaleOut { get; set; }

        public DateTime? CreateDay { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? UpdateDay { get; set; }

        public string? UpdateBy { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblStoreOrderOperating, StoreOrderOperatingDto>().ReverseMap();
        }
    }

    public class ReceiveOrderDto : IMapFrom, IDto
    {
        public int Id { get; set; }

        public string DriverUserName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblStoreOrderOperating, ReceiveOrderDto>().ReverseMap();
        }
    }

    public class OrderUpdateStepDto : IMapFrom, IDto
    {
        public int Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TblStoreOrderOperating, OrderUpdateStepDto>().ReverseMap();
        }
    }
}
