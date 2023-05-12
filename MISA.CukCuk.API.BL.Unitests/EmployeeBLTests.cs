using MISA.Testamis.API.Enums.DTO;
using MISA.Testamis.BL;
using MISA.Testamis.Common;
using MISA.Testamis.Common.Entitis;
using MISA.Testamis.Common.Enums.DTO;
using MISA.Testamis.DL;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Testamis.API.BL.Unitests
{
    internal class EmployeeBLTests
    {
        #region Field

        private IEmployeeBL _employeeBL;
        private IEmployeeDL _fakeEmployeeDL;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fakeEmployeeDL = Substitute.For<IEmployeeDL>();
            _employeeBL = new EmployeeBL(_fakeEmployeeDL);
        }
  
        /// <summary>
        /// Hàm kiểm tra khi nhập sai id nhân viên
        /// </summary>
        /// CreatedBy: Bien (10/05/2023)
        [Test]
        public void GetById_ErrorEmployeeIdNotExist_ReturnsInvalid()
        {
            // Arrange
            var employeeId = Guid.Parse("74d2efcc-aaee-11ed-a58f-d8d09038d028");

            var employee = new List<Employee>();
            employee = null;

            _fakeEmployeeDL.GetById(employeeId).Returns(employee);

            var expectedResult = new ServiceResult
            {
                IsSuccess = false,
                ErrorCode = Common.Enums.ErrorCode.GetMethodByDLError,
            };
            // Act
            var actualResult = _employeeBL.GetById(employeeId);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        /// <summary>
        /// Hàm kiểm tra khi thực hiện thêm nhân viên thành công
        /// </summary>
        /// CreatedBy: Bien (09/05/2023)
        [Test]
        public void Insert_Employee_ReturnsValid()
        {
            // Arange
            var employee = new Employee
            {
                EmployeeCode = "NV-89789",
                FullName = "Bien",
                DateOfBirth = DateTime.Parse("02/05/2022"),
                Gender = 0,
                DepartmentId = Guid.Parse("4e272fc4-7875-78d6-7d32-6a1673ffca7c"),
                LandlineNumber = "0987419510",
                PhoneNumber = "0987419510",
                Email = "nhaem2@gmail.com",
                Address = "Hà Nội",
                IdentityNumber = "033201005075",
                IdentityDate = DateTime.Parse("05/07/2016"),
                IdentityPlace = "Hà Nội",
                PositionName = "Nhân viên",
                BankAccount = "66668888",
                BankName = "Vietcombank",
                BankBranch = "Hà Nội",
                CreatedDate = DateTime.Now,
                CreatedBy = "Bien",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "Minh"
            };

            _fakeEmployeeDL.Insert(employee).Returns(1);

            var expectedResult = new ServiceResult
            {
                IsSuccess= true,
            };

            // Act
            var actualResult = _employeeBL.Insert(employee);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
        }

        /// <summary>
        /// Hàm kiểm tra thêm nhân viên khi mã nhân viên truyền vào là trống
        /// </summary>
        /// CreatedBy: Bien (09/05/2023)
        [Test]
        public void Insert_EmptyCode_ReturnsInvalid()
        {
            // Arange
            var employee = new Employee
            {
                EmployeeCode = string.Empty,
                FullName = "Bien",
                DateOfBirth = DateTime.Parse("02/05/2022"),
                Gender = 0,
                DepartmentId = Guid.Parse("142cb08f-7c31-21fa-8e90-67245e8b283e"),
                LandlineNumber = "0987419510",
                PhoneNumber = "0987419510",
                Email = "nhaem2@gmail.com",
                Address = "Hà Nội",
                IdentityNumber = "033201005075",
                IdentityDate = DateTime.Parse("05/07/2016"),
                IdentityPlace = "Hà Nội",
                PositionName = "Nhân viên",
                BankAccount = "66668888",
                BankName = "Vietcombank",
                BankBranch = "Hà Nội",
                CreatedDate = DateTime.Now,
                CreatedBy = "Bien",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "Minh"
            };

            _fakeEmployeeDL.Insert(employee).Returns(0);

            var expectedResult = new ServiceResult
            {
                IsSuccess = false,
                ErrorCode = Common.Enums.ErrorCode.ValidateError,
                Message = Common.Resource.ErrorMsg_Validate,
                Data = new Dictionary<string, string>(){{"EmployeeCode", "Mã không được để trống"}}
            };

            // Act
            var actualResult = _employeeBL.Insert(employee);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
            Assert.AreEqual(expectedResult.Data, actualResult.Data);
        }

        /// <summary>
        /// Hàm kiểm tra thực hiện hàm thêm nhân viên khi mã nhân viên truyền vào là trống
        /// </summary>
        /// CreatedBy: Bien (09/05/2023)
        [Test]
        public void Insert_EmptyFullName_ReturnsInvalid()
        {
            // Arange
            var employee = new Employee
            {
                EmployeeCode = "NV-25555",
                FullName = string.Empty,
                DateOfBirth = DateTime.Parse("02/05/2022"),
                Gender = 0,
                DepartmentId = Guid.Parse("142cb08f-7c31-21fa-8e90-67245e8b283e"),
                LandlineNumber = "0225559999",
                PhoneNumber = "098719510",
                Email = "nhaem2@gmail.com",
                Address = "Hà Nội",
                IdentityNumber = "042586512578",
                IdentityDate = DateTime.Parse("05/07/2016"),
                IdentityPlace = "Hà Nội",
                PositionName = "Nhân viên",
                BankAccount = "66668888",
                BankName = "Vietcombank",
                BankBranch = "Hà Nội",
                CreatedDate = DateTime.Now,
                CreatedBy = "Bien",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "Minh"
            };

            _fakeEmployeeDL.Insert(employee).Returns(0);

            var expectedResult = new ServiceResult
            {
                IsSuccess = false,
                ErrorCode = Common.Enums.ErrorCode.ValidateError,
                Message = Common.Resource.ErrorMsg_Validate,
                Data = new Dictionary<string, string>(){{"FullName", "Tên không được để trống" }}
            };

            // Act
            var actualResult = _employeeBL.Insert(employee);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        /// <summary>
        /// Hàm kiểm tra thực hiện hàm thêm nhân viên khi DepartmentID truyền vào là trống
        /// </summary>
        /// CreatedBy: Bien (09/05/2023)
        [Test]
        public void Insert_EmptyDepartmentId_ReturnsInvalid()
        {
            // Arange
            var employee = new Employee
            {
                EmployeeCode = "NV-25555",
                FullName = "Hiển",
                DateOfBirth = DateTime.Parse("02/05/2022"),
                Gender = 0,
                DepartmentId = Guid.Empty,
                LandlineNumber = "0225559999",
                PhoneNumber = "098719510",
                Email = "nhaem2@gmail.com",
                Address = "Hà Nội",
                IdentityNumber = "042586512578",
                IdentityDate = DateTime.Parse("05/07/2016"),
                IdentityPlace = "Hà Nội",
                PositionName = "Nhân viên",
                BankAccount = "66668888",
                BankName = "Vietcombank",
                BankBranch = "Hà Nội",
                CreatedDate = DateTime.Now,
                CreatedBy = "Bien",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "Minh"
            };

            _fakeEmployeeDL.Insert(employee).Returns(0);

            var expectedResult = new ServiceResult
            {
                IsSuccess = false,
                ErrorCode = Common.Enums.ErrorCode.ValidateError,
            };

            // Act
            var actualResult = _employeeBL.Insert(employee);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        /// <summary>
        /// Hàm kiểm tra khi gọi vào DL thành công
        /// </summary>
        /// CreatedBy: Bien (09/05/2023)
        [Test]
        public void Insert_CallEmployeeDL_ReturnsValidCallDL()
        {
            // Arange
            var employee = new Employee
            {
                EmployeeCode = "NV-25555",
                FullName = "Hiển",
                DateOfBirth = DateTime.Parse("02/05/2022"),
                Gender = 0,
                DepartmentId = Guid.Empty,
                LandlineNumber = "0225559999",
                PhoneNumber = "098719510",
                Email = "nhaem2@gmail.com",
                Address = "Hà Nội",
                IdentityNumber = "042586512578",
                IdentityDate = DateTime.Parse("05/07/2016"),
                IdentityPlace = "Hà Nội",
                PositionName = "Nhân viên",
                BankAccount = "66668888",
                BankName = "Vietcombank",
                BankBranch = "Hà Nội",
                CreatedDate = DateTime.Now,
                CreatedBy = "Bien",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "Minh"
            };

            _fakeEmployeeDL.Insert(employee).Returns(1);

            var expectedResult = new ServiceResult()
            {
                IsSuccess = false
            };

            // Act
            var actualResult = _employeeBL.Insert(employee);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
        }

        /// <summary>
        /// Hàm kiểm tra khi gọi vào DL bị lỗi
        /// </summary>
        /// CreatedBy: Bien (09/05/2023)
        [Test]
        public void Insert_ErrorCallEmployeeDL_ReturnsInvalidCallDL()
        {
            // Arange
            var employee = new Employee
            {
                EmployeeCode = "NV-25555",
                FullName = "Hiển",
                DateOfBirth = DateTime.Parse("02/05/2022"),
                Gender = 0,
                DepartmentId = Guid.Empty,
                LandlineNumber = "0225559999",
                PhoneNumber = "098719510",
                Email = "nhaem2@gmail.com",
                Address = "Hà Nội",
                IdentityNumber = "042586512578",
                IdentityDate = DateTime.Parse("05/07/2016"),
                IdentityPlace = "Hà Nội",
                PositionName = "Nhân viên",
                BankAccount = "66668888",
                BankName = "Vietcombank",
                BankBranch = "Hà Nội",
                CreatedDate = DateTime.Now,
                CreatedBy = "Bien",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "Minh"
            };

            _fakeEmployeeDL.Insert(employee).Returns(0);

            var expectedResult = new ServiceResult()
            {
                IsSuccess = false,
            ErrorCode = Common.Enums.ErrorCode.ValidateError,
        };

            // Act
            var actualResult = _employeeBL.Insert(employee);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        /// <summary>
        /// Hàm kiểm tra khi thực hiện sửa nhân viên thành công
        /// </summary>
        /// CreatedBy: Bien (09/05/2023)
        [Test]
        public void Update_Employee_ReturnValid()
        {
            // Arange
            var employee = new Employee
            {
                EmployeeCode = "NV-25263",
                FullName = "Bien",
                DateOfBirth = DateTime.Parse("02/05/2022"),
                Gender = 0,
                DepartmentId = Guid.Parse("4e272fc4-7875-78d6-7d32-6a1673ffca7c"),
                LandlineNumber = "0987419510",
                PhoneNumber = "0987419510",
                Email = "nhaem2@gmail.com",
                Address = "Hà Nội",
                IdentityNumber = "033201005075",
                IdentityDate = DateTime.Parse("05/07/2016"),
                IdentityPlace = "Hà Nội",
                PositionName = "Nhân viên",
                BankAccount = "66668888",
                BankName = "Vietcombank",
                BankBranch = "Hà Nội",
                CreatedDate = DateTime.Now,
                CreatedBy = "Bien",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "Minh"
            };

            _fakeEmployeeDL.Update(employee.EmployeeId,employee).Returns(1);

            var expectedResult = new ServiceResult
            {
                IsSuccess = true,
            };

            // Act
            var actualResult = _employeeBL.Update(employee.EmployeeId, employee);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
        }

        /// <summary>
        /// Hàm kiểm tra sửa nhân viên khi mã nhân viên truyền vào là trống
        /// </summary>
        /// CreatedBy: Bien (09/05/2023)
        [Test]
        public void Update_EmptyCode_ReturnsInvalid()
        {
            // Arange
            var employee = new Employee
            {
                EmployeeCode = string.Empty,
                FullName = "Bien",
                DateOfBirth = DateTime.Parse("02/05/2022"),
                Gender = 0,
                DepartmentId = Guid.Parse("142cb08f-7c31-21fa-8e90-67245e8b283e"),
                LandlineNumber = "0225559999",
                PhoneNumber = "098719510",
                Email = "nhaem2@gmail.com",
                Address = "Hà Nội",
                IdentityNumber = "042586512578",
                IdentityDate = DateTime.Parse("05/07/2016"),
                IdentityPlace = "Hà Nội",
                PositionName = "Nhân viên",
                BankAccount = "66668888",
                BankName = "Vietcombank",
                BankBranch = "Hà Nội",
                CreatedDate = DateTime.Now,
                CreatedBy = "Bien",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "Minh"
            };

            _fakeEmployeeDL.Update(employee.EmployeeId, employee).Returns(0);

            var expectedResult = new ServiceResult
            {
                IsSuccess = false,
                ErrorCode = Common.Enums.ErrorCode.ValidateError,
                Message = Common.Resource.ErrorMsg_Validate
            };

            // Act
            var actualResult = _employeeBL.Update(employee.EmployeeId, employee);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        /// <summary>
        /// Hàm kiểm tra thực hiện hàm thêm nhân viên khi mã nhân viên truyền vào là trống
        /// </summary>
        /// CreatedBy: Bien (09/05/2023)
        [Test]
        public void Update_EmptyFullName_ReturnsInvalid()
        {
            // Arange
            var employee = new Employee
            {
                EmployeeCode = "NV-25555",
                FullName = string.Empty,
                DateOfBirth = DateTime.Parse("02/05/2022"),
                Gender = 0,
                DepartmentId = Guid.Parse("142cb08f-7c31-21fa-8e90-67245e8b283e"),
                LandlineNumber = "0225559999",
                PhoneNumber = "098719510",
                Email = "nhaem2@gmail.com",
                Address = "Hà Nội",
                IdentityNumber = "042586512578",
                IdentityDate = DateTime.Parse("05/07/2016"),
                IdentityPlace = "Hà Nội",
                PositionName = "Nhân viên",
                BankAccount = "66668888",
                BankName = "Vietcombank",
                BankBranch = "Hà Nội",
                CreatedDate = DateTime.Now,
                CreatedBy = "Bien",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "Minh"
            };

            _fakeEmployeeDL.Update(employee.EmployeeId,employee).Returns(0);

            var expectedResult = new ServiceResult
            {
                IsSuccess = false,
                ErrorCode = Common.Enums.ErrorCode.ValidateError,
                Message = Common.Resource.ErrorMsg_Validate
            };

            // Act
            var actualResult = _employeeBL.Update(employee.EmployeeId, employee);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
            Assert.AreEqual(expectedResult.Message, actualResult.Message);
        }

        /// <summary>
        /// Hàm kiểm tra thực hiện hàm sửa nhân viên khi DepartmentID truyền vào là trống
        /// </summary>
        /// CreatedBy: Bien (09/05/2023)
        [Test]
        public void Update_EmptyDepartmentId_ReturnsInvalid()
        {
            // Arange
            var employee = new Employee
            {
                EmployeeCode = "NV-25555",
                FullName = "Hiển",
                DateOfBirth = DateTime.Parse("02/05/2022"),
                Gender = 0,
                DepartmentId = Guid.Empty,
                LandlineNumber = "0225559999",
                PhoneNumber = "098719510",
                Email = "nhaem2@gmail.com",
                Address = "Hà Nội",
                IdentityNumber = "042586512578",
                IdentityDate = DateTime.Parse("05/07/2016"),
                IdentityPlace = "Hà Nội",
                PositionName = "Nhân viên",
                BankAccount = "66668888",
                BankName = "Vietcombank",
                BankBranch = "Hà Nội",
                CreatedDate = DateTime.Now,
                CreatedBy = "Bien",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "Minh"
            };

            _fakeEmployeeDL.Update(employee.EmployeeId, employee).Returns(0);

            var expectedResult = new ServiceResult
            {
                IsSuccess = false,
                ErrorCode = Common.Enums.ErrorCode.ValidateError,
            };

            // Act
            var actualResult = _employeeBL.Update(employee.EmployeeId, employee);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
            Assert.AreEqual(expectedResult.ErrorCode, actualResult.ErrorCode);
        }

        /// <summary>
        /// Hàm kiểm tra tìm kiếm khi id nhân viên không tồn tại
        /// </summary>
        /// CreatedBy: Bien (09/05/2023)
        [Test]
        public void Delete_ErrorEmployeeIdNotExist_ReturnsInvalid()
        {
            // Arrange
            var employeeId = Guid.Parse("74d2efcc-aaee-11ed-a58f-d8d09038d028");

            _fakeEmployeeDL.Delete(employeeId).Returns(0);

            var expectedResult = new ServiceResult
            {
                IsSuccess = false,
            };
            // Act
            var actualResult = _employeeBL.Delete(employeeId);

            // Assert
            Assert.AreEqual(expectedResult.IsSuccess, actualResult.IsSuccess);
        }

        /// <summary>
        /// Hàm kiểm tra mã nhân viên trùng mã nhân viên
        /// </summary>
        /// CreatedBy: Bien (10/05/2023)
        [Test]
        public void CheckEmployeeCode_AlreadyEmployeeCode_ReturnsInvalid()
        {
            // Arange
            var employee = new Employee
            {
                EmployeeCode = "NV-25555",
                FullName = string.Empty,
                DateOfBirth = DateTime.Parse("02/05/2022"),
                Gender = 0,
                DepartmentId = Guid.Parse("142cb08f-7c31-21fa-8e90-67245e8b283e"),
                LandlineNumber = "0225559999",
                PhoneNumber = "098719510",
                Email = "nhaem2@gmail.com",
                Address = "Hà Nội",
                IdentityNumber = "042586512578",
                IdentityDate = DateTime.Parse("05/07/2016"),
                IdentityPlace = "Hà Nội",
                PositionName = "Nhân viên",
                BankAccount = "66668888",
                BankName = "Vietcombank",
                BankBranch = "Hà Nội",
                CreatedDate = DateTime.Now,
                CreatedBy = "Bien",
                ModifiedDate = DateTime.Now,
                ModifiedBy = "Minh"
            };

            _fakeEmployeeDL.CheckEmployeeCode(employee.EmployeeCode).Returns(Guid.Empty);

            var expectedResult = false;
            // Act
            var actualResult = _employeeBL.CheckEmployeeCode(employee);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
