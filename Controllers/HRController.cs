using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VacationManagementSystem.Data;
using VacationManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace VacationManagementSystem.Controllers
{
    [Authorize(Roles = "HR")]
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HRController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Leave Request Management
        public async Task<IActionResult> ApproveLeave(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null) return NotFound();

            leaveRequest.Status = "Approved";
            _context.LeaveRequests.Update(leaveRequest);
            await UpdateLeaveBalance(leaveRequest); // Update leave balance
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageLeaveRequests");
        }

        public async Task<IActionResult> RejectLeave(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null) return NotFound();

            leaveRequest.Status = "Rejected";
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageLeaveRequests");
        }

        // Employee Management
        public IActionResult ManageEmployees()
        {
            var employees = _context.Employees.ToList();
            return View(employees);
        }

        public IActionResult AddEmployee() => View();

        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageEmployees");
            }
            return View(employee);
        }

        public async Task<IActionResult> EditEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageEmployees");
            }
            return View(employee);
        }

        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageEmployees");
        }

        // Bonus Days Management
        public IActionResult ManageBonusDays()
        {
            var bonusDays = _context.BonusDays.ToList();
            return View(bonusDays);
        }

        public IActionResult AddBonusDay() => View();

        [HttpPost]
        public async Task<IActionResult> AddBonusDay(BonusDays bonusDay)
        {
            if (ModelState.IsValid)
            {
                _context.BonusDays.Add(bonusDay);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageBonusDays");
            }
            return View(bonusDay);
        }

        public async Task<IActionResult> EditBonusDay(int id)
        {
            var bonusDay = await _context.BonusDays.FindAsync(id);
            if (bonusDay == null) return NotFound();
            return View(bonusDay);
        }

        [HttpPost]
        public async Task<IActionResult> EditBonusDay(BonusDays bonusDay)
        {
            if (ModelState.IsValid)
            {
                _context.BonusDays.Update(bonusDay);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageBonusDays");
            }
            return View(bonusDay);
        }

        public async Task<IActionResult> DeleteBonusDay(int id)
        {
            var bonusDay = await _context.BonusDays.FindAsync(id);
            if (bonusDay == null) return NotFound();
            _context.BonusDays.Remove(bonusDay);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageBonusDays");
        }

        // Leave Requests List
        public IActionResult ManageLeaveRequests()
        {
            var leaveRequests = _context.LeaveRequests.ToList();
            return View(leaveRequests);
        }

        // Report Generation
        public IActionResult GenerateReport()
        {
            // Simple report: All leave requests
            var leaveRequests = _context.LeaveRequests.ToList();
            return View(leaveRequests);
        }

        // Helper Method for Updating Leave Balance
        private async Task UpdateLeaveBalance(LeaveRequest leaveRequest)
        {
            var leaveBalance = await _context.LeaveBalances.FirstOrDefaultAsync(lb => lb.EmployeeId == leaveRequest.EmployeeId);
            if (leaveBalance == null) return;

            int days = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;
            if (leaveRequest.LeaveType == "Annual")
            {
                leaveBalance.AnnualLeaveDays -= days;
            }
            else if (leaveRequest.LeaveType == "Bonus")
            {
                leaveBalance.BonusLeaveDays -= days;
            }
            _context.LeaveBalances.Update(leaveBalance);
        }
    }
}
