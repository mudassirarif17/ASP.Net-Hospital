using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Areas.Identity.Data;
using project.Models;
using System.Net;
using System.Net.Mail;

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

        public async Task<IActionResult> dlt_department(int id)
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
                    var dpData = await _dbContext.Departments.FindAsync(id);
                    if (dpData != null)
                    {
                        _dbContext.Departments.Remove(dpData);
                        _dbContext.SaveChanges();
                        TempData["success"] = "Department Deleted SuccessFully!";
                        return Redirect("/admin/all_depart");

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

        public async Task<IActionResult> pending_app()
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
                    //var pen_app = _dbContext.Appointments.Include(d => d.Doctor).Include(d => d.User).ToList();
                    var pen_app = _dbContext.Appointments.Where(a => a.status == "Pending").Include(d => d.Doctor).Include(d => d.User).ToList();
                    return View(pen_app);
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> approved_app()
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
                    //var pen_app = _dbContext.Appointments.Include(d => d.Doctor).Include(d => d.User).ToList();
                    var pen_app = _dbContext.Appointments.Where(a => a.status == "Approved").Include(d => d.Doctor).Include(d => d.User).ToList();
                    return View(pen_app);
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> canceled_app()
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
                    //var pen_app = _dbContext.Appointments.Include(d => d.Doctor).Include(d => d.User).ToList();
                    var pen_app = _dbContext.Appointments.Where(a => a.status == "Cancelled").Include(d => d.Doctor).Include(d => d.User).ToList();
                    return View(pen_app);
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> app_approved(int id)
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
                    var app = await _dbContext.Appointments.FindAsync(id);
                    if (app == null)
                    {
                        return NotFound();
                    }

                    app.status = "Approved";
                    _dbContext.Appointments.Update(app);
                    await _dbContext.SaveChangesAsync();

                    var forEmail = "mudassirarif04@gmail.com";
                    var forPassword = "wcyk nktm eiwa zpzj";
                    var subject = "Appointment Approved";
                    var body = $"Dear {user.FirstName} {user.LastName} Your Appointment has been Approved Thanks for using our service";

                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(forEmail, forPassword),
                        EnableSsl = true,
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(forEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = false
                    };

                    mailMessage.To.Add(user.Email);

                    try
                    {
                        smtpClient.Send(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Email Sending Failed " + ex.Message);
                    }

                    return RedirectToAction("pending_app", "Admin");
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> app_canceled(int id)
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
                    var app = await _dbContext.Appointments.FindAsync(id);
                    if (app == null)
                    {
                        return NotFound();
                    }

                    app.status = "Canceled";
                    _dbContext.Appointments.Update(app);
                    await _dbContext.SaveChangesAsync();

                    var forEmail = "mudassirarif04@gmail.com";
                    var forPassword = "wcyk nktm eiwa zpzj";
                    var subject = "Appointment Canceled";
                    var body = $"Dear {user.FirstName} {user.LastName} Your Appointment has been canceled due to some reason Thanks for using our service";

                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(forEmail, forPassword),
                        EnableSsl = true,
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(forEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = false
                    };

                    mailMessage.To.Add(user.Email);

                    try
                    {
                        smtpClient.Send(mailMessage);
                    }catch(Exception ex)
                    {
                        Console.WriteLine("Email Sending Failed "+ ex.Message);
                    }

                    return RedirectToAction("pending_app", "Admin");
                }
            }
            return RedirectToAction("Login", "Account");
        }

    }
}
