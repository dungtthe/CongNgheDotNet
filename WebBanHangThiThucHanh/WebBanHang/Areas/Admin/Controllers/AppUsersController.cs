using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebBanHang.Helpers;
using WebBanHang.Models;
using WebBanHang.Security;
using WebBanHang.ViewModel;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorizationFilter]
    [Route("admin/editUserVM")]
    public class AppUsersController : Controller
    {
        private readonly WebDbContext _context;

        public AppUsersController(WebDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var webDbContext = _context.AppUsers.Include(a => a.Role);
            return View(await webDbContext.ToListAsync());
        }

        // GET: Admin/AppUsers/Details/5
        [HttpGet("details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AppUsers == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUsers
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // GET: Admin/AppUsers/Create
        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName");
            return View();
        }

        // POST: Admin/AppUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,UserName,Password,Email,PhoneNumber,RoleId")] CreateAppUserViewModel appUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var checkUser = Utils.CheckTonTaiUserNameAndEmail(appUserViewModel.UserName, appUserViewModel.Email, _context);

                if (!checkUser)
                {
                    var appUser = new AppUser
                    {
                        Name = appUserViewModel.Name,
                        UserName = appUserViewModel.UserName,
                        Password = appUserViewModel.Password,
                        Email = appUserViewModel.Email,
                        PhoneNumber = appUserViewModel.PhoneNumber,
                        RoleId = appUserViewModel.RoleId,
                        IsLock = false
                    };
                    _context.Add(appUser);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tên tài khoản hoặc email đã tồn tại.");
                    ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName", 1);
                    return View(appUserViewModel);
                }
            }

            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName", 1);
            return View(appUserViewModel);
        }


        // GET: Admin/AppUsers/Edit/5
        [HttpGet("edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AppUsers == null)
            {
                return NotFound();
            }

            var user = _context.AppUsers.Include(p => p.Role).FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName", user.Role.RoleName);

            return View(user);
        }

        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,UserName,Password,PhoneNumber,IsLock,RoleId,Email")] EditAppUserViewModel editUserVM)
        {
            var appUser = _context.AppUsers.Include(p => p.Role).FirstOrDefault(u => u.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var checkUser = _context.AppUsers
                    .Any(user => (user.UserName == editUserVM.UserName || user.Email == editUserVM.Email) && user.Id != id);

                if (!checkUser)
                {
                    appUser.Name = editUserVM.Name;
                    appUser.UserName = editUserVM.UserName;
                    appUser.Email = editUserVM.Email;
                    appUser.Password = editUserVM.Password;
                    appUser.PhoneNumber = editUserVM.PhoneNumber;
                    appUser.RoleId = editUserVM.RoleId;
                    appUser.IsLock = editUserVM.IsLock;

                    _context.Update(appUser);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tên tài khoản hoặc email đã tồn tại.");
                }
            }

            var model = new AppUser
            {
                Id = id,
                Name = editUserVM.Name,
                UserName = editUserVM.UserName,
                Email = editUserVM.Email,
                Password = editUserVM.Password,
                PhoneNumber = editUserVM.PhoneNumber,
                RoleId = editUserVM.RoleId,
                IsLock = editUserVM.IsLock
            };

            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName", appUser.RoleId);
            return View(model);
        }


        [HttpGet("delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _context.AppUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                _context.AppUsers.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AppUserExists(int id)
        {
            return (_context.AppUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
