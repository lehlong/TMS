using AutoMapper;
using Common;
using DMS.BUSINESS.Common;
using DMS.BUSINESS.Common.Enum;
using DMS.BUSINESS.Dtos.SO;
using DMS.BUSINESS.Filter.SO;
using DMS.BUSINESS.Services.CM.Comment;
using DMS.CORE;
using DMS.CORE.Entities.SO;
using Microsoft.EntityFrameworkCore;

namespace DMS.BUSINESS.Services.SO
{
    public interface IOrderService : IGenericService<TblSoOrder, OrderDto>
    {
        Task<PagedResponseDto> Search(OrderFilter filter);
        Task UpdateState(IDto dto);
        Task<IList<OrderDto>> GetAll(OrderGetAllFilter filter);
    }

    public class OrderService(AppDbContext dbContext, IMapper mapper) : GenericService<TblSoOrder, OrderDto>(dbContext, mapper), IOrderService
    {
        public override async Task<OrderDto> Add(IDto dto)
        {
            try
            {
                if (dto is not OrderCreateDto model)
                {
                    this.Status = false;
                    this.MessageObject.Code = "0000";
                    return null;
                }

                if (model.PartnerIdSell != null && !await _dbContext.TblMnPartnerReference.AnyAsync(x => x.PartnerIdBuy == model.PartnerIdBuy && x.PartnerIdSell == model.PartnerIdSell))
                {
                    this.Status = false;
                    this.MessageObject.Code = "0000";
                    return null;
                }

                var partnerBuy = await _dbContext.TblMdPartner.FirstOrDefaultAsync(x => x.Id == model.PartnerIdBuy);

                if (partnerBuy == null)
                {
                    this.Status = false;
                    this.MessageObject.Code = "0000";
                    return null;
                }
                if (model.PartnerIdSell != null)
                {
                    var partnerSell = await _dbContext.TblMdPartner.FirstOrDefaultAsync(x => x.Id == model.PartnerIdSell);

                    if (partnerSell == null)
                    {
                        if (partnerBuy == null)
                        {
                            this.Status = false;
                            this.MessageObject.Code = "0000";
                            return null;
                        }
                    }

                    if (partnerBuy.SaleTypeCode == "CH-C3" && partnerSell.SaleTypeCode == "CH-C2")
                    {
                        model.Type = "C3";
                    }
                    else if (partnerBuy.SaleTypeCode == "CH-C2" && partnerSell.SaleTypeCode == "NPP")
                    {
                        model.Type = "C2";
                    }
                }

                if (partnerBuy.SaleTypeCode == "NPP" && model.PartnerIdSell == null)
                {
                    model.Type = "NPP";
                }

                int codeType = 0;
                switch (model.Type)
                {
                    case "NPP": codeType = 1; break;
                    case "C2": codeType = 2; break;
                    case "C3": codeType = 3; break;
                }

                if (codeType == 0)
                {
                    this.Status = false;
                    this.MessageObject.Code = "0000";
                    return null;
                }

                var sequenceVal = await _dbContext.GetNextSequenceValue("ORDER_SEQUENCE");
                model.Code = $"{codeType}-{DateTime.Now:ddMMyy}-{sequenceVal}";

                var order = _mapper.Map<TblSoOrder>(model);

                order.Processes = [new TblSoOrderProcess() {
                ProcessDate = DateTime.Now,
                ActionCode = OrderAction.TAO_MOI.ToString(),
                State = OrderState.KHOI_TAO.ToString()}];

                var result = await _dbContext.AddAsync(order);
                await _dbContext.SaveChangesAsync();

                if (this.Status && model.Childs != null && model.Childs.Count != 0)
                {
                    var childs = await _dbContext.TblSoOrder.Where(x => model.Childs.Select(x => x.Code).Contains(x.Code)).ToListAsync();
                    childs.ForEach(x =>
                    {
                        x.ParentCode = model.Code;
                    });

                    _dbContext.UpdateRange(childs);
                    await _dbContext.SaveChangesAsync();

                    if (!string.IsNullOrEmpty(model.Notes))
                    {
                        await new CommentService(_dbContext, _mapper).Add(new Dtos.BU.CommentCreateDto()
                        {
                            ReferenceId = result.Entity.ReferenceId ?? Guid.NewGuid(),
                            Type = "text",
                            Content = model.Notes,
                        });
                    }
                }

                return _mapper.Map<OrderDto>(result.Entity);
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return null;
            }
        }

        public override async Task<OrderDto> GetById(object id)
        {
            var data = await _dbContext.TblSoOrder
                .Include(x => x.PartnerBuy)
                .Include(x => x.PartnerSell)
                .Include(x => x.Area)
                .Include(x => x.Vehicle)
                .Include(x => x.Details).ThenInclude(x => x.Item)
                .Include(x => x.Details).ThenInclude(x => x.Unit)
                .Include(x => x.Driver)
                .Include(x => x.Processes)
                .Include(x => x.Childs).ThenInclude(x => x.Details)
                .Include(x => x.Childs).ThenInclude(x => x.Childs).ThenInclude(x => x.Details)
                .Include(x => x.Childs).ThenInclude(x => x.Childs).ThenInclude(x => x.Childs).ThenInclude(x => x.Details)
                .Include(x => x.Parent).ThenInclude(x => x.Details)
                .Include(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.Details)
                .Include(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.Details)
                .Include(x => x.Childs).ThenInclude(x => x.PartnerBuy)
                .Include(x => x.Childs).ThenInclude(x => x.Childs).ThenInclude(x => x.PartnerBuy)
                .Include(x => x.Childs).ThenInclude(x => x.Childs).ThenInclude(x => x.Childs).ThenInclude(x => x.PartnerBuy)
                .Include(x => x.Parent).ThenInclude(x => x.PartnerBuy)
                .Include(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.PartnerBuy)
                .Include(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.Parent).ThenInclude(x => x.PartnerBuy)
                .FirstOrDefaultAsync(x => x.Code == id.ToString());

            return _mapper.Map<OrderDto>(data);
        }

        public async Task<IList<OrderDto>> GetAll(OrderGetAllFilter filter)
        {
            var query = _dbContext.TblSoOrder
                .Include(x => x.PartnerBuy)
                .Include(x => x.PartnerSell)
                .Include(x => x.Details).ThenInclude(x => x.Item)
                .Include(x => x.Details).ThenInclude(x => x.Unit)
                .Include(x => x.Driver)
                .Include(x => x.Area)
                .Include(x => x.Vehicle)
                .Where(x => filter.PartnerIdBuy == null || x.PartnerIdBuy == filter.PartnerIdBuy)
                .Where(x => filter.PartnerIdSell == null || x.PartnerIdSell == filter.PartnerIdSell)
                .Where(x => filter.AreaId == null || x.AreaId == filter.AreaId)
                .Where(x => string.IsNullOrEmpty(filter.Type) || x.Type == filter.Type)
                .Where(x => filter.States == null || filter.States.Count == 0 || filter.States.Contains(x.State))
                .Where(x => string.IsNullOrEmpty(filter.ParentCode) || x.ParentCode == filter.ParentCode)
                .Where(x => filter.HasParent == null || (filter.HasParent == true ? x.ParentCode != null : x.ParentCode == null))
                .Where(x => string.IsNullOrEmpty(filter.ItemCode) || x.Details.Any(y => y.ItemCode == filter.ItemCode));

            return _mapper.Map<IList<OrderDto>>(await query.ToListAsync());
        }

        public async Task<PagedResponseDto> Search(OrderFilter filter)
        {
            var query = _dbContext.TblSoOrder
                .Include(x => x.PartnerBuy)
                .Include(x => x.PartnerSell)
                .Include(x => x.Details).ThenInclude(x => x.Item)
                .Include(x => x.Details).ThenInclude(x => x.Unit)
                .Include(x => x.Driver)
                .Include(x => x.Area)
                .Include(x => x.Vehicle)
                .Include(x => x.Childs).ThenInclude(x => x.Details)
                .Include(x => x.Childs).ThenInclude(x => x.Childs).ThenInclude(x => x.Details)
                .Include(x => x.Childs).ThenInclude(x => x.Childs).ThenInclude(x => x.Childs).ThenInclude(x => x.Details)
                .Where(x => filter.PartnerIdSell == null || x.PartnerIdSell == filter.PartnerIdSell)
                .Where(x => string.IsNullOrEmpty(filter.VehicleCode) || x.VehicleCode == filter.VehicleCode)
                .Where(x => filter.AreaId == null || x.AreaId == filter.AreaId)
                .Where(x => string.IsNullOrEmpty(filter.ItemCode) || x.Details.Any(y => y.ItemCode == filter.ItemCode))
                .Where(x => filter.States == null || filter.States.Count == 0 || filter.States.Contains(x.State))
                .Where(x => filter.FromDate == null || (x.OrderDate.HasValue && x.OrderDate.Value.Date >= filter.FromDate))
                .Where(x => string.IsNullOrEmpty(filter.Type) || x.Type == filter.Type)
                .Where(x => string.IsNullOrEmpty(filter.ParentCode) || x.ParentCode == filter.ParentCode)
                .Where(x => filter.HasParent == null || (filter.HasParent == true ? x.ParentCode != null : x.ParentCode == null))
                .Where(x => filter.ToDate == null || (x.OrderDate.HasValue && x.OrderDate.Value.Date <= filter.ToDate));

            return await base.Paging(query, filter);
        }

        public override async Task Update(IDto dto)
        {
            if (dto is not OrderUpdateDto model)
            {
                this.Status = false;
                this.MessageObject.Code = "0000";
                return;
            }
            try
            {
                var currentObj = await _dbContext.TblSoOrder.Include(x => x.Details).FirstOrDefaultAsync(x => x.Code == model.Code);
                await base.UpdateWithListInside(dto, currentObj);

                var childs = await _dbContext.TblSoOrder.Where(x => x.ParentCode == model.Code).ToListAsync();
                childs.ForEach(x =>
                {
                    x.ParentCode = null;
                });
                _dbContext.TblSoOrder.UpdateRange(childs);
                await _dbContext.SaveChangesAsync();

                if (this.Status && model.Childs != null && model.Childs.Count != 0)
                {
                    var addChilds = await _dbContext.TblSoOrder.Where(x => model.Childs.Select(x => x.Code).Contains(x.Code)).ToListAsync();
                    addChilds.ForEach(x =>
                    {
                        x.ParentCode = model.Code;
                    });

                    _dbContext.UpdateRange(addChilds);
                    await _dbContext.SaveChangesAsync();

                    await _dbContext.TblSoOrderProcess.AddAsync(new TblSoOrderProcess()
                    {
                        ActionCode = OrderAction.THAY_DOI_THONG_TIN.ToString(),
                        OrderCode = model.Code,
                        State = currentObj.State,
                        ProcessDate = DateTime.Now,
                    });

                    await _dbContext.SaveChangesAsync();

                    if (!string.IsNullOrEmpty(model.Notes))
                    {
                        await new CommentService(_dbContext, _mapper).Add(new Dtos.BU.CommentCreateDto()
                        {
                            ReferenceId = currentObj.ReferenceId ?? Guid.NewGuid(),
                            Type = "text",
                            Content = model.Notes,
                        });
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                this.Status = false;
                this.Exception = ex;
                return;
            }
        }

        public override async Task Delete(object code)
        {
            var currentObj = _dbContext.TblSoOrder.Find(code);
            if (currentObj == null)
            {
                Status = false;
                MessageObject.Code = "0000";
                return;
            }
            _dbContext.Entry(currentObj).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateState(IDto dto)
        {
            if (dto is not OrderUpdateStateDto model)
            {
                this.Status = false;
                this.MessageObject.Code = "0000";
                return;
            }

            if (!Enum.TryParse(model.State, out OrderState state))
            {
                this.Status = false;
                this.MessageObject.Code = "0000";
                return;
            }
            await base.Update(model);
            if (this.Status)
            {
                OrderAction? action = null;
                switch (state)
                {
                    case OrderState.CHO_XAC_NHAN:
                        action = OrderAction.GUI_DON_HANG;
                        break;
                    case OrderState.DA_XAC_NHAN:
                        action = OrderAction.XAC_NHAN;
                        break;
                    case OrderState.TU_CHOI:
                        action = OrderAction.TU_CHOI;
                        break;
                    case OrderState.HUY:
                        action = OrderAction.HUY;
                        break;
                    case OrderState.DA_IN_PHIEU:
                        action = OrderAction.IN_PHIEU;
                        break;
                    case OrderState.DA_XUAT_HANG:
                        action = OrderAction.XUAT_HANG;
                        break;
                }
                await _dbContext.TblSoOrderProcess.AddAsync(new TblSoOrderProcess()
                {
                    ActionCode = action.ToString(),
                    OrderCode = model.Code,
                    State = model.State,
                    ProcessDate = DateTime.Now,
                });
                await _dbContext.SaveChangesAsync();

                if (state == OrderState.HUY || state == OrderState.TU_CHOI)
                {
                    if (!string.IsNullOrEmpty(model.Comment))
                    {
                        var currentObj = await _dbContext.TblSoOrder.FirstOrDefaultAsync(x => x.Code == model.Code);
                        await new CommentService(_dbContext, _mapper).Add(new Dtos.BU.CommentCreateDto()
                        {
                            ReferenceId = currentObj.ReferenceId ?? Guid.NewGuid(),
                            Type = "text",
                            Content = model.Comment,
                        });
                    }
                }
            }
        }
    }
}
