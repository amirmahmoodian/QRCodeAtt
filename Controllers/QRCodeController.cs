using System;
using MessagingToolkit.QRCode.Codec;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCodeAttendance.Models;
using MessagingToolkit.QRCode.Codec.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Linq;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using IdentitySample.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using login.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Runtime.ConstrainedExecution;

namespace QRCodeAttendance.Controllers
{
    //[Route("api/[controller]/[action]")]
    //[ApiController]
    [Authorize]
    public class QRCodeController :Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;

        public QRCodeController(ApplicationDbContext context, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration([Bind("Id,UserName,UserCode")] QRCodeModel code)
        {
            try
            {
                if (!UserExists2(code.UserName))
                {
                    QRCodeEncoder encoder = new QRCodeEncoder();
                    encoder.QRCodeScale = 8;
                    Bitmap bmp = encoder.Encode(code.UserName);
                    var Usercode = DateTime.Now.ToString("yy") + bmp.GetHashCode().ToString();
                    byte[] BitmapArray = bmp.BitmapToByteArray();
                    string ActtualFileUrl = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
                    ViewBag.QrCodeUri = ActtualFileUrl;
                    code.ActtualFileUrl = ActtualFileUrl;
                    code.UserCode = Usercode;
                    
                    ViewBag.Usercode = Usercode;
                    if (ModelState.IsValid)
                    {
                        _context.Add(code);
                        await _context.SaveChangesAsync();
                        return View();
                    }
                }
                else ViewBag.Usercode = "User Already Exists";
            }
            catch
            {
                ViewBag.Usercode = "Somthing went wrong";
            }
            return View();
        }

        [HttpGet]
        public IActionResult Submission()
        {
            return View();
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            var filePath = _env.ContentRootPath + "/Photos/" + file.FileName;
            using (FileStream ms = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(ms);
            }
            return filePath;
        }

        // GET: Movies
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index(string searchString)
        {

            var code = from m in _context.QRCode
                         select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.keyword = searchString;
                code = code.Where(s => s.UserName.Contains(searchString));
            }

            return View(await code.ToListAsync());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmissionTag(QRCodeModel code, [Bind("Id,UserName,Date")] logsModel log)
        {
            try
            {
                if (code.FileUri is not null)
                {
                    string path = await UploadImage(code.FileUri);
                    code.UserCode = path;
                    QRCodeDecoder decoder = new QRCodeDecoder();
                    string DecodedQR = decoder.Decode(new QRCodeBitmapImage(Image.FromFile(path) as Bitmap));
                    ViewBag.QrCodeUri = DecodedQR;
                    foreach (var item in _context.QRCode)
                    {
                        if (DecodedQR == (item.UserName))
                        {
                            ViewBag.QrCodeUri = "User '" + item.UserName +
                                "' Submited in " + DateTime.Now;
                            log.UserName = item.UserName;
                            log.Date = DateTime.Today;
                            break;
                        }
                        else
                        {
                            ViewBag.QrCodeUri = "NOT VALID";
                        }
                    }
                }
                if (ModelState.IsValid)
                {
                    _context.Add(log);
                    await _context.SaveChangesAsync();
                    return View("Submission");
                }
                return View("Submission");
            }
            catch
            {
                ViewBag.QrCodeUri = "NOT VALID";
                return View("Submission");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmissionCode(QRCodeModel code, [Bind("Id,UserName,Date")] logsModel log)
        {
            try
            {
                string usercode = code.UserCode;
                foreach (var item in _context.QRCode)
                {
                    if (usercode == (item.UserCode))
                    {
                        ViewBag.code = "User '" + item.UserName +
                            "' Submited in " + DateTime.Now;
                        log.UserName = item.UserName;
                        log.Date = DateTime.Today;
                        break;
                    }
                    else
                    {
                        ViewBag.code = "NOT VALID";
                    }
                }
                if (ModelState.IsValid)
                {
                    _context.Add(log);
                    await _context.SaveChangesAsync();
                    return View("Submission");
                }
                return View("Submission");
            }
            catch
            {
                ViewBag.code = "NOT VALID";
                return View("Submission");
            }
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.QRCode
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Detailslog(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.log
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }
        // GET: Movies/Create
        [Authorize(Roles = "admin")]
        public IActionResult CreateLog()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,UserName,UserCode,FileUri")] QRCodeModel code)
        {
            if (code.FileUri != null)
            {
                var filePath = _env.ContentRootPath + "/Photos/" + code.FileUri.FileName;
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    await code.FileUri.CopyToAsync(fs);
                }
                code.ActtualFileUrl = filePath;
            }

            if (ModelState.IsValid)
            {
                if (!UserExists2(code.UserName)) { 
                    _context.Add(code);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(code);
        }

        // GET: Movies
        public async Task<IActionResult> logs(DateTime startdate, DateTime enddate, string SearchString)
        {
            var log = from m in _context.log
                      select m;
            if (!User.IsInRole("admin"))
            {
                log = log.Where(s => s.UserName == User.FindFirstValue(ClaimTypes.Name));
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                ViewBag.key1 = SearchString;
                log = log.Where(s => s.UserName.Contains(SearchString));
            }
            if (startdate != DateTime.MinValue)
            {
                log = log.Where(s => s.Date > (startdate));
                ViewBag.startdate = startdate;
            }
            if (enddate != DateTime.MinValue)
            {
                log = log.Where(s => s.Date < (enddate));
                ViewBag.enddate = enddate;
            }

            return View(await log.ToListAsync());
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Createlog([Bind("Id,UserName,Date")] logsModel log)
        {
            if (ModelState.IsValid)
            {
                _context.Add(log);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(logs));
            }
            return View(log);
        }
        // GET: Movies/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.QRCode.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }
        // GET: Movies/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Editlog(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.log.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,UserCode")] QRCodeModel code)
        {
            if (id != code.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(code);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(code.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(code);
        }
        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Editlog(int id, [Bind("Id,UserName,Date")] logsModel log)
        {
            if (id != log.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(log);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(log.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(logs));
            }
            return View(log);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.QRCode
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
        // GET: Movies/Delete/5
        public async Task<IActionResult> Deletelog(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.log
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.QRCode.FindAsync(id);
            _context.QRCode.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Deletelog")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedlog(int id)
        {
            var movie = await _context.log.FindAsync(id);
            _context.log.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(logs));
        }

        private bool UserExists(int id)
        {
            return _context.QRCode.Any(e => e.Id == id);
        }
        private bool UserExists2(string username)
        {
            return _context.QRCode.Any(e => e.UserName == username);
        }

    }



    //Extension method to convert Bitmap to Byte Array
    public static class BitmapExtension
    {
        public static object Request { get; private set; }

        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}

