using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using project.Areas.Identity.Data;
using project.Models;

namespace project.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public UserController(ApplicationDBContext dbContext , SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this._dbContext = dbContext;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var dpt = _dbContext.Departments.ToList();
            ViewBag.Departments = dpt;
            return View();
        }

        [HttpGet("GetDoctorByDepartment")]
        public IActionResult GetDoctorByDepartmentId(int departmentId)
        {
            var doctor = _dbContext.Doctors.Where(d => d.DepartmentId == departmentId).Select(d => new { d.DoctorId, d.Name }).ToList();
            return Ok(doctor);
        }

        public async Task<IActionResult> add_app(Appointment obj, string pname , int dId , DateTime app_date)
        {
            var user = await _userManager.GetUserAsync(User);
            _dbContext.Appointments.Add(obj);
            obj.status = "Pending";
            obj.AppointmentDate = app_date;
            obj.DoctorId = dId;
            obj.patientName = pname;
            obj.UserId = user.Id;
            _dbContext.SaveChanges();
            return RedirectToAction("index", "user");
        }

    }

}
