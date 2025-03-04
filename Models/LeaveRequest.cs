using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VacationManagementSystem.Models
{
    public class LeaveRequest
    {
        [Key]
        public int LeaveRequestId { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string LeaveType { get; set; } // "Annual", "Bonus", "Sick"

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } // "Pending", "Approved", "Rejected"

        [MaxLength(500)]
        public string Reason { get; set; }

        [MaxLength(255)] // Path to file
        public string MedicalReportPath { get; set; } // Optional

        public Employee Employee { get; set; }
    }
}
