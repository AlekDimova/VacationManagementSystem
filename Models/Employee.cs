using System.ComponentModel.DataAnnotations;

namespace VacationManagementSystem.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } // Consider hashing in real apps

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } // "Employee" or "HR"

        [Required]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        // Navigation properties
        public LeaveBalance LeaveBalance { get; set; }
        public ICollection<LeaveRequest> LeaveRequests { get; set; }
        public ICollection<BonusDays> BonusDays { get; set; }
    }
}
