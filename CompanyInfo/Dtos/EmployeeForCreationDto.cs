using System.ComponentModel.DataAnnotations;

namespace CompanyInfo.Dtos
{
    public class EmployeeForCreationDto
    {
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
