using System.ComponentModel.DataAnnotations;

namespace CompanyInfo.Models
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Maximum 50 characters")]
        public String Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Maximum 50 characters")]
        public String Position { get; set; }
        [Required]
        public Guid CompanyId { get; set; }
    }
}
