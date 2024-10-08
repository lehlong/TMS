using Common;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Extensions;
using DMS.BUSINESS.Common.Enum;
using DMS.BUSINESS.Dtos.SO;
using DMS.BUSINESS.Filter.SO;
using DMS.BUSINESS.Services.SO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers.MD
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController(IOrderService service) : ControllerBase
    {
        public readonly IOrderService _service = service;

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] OrderFilter filter)
        {
            var transferObject = new TransferObject();
            var result = await _service.Search(filter);
            if (_service.Status)
            {
                transferObject.Data = result;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] OrderGetAllFilter filter)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetAll(filter);
            if (_service.Status)
            {
                transferObject.Data = result;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] OrderCreateDto Order)
        {
            var transferObject = new TransferObject();
            var result = await _service.Add(Order);
            if (_service.Status)
            {
                transferObject.Data = result;
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0100", _service);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0101", _service);
            }
            return Ok(transferObject);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] OrderUpdateDto Order)
        {
            var transferObject = new TransferObject();
            await _service.Update(Order);
            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0103", _service);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0104", _service);
            }
            return Ok(transferObject);
        }

        [HttpDelete("Delete/{code}")]
        public async Task<IActionResult> Delete([FromRoute] string code)
        {
            var transferObject = new TransferObject();
            await _service.Delete(code);
            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0105", _service);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0106", _service);
            }
            return Ok(transferObject);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] string code)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetById(code);
            if (_service.Status)
            {
                transferObject.Data = result;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }

        [HttpPut("Send")]
        public async Task<IActionResult> Send([FromBody] OrderUpdateStateDto model)
        {
            var transferObject = new TransferObject();
            await _service.UpdateState(new OrderUpdateStateDto()
            {
                Code = model.Code,
                State = OrderState.CHO_XAC_NHAN.ToString()
            });
            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0103", _service);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0104", _service);
            }
            return Ok(transferObject);
        }

        [HttpPut("Reject")]
        public async Task<IActionResult> Reject([FromBody] OrderUpdateStateDto model)
        {
            var transferObject = new TransferObject();
            await _service.UpdateState(new OrderUpdateStateDto()
            {
                Code = model.Code,
                State = OrderState.TU_CHOI.ToString(),
                Comment = model.Comment
            });
            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0103", _service);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0104", _service);
            }
            return Ok(transferObject);
        }

        [HttpPut("Cancel")]
        public async Task<IActionResult> Cancel([FromBody] OrderUpdateStateDto model)
        {
            var transferObject = new TransferObject();
            await _service.UpdateState(new OrderUpdateStateDto()
            {
                Code = model.Code,
                State = OrderState.HUY.ToString(),
                Comment = model.Comment
            });
            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0103", _service);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0104", _service);
            }
            return Ok(transferObject);
        }

        [HttpPut("Confirm")]
        public async Task<IActionResult> Confirm([FromBody] OrderUpdateStateDto model)
        {
            var transferObject = new TransferObject();
            await _service.UpdateState(new OrderUpdateStateDto()
            {
                Code = model.Code,
                State = OrderState.DA_XAC_NHAN.ToString()
            });
            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0103", _service);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0104", _service);
            }
            return Ok(transferObject);
        }

        [HttpPut("Release")]
        public async Task<IActionResult> Release([FromBody] OrderUpdateStateDto model)
        {
            var transferObject = new TransferObject();
            await _service.UpdateState(new OrderUpdateStateDto()
            {
                Code = model.Code,
                State = OrderState.DA_XUAT_HANG.ToString()
            });
            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0103", _service);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0104", _service);
            }
            return Ok(transferObject);
        }
    }
}
