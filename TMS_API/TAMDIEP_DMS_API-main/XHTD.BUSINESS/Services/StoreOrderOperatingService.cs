using AutoMapper;
using Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using XHTD.BUSINESS.Common;
using XHTD.BUSINESS.Dtos.Hub;
using XHTD.BUSINESS.Dtos.XHTD;
using XHTD.BUSINESS.Filter.XHTD;
using XHTD.BUSINESS.Hub;
using XHTD.CORE.Entities;

namespace XHTD.BUSINESS.Services
{
    public interface IStoreOrderOperatingService : IXHTDGenericService<TblStoreOrderOperating, StoreOrderOperatingDto>
    {
        Task<PagedResponseDto> Search(StoreOrderOperatingFilter filter);
        Task ReceiveOrder(ReceiveOrderDto model);
        Task CancelOrder(OrderUpdateStepDto model);
        Task ConfirmOrder(OrderUpdateStepDto model);
        Task CancelConfirmOrder(OrderUpdateStepDto model);
        Task CheckIn(OrderUpdateStepDto model);
        Task WeighIn(OrderUpdateStepDto model);
        Task WeighOut(OrderUpdateStepDto model);
        Task CheckOut(OrderUpdateStepDto model);
        Task CompleteOrder(OrderUpdateStepDto model);
        Task<SendGatewayNotificationDto> SendGatewayNotification(SendGatewayNotificationDto model);
    }
    public class StoreOrderOperatingService(XHTDDbContext dbContext, IMapper mapper, IHubContext<GatewayHub> hubContext) : XHTDGenericService<TblStoreOrderOperating, StoreOrderOperatingDto>(dbContext, mapper), IStoreOrderOperatingService
    {
        private readonly IHubContext<GatewayHub> _hubContext = hubContext;

        public async Task<PagedResponseDto> Search(StoreOrderOperatingFilter filter)
        {
            var query = _dbContext.TblStoreOrderOperatings
                .Where(x => filter.Steps == null || filter.Steps.Count == 0 || filter.Steps.Contains(x.Step ?? 0))
                .Where(x => filter.OrderDate == null || (x.OrderDate.HasValue && x.OrderDate.Value.Date == filter.OrderDate.Value.Date))
                .Where(x => filter.FromDate == null || (x.OrderDate.HasValue && x.OrderDate.Value.Date >= filter.FromDate.Value.Date))
                .Where(x => filter.ToDate == null || (x.OrderDate.HasValue && x.OrderDate.Value.Date <= filter.ToDate.Value.Date))
                .Where(x => filter.IsVoiced == null || (x.IsVoiced.HasValue && x.IsVoiced.Value == filter.IsVoiced.Value))
                .Where(x => filter.WeightInTime == null || x.WeightInTime.HasValue && x.WeightInTime.Value.Date == filter.WeightInTime.Value.Date)
                .Where(x => filter.DeliveryCodes == null || filter.DeliveryCodes.Count == 0 || filter.DeliveryCodes.Contains(x.DeliveryCode))
                .Where(x => string.IsNullOrEmpty(filter.TypeProduct) || x.TypeProduct == filter.TypeProduct)
                .Where(x => string.IsNullOrEmpty(filter.KeyWord)
                || x.DeliveryCode.ToLower().Contains(filter.KeyWord.ToLower())
                || x.Vehicle.ToLower().Contains(filter.KeyWord.ToLower())
                || x.DriverUserName.ToLower().Contains(filter.KeyWord.ToLower())
                || x.DriverName.ToLower().Contains(filter.KeyWord.ToLower()));

            if (!string.IsNullOrEmpty(filter.DriverUserName))
            {
                var accessVehicle = await _dbContext.TblDriverVehicles.Where(x => x.UserName == filter.DriverUserName).Select(x => x.Vehicle).ToListAsync();
                query = query.Where(x => accessVehicle.Contains(x.Vehicle));
            }

            if (!string.IsNullOrEmpty(filter.Rfid))
            {
                var rfid = await _dbContext.TblRfids.FirstOrDefaultAsync(x => x.Code == filter.Rfid);

                if (rfid != null)
                {
                    query = query.Where(x => x.Vehicle == rfid.Vehicle);
                }
            }

            if (filter.Fields != null && filter.Fields.Count != 0)
            {
                query = query.SelectFields(filter.Fields);
            }

            return await base.Paging(query, filter);
        }

        public async Task ReceiveOrder(ReceiveOrderDto model)
        {
            try
            {
                var order = await _dbContext.TblStoreOrderOperatings.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (order == null || order.Step != (int)OrderState.CHUA_NHAN_DON || !string.IsNullOrEmpty(order.DriverUserName))
                {
                    this.Status = false;
                    this.MessageObject.Code = "2001";
                    return;
                }

                order.Step = (int)OrderState.DA_NHAN_DON;
                order.DriverUserName = model.DriverUserName;
                order.TimeConfirm1 = DateTime.Now;
                order.LogProcessOrder += $"#Nhận đơn thủ công lúc {DateTime.Now:dd/MM/yyyy HH:mm:ss} ";

                _dbContext.Update(order);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return;
            }
        }

        public async Task CancelOrder(OrderUpdateStepDto model)
        {
            try
            {
                var order = await _dbContext.TblStoreOrderOperatings.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (order == null || order.Step != (int)OrderState.DA_NHAN_DON || string.IsNullOrEmpty(order.DriverUserName))
                {
                    this.Status = false;
                    this.MessageObject.Code = "2001";
                    return;
                }

                order.Step = (int)OrderState.CHUA_NHAN_DON;
                order.DriverUserName = null;

                _dbContext.Update(order);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return;
            }
        }

        public async Task ConfirmOrder(OrderUpdateStepDto model)
        {
            try
            {
                var order = await _dbContext.TblStoreOrderOperatings.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (order == null || order.Step != (int)OrderState.DA_NHAN_DON)
                {
                    this.Status = false;
                    this.MessageObject.Code = "2001";
                    return;
                }

                var maxIndex = await _dbContext.TblStoreOrderOperatings.Where(x => x.TypeProduct == order.TypeProduct).MaxAsync(x => x.IndexOrder) ?? 0;

                order.Step = (int)OrderState.XAC_THUC;
                order.IndexOrder = maxIndex++;
                order.TimeConfirm10 = DateTime.Now;
                order.LogProcessOrder += $"#Xác thực thủ công lúc {DateTime.Now:dd/MM/yyyy HH:mm:ss} ";

                _dbContext.Update(order);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return;
            }
        }

        public async Task CancelConfirmOrder(OrderUpdateStepDto model)
        {
            try
            {
                var order = await _dbContext.TblStoreOrderOperatings.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (order == null || order.Step != (int)OrderState.XAC_THUC)
                {
                    this.Status = false;
                    this.MessageObject.Code = "2001";
                    return;
                }

                order.Step = (int)OrderState.DA_NHAN_DON;
                order.IndexOrder = null;

                _dbContext.Update(order);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return;
            }
        }

        public async Task CheckIn(OrderUpdateStepDto model)
        {
            try
            {
                var order = await _dbContext.TblStoreOrderOperatings.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (order == null || order.Step != (int)OrderState.XAC_THUC)
                {
                    this.Status = false;
                    this.MessageObject.Code = "2001";
                    return;
                }

                order.Step = (int)OrderState.DA_VAO_CONG;
                order.TimeConfirm2 = DateTime.Now;
                order.LogProcessOrder += $"#Vào cổng thủ công lúc {DateTime.Now:dd/MM/yyyy HH:mm:ss} ";

                _dbContext.Update(order);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return;
            }
        }

        public async Task WeighIn(OrderUpdateStepDto model)
        {
            try
            {
                var order = await _dbContext.TblStoreOrderOperatings.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (order == null || (order.Step != (int)OrderState.XAC_THUC && order.Step != (int)OrderState.DA_VAO_CONG))
                {
                    this.Status = false;
                    this.MessageObject.Code = "2001";
                    return;
                }

                order.Step = (int)OrderState.DA_CAN_VAO;
                order.TimeConfirm3 = DateTime.Now;
                order.LogProcessOrder += $"#Cân vào thủ công lúc {DateTime.Now:dd/MM/yyyy HH:mm:ss} ";

                _dbContext.Update(order);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return;
            }
        }

        public async Task WeighOut(OrderUpdateStepDto model)
        {
            try
            {
                var order = await _dbContext.TblStoreOrderOperatings.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (order == null || (order.Step != (int)OrderState.DA_CAN_VAO
                                   && order.Step != (int)OrderState.DANG_GOI_XE
                                   && order.Step != (int)OrderState.DANG_LAY_HANG
                                   && order.Step != (int)OrderState.DA_LAY_HANG))
                {
                    this.Status = false;
                    this.MessageObject.Code = "2001";
                    return;
                }

                order.Step = (int)OrderState.DA_CAN_RA;
                order.TimeConfirm7 = DateTime.Now;
                order.LogProcessOrder += $"#Cân ra thủ công lúc {DateTime.Now:dd/MM/yyyy HH:mm:ss} ";

                _dbContext.Update(order);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return;
            }
        }

        public async Task CheckOut(OrderUpdateStepDto model)
        {
            try
            {
                var order = await _dbContext.TblStoreOrderOperatings.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (order == null || order.Step != (int)OrderState.DA_CAN_RA)
                {
                    this.Status = false;
                    this.MessageObject.Code = "2001";
                    return;
                }

                order.Step = (int)OrderState.DA_HOAN_THANH;
                order.TimeConfirm8 = DateTime.Now;
                order.LogProcessOrder += $"#Ra cổng thủ công lúc {DateTime.Now:dd/MM/yyyy HH:mm:ss} ";

                _dbContext.Update(order);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return;
            }
        }

        public async Task CompleteOrder(OrderUpdateStepDto model)
        {
            try
            {
                var order = await _dbContext.TblStoreOrderOperatings.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (order == null || (order.Step != (int)OrderState.DA_CAN_RA && order.Step != (int)OrderState.DA_HOAN_THANH))
                {
                    this.Status = false;
                    this.MessageObject.Code = "2001";
                    return;
                }

                order.Step = (int)OrderState.DA_GIAO_HANG;
                order.TimeConfirm9 = DateTime.Now;
                order.LogProcessOrder += $"Kết thúc thủ công lúc {DateTime.Now:dd/MM/yyyy HH:mm:ss} ";

                _dbContext.Update(order);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return;
            }
        }

        public async Task<SendGatewayNotificationDto> SendGatewayNotification(SendGatewayNotificationDto model)
        {
            await _hubContext.Clients.All.SendAsync(SignalRMethod.Gateway.ToString(), model);
            return model;
        }
    }
}
