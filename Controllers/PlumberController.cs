using Microsoft.AspNetCore.Mvc;
using LeakAlertDemo.Data;
using LeakAlertDemo.Models;

namespace LeakAlertDemo.Controllers
{
    public class PlumberController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PlumberController> _logger;

        public PlumberController(AppDbContext context, ILogger<PlumberController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Plumber plumber, IFormFile file)
        {
            if (!ModelState.IsValid)
                return View(plumber);

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Verification document required");
                return View(plumber);
            }

            var allowedExtensions = new[] { ".pdf", ".jpg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                _logger.LogWarning("Invalid file type uploaded");
                ModelState.AddModelError("", "Invalid file type");
                return View(plumber);
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("", "File too large");
                return View(plumber);
            }

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid() + extension;
            var path = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            plumber.DocumentPath = fileName;

            _context.Plumbers.Add(plumber);
            await _context.SaveChangesAsync();

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}