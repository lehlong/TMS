using AutoMapper;
using Common;
using XHTD.CORE.Entities;

namespace XHTD.BUSINESS.Common
{
    public class XHTDBaseService(XHTDDbContext dbContext, IMapper mapper) : IBaseService
    {
        public XHTDDbContext _dbContext = dbContext;
        public MessageObject MessageObject { get; set; } = new MessageObject();
        public Exception Exception { get; set; }
        public bool Status { get; set; } = true;
        public IMapper _mapper = mapper;
    }
}
