using Dapper;
using System.Data;
using CompanyInfo.Dtos;
using CompanyInfo.Models;
using CompanyInfo.Contracts;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Net;

namespace CompanyInfo.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly string _connectString = DBUtil.ConnectionString();

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            string sqlQuery = "SELECT * FROM Companies";
            using (var connection = new SqlConnection(_connectString))
            {
                var companies = await connection.QueryAsync<Company>(sqlQuery);
                return companies.ToList();
            }
        }

        public async Task<Company> GetCompany(Guid id)
        {
            string sqlQuery = "SELECT * FROM Companies WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectString))
            {
                var company = await connection.QuerySingleOrDefaultAsync<Company>(sqlQuery, new { Id = id });
                return company;
            }
        }

        public async Task<Company> CreateCompany(CompanyForCreationDto company)
        {
            string sqlQuery = "INSERT INTO Companies (Id, Name, Address, Country)VALUES(@Id, @Name, @Address, @Country)";
            var parameters = new DynamicParameters();
            Guid Cid = Guid.NewGuid();
            parameters.Add("Id", Cid, DbType.Guid);
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);
            using (var connection = new SqlConnection(_connectString))
            {
                await connection.ExecuteAsync(sqlQuery, parameters);
                var createdCompany = new Company
                {
                    Id = Cid,
                    Name = company.Name,
                    Address = company.Address,
                    Country = company.Country
                };

                return createdCompany;
            }
        }

        public async Task UpdateCompany(Guid id, CompanyForUpdateDto company)
        {
            var query = "UPDATE Companies SET Name = @Name, Address =@Address, Country = @Country WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Guid);
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);
            using (var connection = new SqlConnection(_connectString))
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteCompany(Guid id)
        {
            var query = "DELETE FROM Companies WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectString))
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

        public async Task<Company> GetCompanyByEmployeeId(Guid id)
        {
            // 設定要被呼叫的 stored procedure 名稱
            var procedureName = "ShowCompanyForProvidedEmployeeId";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Guid, ParameterDirection.Input);
            using (var connection = new SqlConnection(_connectString))
            {
                var company = await connection.QueryFirstOrDefaultAsync<Company>
                (procedureName, parameters, commandType: CommandType.StoredProcedure);
                return company;
            }
        }

        public async Task<Company> GetCompanyEmployeesMultipleResults(Guid id)
        {
            var query = "SELECT * FROM Companies WHERE Id = @Id;" +
            "SELECT * FROM Employees WHERE CompanyId = @Id";
            using (var connection = new SqlConnection(_connectString))
            using (var multi = await connection.QueryMultipleAsync(query, new { id }))
            {
                var company = await multi.ReadSingleOrDefaultAsync<Company>();
                if (company != null)
                    company.Employees = (await multi.ReadAsync<Employee>()).ToList();
                return company;
            }
        }

        public async Task<List<Company>> GetCompaniesEmployeesMultipleMapping()
        {
            var query = "SELECT * FROM Companies c JOIN Employees e ON c.Id = e.CompanyId";
            using (var connection = new SqlConnection(_connectString))
            {
                var companyDict = new Dictionary<Guid, Company>();
                var companies = await connection.QueryAsync<Company, Employee, Company>(
                query, (company, employee) =>
                {
                    if (!companyDict.TryGetValue(company.Id, out var currentCompany))
                    {
                        currentCompany = company;
                        companyDict.Add(currentCompany.Id, currentCompany);
                    }
                    currentCompany.Employees.Add(employee);
                    return currentCompany;
                }
                );
                return companies.Distinct().ToList();
            }
        }

        public async Task CreateMultipleCompanies(List<CompanyForCreationDto> companies)
        {
            var query = "INSERT INTO Companies (Name, Address, Country) VALUES (@Name, @Address, @Country)";
            using (var connection = new SqlConnection(_connectString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {

                    foreach (var company in companies)
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("Name", company.Name, DbType.String);
                        parameters.Add("Address", company.Address, DbType.String);
                        parameters.Add("Country", company.Country, DbType.String);
                        await connection.ExecuteAsync(query, parameters, transaction: transaction);
                        //throw new Exception();
                    }
                    transaction.Commit();
                }
            }
        }
    }

}
