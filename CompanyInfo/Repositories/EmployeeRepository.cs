using Dapper;
using System.Data;
using CompanyInfo.Dtos;
using CompanyInfo.Models;
using CompanyInfo.Contracts;
using Microsoft.Data.SqlClient;
using System.ComponentModel.Design;

namespace CompanyInfo.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectString = DBUtil.ConnectionString();

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            string sqlQuery = "SELECT * FROM Employees";
            using (var connection = new SqlConnection(_connectString))
            {
                var employees = await connection.QueryAsync<Employee>(sqlQuery);
                return employees.ToList();
            }
        }

        public async Task<Employee> GetEmployee(Guid id)
        {
            string sqlQuery = "SELECT * FROM Employees WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectString))

            {
                var employee = await connection.QuerySingleOrDefaultAsync<Employee>(sqlQuery, new { Id = id });
                return employee;
            }
        }

        public async Task<Employee> CreateEmployee(EmployeeForCreationDto employee)
        {
            string sqlQuery = "INSERT INTO Employees (Id, Name, Age, Position, CompanyId)VALUES(@Id, @Name, @Age, @Position, @CompanyId)";
            var parameters = new DynamicParameters();
            Guid Eid = Guid.NewGuid();
            parameters.Add("Id", Eid, DbType.Guid);
            parameters.Add("Name", employee.Name, DbType.String);
            parameters.Add("Age", employee.Age, DbType.Int16);
            parameters.Add("Position", employee.Position, DbType.String);
            parameters.Add("CompanyId", employee.CompanyId, DbType.Guid);
            using (var connection = new SqlConnection(_connectString))
            {
                await connection.ExecuteAsync(sqlQuery, parameters);
                var createdEmployee = new Employee
                {
                    Id = Eid,
                    Name = employee.Name,
                    Age = employee.Age,
                    Position = employee.Position,
                    CompanyId = employee.CompanyId
                };
                return createdEmployee;
            }
        }

        public async Task UpdateEmployee(Guid id, EmployeeForUpdateDto employee)
        {
            var query = "UPDATE Employees SET Name = @Name, Age = @Age, Position =@Position, CompanyId = @CompanyId WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Guid);
            parameters.Add("Name", employee.Name, DbType.String);
            parameters.Add("Age", employee.Age, DbType.Int16);
            parameters.Add("Position", employee.Position, DbType.String);
            parameters.Add("CompanyId", employee.CompanyId, DbType.Guid);
            using (var connection = new SqlConnection(_connectString))

            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteEmployee(Guid id)
        {
            var query = "DELETE FROM Employees WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectString))
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

    }

}
