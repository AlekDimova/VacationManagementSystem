using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace VacationManagementSystem.Models
{
    public class BonusDays
    {
        [Key]
        public int BonusDaysId { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        public int Year { get; set; }

        public int Days { get; set; }

        public Employee Employee { get; set; }
    }
}
