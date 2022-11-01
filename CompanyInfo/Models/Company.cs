using System.ComponentModel.DataAnnotations;

namespace CompanyInfo.Models
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Maximum 50 characters")]
        public String Name { get; set; }
        [Required]
        [StringLength(150, ErrorMessage = "Maximum 50 characters")]
        public String Address { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Maximum 50 characters")]
        public String Country { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
