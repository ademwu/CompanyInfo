using CompanyInfo.Dtos;
using CompanyInfo.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CompanyInfo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {

        private readonly ILogger<EmployeesController> _logger;
        private readonly IEmployeeRepository _employeeRepo;
        public EmployeesController(ILogger<EmployeesController> logger, IEmployeeRepository
        employeeRepo)
        {
            _logger = logger;
            _employeeRepo = employeeRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await _employeeRepo.GetEmployees();
                return Ok(new
                {
                    Success = true,
                    Message = "all employees returned.",
                    employees
                });
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        //[HttpGet("{id}", Name = "CompanyById")]
        public async Task<IActionResult> GetEmployee(Guid id)
        {
            try
            {
                var employee = await _employeeRepo.GetEmployee(id);
                if (employee == null)
                    return NotFound();
                return Ok(new
                {

                    Success = true,
                    Message = "Employee fetched.",
                    employee
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeForCreationDto employee)
        {
            try
            {
                var createdEmployee = await _employeeRepo.CreateEmployee(employee);
                return Ok(new
                {
                    Success = true,
                    Message = "Employee created.",
                    createdEmployee
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, EmployeeForUpdateDto employee)
        {
            try
            {
                var dbEmployee = await _employeeRepo.GetEmployee(id);
                if (dbEmployee == null)
                    return NotFound();
                await _employeeRepo.UpdateEmployee(id, employee);
                return Ok(new
                {
                    Success = true,
                    Message = "Employee updated."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                var dbEmployee = await _employeeRepo.GetEmployee(id);
                if (dbEmployee == null)
                    return NotFound();
                await _employeeRepo.DeleteEmployee(id);
                return Ok(new
                {
                    Success = true,
                    Message = "Employee deleted."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
