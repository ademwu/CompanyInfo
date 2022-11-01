using CompanyInfo.Dtos;
using CompanyInfo.Models;

namespace CompanyInfo.Contracts
{
    public interface IEmployeeRepository
    {
        // 查詢所有 Employee 資料
        public Task<IEnumerable<Employee>> GetEmployees();
        // 查詢指定 id 的單一 Employee 資料
        public Task<Employee> GetEmployee(Guid id);
        // 新增 Employee 資料
        public Task<Employee> CreateEmployee(EmployeeForCreationDto employee);
        // 修改指定 id 的 Employee 資料
        public Task UpdateEmployee(Guid id, EmployeeForUpdateDto employee);
        // 刪除指定 id 的 Employee 資料
        public Task DeleteEmployee(Guid id);
    }

}
