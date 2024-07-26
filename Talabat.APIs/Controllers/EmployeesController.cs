using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.RepositoriesInterFaces;
using Talabat.Core.Specifications.EmployeeSpecs;

namespace Talabat.APIs.Controllers
{
    public class EmployeesController : BaseAPIController
    {
        private readonly IGenericRepository<Employee> _employeeRepo;

        public EmployeesController(IGenericRepository<Employee> EmployeeRepo)
        {
            _employeeRepo = EmployeeRepo;
        }

        [ProducesResponseType(typeof(Employee),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var spec = new EmployeeWithDepartmentSpecification();
            var employees =await _employeeRepo.GetWithSpecAsync(spec);
            return Ok(employees);
        }
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]

        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesById(int id)
        {
            var spec = new EmployeeWithDepartmentSpecification(id);
            var employees = await _employeeRepo.GetWithSpecAsync(spec);


            if (employees is null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(employees);
        }
    }
}
