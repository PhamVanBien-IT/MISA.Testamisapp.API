using Amazon.Auth.AccessControlPolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Testamis.BL;
using MISA.Testamis.Common.Entitis;
using MISA.Testamis.Common.Enums.DTO;

namespace MISA.Testamis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeMissionallowancesController : BaseController<EmployeeMissionallowance>
    {
        public EmployeeMissionallowancesController(IBaseBL<EmployeeMissionallowance> baseBL) : base(baseBL)
        {
        }
    }
}
