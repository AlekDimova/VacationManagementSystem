using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VacationManagementSystem.Data;
using VacationManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Employee")]
public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public EmployeeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == user.Email);

        if (employee == null)
        {
            return NotFound();
        }

        var leaveBalance = await _context.LeaveBalances.FirstOrDefaultAsync(lb => lb.EmployeeId == employee.EmployeeId);

        if (leaveBalance == null)
        {
            return NotFound();
        }

        return View(leaveBalance);
    }

    public IActionResult RequestLeave()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RequestLeave(LeaveRequest model, IFormFile medicalReport)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == user.Email);

            if (employee == null)
            {
                return NotFound();
            }

            model.EmployeeId = employee.EmployeeId;
            model.Status = "Pending";

            if (model.LeaveType == "Sick")
            {
                if (medicalReport != null && medicalReport.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(medicalReport.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await medicalReport.CopyToAsync(fileStream);
                    }

                    model.MedicalReportPath = "/uploads/" + fileName;
                }
            }
            else
            {
                model.MedicalReportPath = null; // Ensure this is null for other leave types
            }

            _context.LeaveRequests.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        return View(model);
    }
}