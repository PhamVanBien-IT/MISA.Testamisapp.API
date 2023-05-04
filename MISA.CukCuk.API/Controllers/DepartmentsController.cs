using Microsoft.AspNetCore.Mvc;
using MISA.Testamis.BL;
using MISA.Testamis.Common.Entitis;

namespace MISA.Testamis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseController<Department>
    {
        public DepartmentsController(IBaseBL<Department> baseBL) : base(baseBL)
        {
        }

        /// <summary>
        /// Hàm ngăn không cho thực thi thêm department
        /// </summary>
        /// <param name="department">Đơn vị muốn thêm</param>
        /// <returns>Lỗi 405: Không được phép thực hiện</returns>
        /// CreatedBy: Bien (9/2/2023)
        public override IActionResult Insert([FromBody] Department department)
        {
            return StatusCode(StatusCodes.Status405MethodNotAllowed);
        }

        /// <summary>
        /// Hàm ngăn không cho thực thi sửa department
        /// </summary>
        /// <param name="entityId">ID dơn vị muốn sửa</param>
        /// <param name="department">Đơn vị chứa dữ liệu mới</param>
        /// <returns>Lỗi 405: Không được phép thực hiện</returns>
        /// CreatedBy: Bien (9/2/2023)
        public override IActionResult Update([FromRoute] Guid entityId, [FromBody] Department department)
        {
            return StatusCode(StatusCodes.Status405MethodNotAllowed);
        }

        /// <summary>
        /// Hàm ngăn không cho thực thi xóa department
        /// </summary>
        /// <param name="entityId">ID dơn vị muốn sửa</param>
        /// <returns>Lỗi 405: Không được phép thực hiện</returns>
        /// CreatedBy: Bien (9/2/2023)
        public override IActionResult Delete([FromRoute] Guid entityId)
        {
            return StatusCode(StatusCodes.Status405MethodNotAllowed);
        }
    }
}
