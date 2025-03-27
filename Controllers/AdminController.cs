using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Areas.Identity.Data;
using project.Models;

namespace project.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDBContext _dbContext;
        private readonly IWebHostEnvironment _webHost;

        public AdminController(SignInManager<ApplicationUser> signInManager , UserManager<ApplicationUser> userManager , ApplicationDBContext dbContext , IWebHostEnvironment webHost)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            this._dbContext = dbContext;
            this._webHost = webHost;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user != null)
            {
                if(user.Role == "0")
                {
                    return RedirectToAction("index", "user");
                }else if (user.Role == "1")
                {
                    return View();
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> add_depart()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (user.Role == "0")
                {
                    return RedirectToAction("index", "user");
                }
                else if (user.Role == "1")
                {
                    return View();
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> all_depart()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (user.Role == "0")
                {
                    return RedirectToAction("index", "user");
                }
                else if (user.Role == "1")
                {
                    var dptData = _dbContext.Departments.ToList();
                    return View(dptData);
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> all_doctors()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (user.Role == "0")
                {
                    return RedirectToAction("index", "user");
                }
                else if (user.Role == "1")
                {
                    var doctorData = _dbContext.Doctors.Include(d => d.Department).ToList();
                    return View(doctorData);
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> dlt_doctor(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (user.Role == "0")
                {
                    return RedirectToAction("index", "user");
                }
                else if (user.Role == "1")
                {
                    var drData = await _dbContext.Doctors.FindAsync(id);
                    if(drData != null)
                    {
                        _dbContext.Doctors.Remove(drData);
                        _dbContext.SaveChanges();
                        TempData["success"] = "Dr Deleted SuccessFully!";
                        return Redirect("/admin/all_doctors");

                    }
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> add_doctor()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (user.Role == "0")
                {
                    return RedirectToAction("index", "user");
                }
                else if (user.Role == "1")
                {
                    var dpt = _dbContext.Departments.ToList();
                    ViewBag.Departments = dpt;
                    return View();
                }
            }
            return RedirectToAction("Login", "Account");
        }


        [HttpPost]
        public async Task<IActionResult> add_depart(Department obj)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (user.Role == "0")
                {
                    return RedirectToAction("index", "user");
                }
                else if (user.Role == "1")
                {
                    _dbContext.Departments.Add(obj);
                    _dbContext.SaveChanges();
                    return RedirectToAction("add_depart", "Admin");
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> add_doctor(Doctor obj , IFormFile Image)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (user.Role == "0")
                {
                    return RedirectToAction("index", "user");
                }
                else if (user.Role == "1")
                {
                    string uploadFolder = Path.Combine(_webHost.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    string fileName = Path.GetFileName(Image.FileName);
                    string fileSavePath = Path.Combine(uploadFolder, fileName);
                    using(FileStream stream = new FileStream(fileSavePath , FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    obj.Image = "/uploads/" + fileName;
                    _dbContext.Doctors.Add(obj);
                    _dbContext.SaveChanges();

                    return RedirectToAction("add_doctor", "Admin");
                }
            }
            return RedirectToAction("Login", "Account");
        }


    }
}
