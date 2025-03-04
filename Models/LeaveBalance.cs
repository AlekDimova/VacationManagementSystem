using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VacationManagementSystem.Models
{
    public class LeaveBalance
    {
        [Key]
        public int LeaveBalanceId { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        public int AnnualLeaveDays { get; set; } // 21 initially

        public int BonusLeaveDays { get; set; }

        public Employee Employee { get; set; }
    }
}
