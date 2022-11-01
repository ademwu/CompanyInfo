using System.ComponentModel.DataAnnotations;

namespace CompanyInfo.Dtos
{
    public class CompanyForCreationDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Maximum 50 characters")]
        public String Name { get; set; }
        [Required]
        [StringLength(150, ErrorMessage = "Maximum 50 characters")]
        public String Address { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Maximum 50 characters")]
        public String Country { get; set; }
    }
}
