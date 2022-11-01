using CompanyInfo.Dtos;
using CompanyInfo.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CompanyInfo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly ICompanyRepository _companyRepo;
        public CompaniesController(ILogger<CompaniesController> logger, ICompanyRepository
        companyRepo)
        {
            _logger = logger;
            _companyRepo = companyRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            try
            {
                var companies = await _companyRepo.GetCompanies();
                return Ok(new
                {
                    Success = true,
                    Message = "all companies returned.",
                    companies
                });
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // [HttpGet]
        // [Route("{id}")]
        [HttpGet("{id}", Name = "CompanyById")]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            try
            {
                var company = await _companyRepo.GetCompany(id);
                if (company == null)
                    return NotFound();
                return Ok(new
                {
                    Success = true,
                    Message = "Company fetched.",
                    company
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]

        public async Task<IActionResult> CreateCompany(CompanyForCreationDto company)
        {
            try
            {
                var createdCompany = await _companyRepo.CreateCompany(company);
                return Ok(new
                {
                    Success = true,
                    Message = "Company created.",
                    createdCompany
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCompany(Guid id, CompanyForUpdateDto company)
        {
            try
            {
                var dbCompany = await _companyRepo.GetCompany(id);
                if (dbCompany == null)
                    return NotFound();
                await _companyRepo.UpdateCompany(id, company);
                return Ok(new
                {
                    Success = true,
                    Message = "Company updated."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            try
            {

                var dbCompany = await _companyRepo.GetCompany(id);
                if (dbCompany == null)
                    return NotFound();
                await _companyRepo.DeleteCompany(id);
                return Ok(new
                {
                    Success = true,
                    Message = "Company deleted."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("ByEmployeeId/{id}")]
        public async Task<IActionResult> GetCompanyForEmployee(Guid id)
        {
            try
            {
                var company = await _companyRepo.GetCompanyByEmployeeId(id);
                if (company == null)
                    return NotFound();
                return Ok(company);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}/MultipleResult")]
        public async Task<IActionResult> GetCompanyEmployeesMultipleResult(Guid id)
        {
            try
            {
                var company = await _companyRepo.GetCompanyEmployeesMultipleResults(id);
                if (company == null)
                    return NotFound();
                return Ok(company);
            }
            catch (Exception ex)
            {
                //log error

                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("MultipleMapping")]
        public async Task<IActionResult> GetCompaniesEmployeesMultipleMapping()
        {
            try
            {
                var company = await _companyRepo.GetCompaniesEmployeesMultipleMapping();
                return Ok(company);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("multiple")]
        public async Task<IActionResult> CreateCompany(List<CompanyForCreationDto> companies)
        {
            try
            {
                await _companyRepo.CreateMultipleCompanies(companies);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }


    }
}
