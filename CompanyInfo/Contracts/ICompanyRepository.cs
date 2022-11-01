using CompanyInfo.Dtos;
using CompanyInfo.Models;

namespace CompanyInfo.Contracts
{
    public interface ICompanyRepository
    {
        // 查詢所有 Companies 資料
        public Task<IEnumerable<Company>> GetCompanies();
        // 查詢指定 id 的單一 Company 資料
        public Task<Company> GetCompany(Guid id);
        // 新增 Company 資料
        public Task<Company> CreateCompany(CompanyForCreationDto company);
        // 修改指定 id 的 Company 資料
        public Task UpdateCompany(Guid id, CompanyForUpdateDto company);
        // 刪除指定 id 的 Company 資料
        public Task DeleteCompany(Guid id);
        // 查詢指定 Employee 的 id 所在 Company 資料
        public Task<Company> GetCompanyByEmployeeId(Guid id);
        // 查詢指定 Company 所屬的所有 Employees 資料
        public Task<Company> GetCompanyEmployeesMultipleResults(Guid id);
        // 查詢所有的 Companies，以及它底下的所有 Employees 資料
        public Task<List<Company>> GetCompaniesEmployeesMultipleMapping();
        // 批次新增多筆 Companies 資料
        public Task CreateMultipleCompanies(List<CompanyForCreationDto> companies);
    }
}
