using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebBanHang.Models;
using WebBanHang.Models.Authentication;
using X.PagedList;

namespace WebBanHang.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        QLBanVaLiContext db = new QLBanVaLiContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        [Authentication]
        public IActionResult Index(int ?page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstSanPham = db.TDanhMucSps.AsNoTracking().OrderBy(x=>x.TenSp);
            
            //TDanhMucSp là bảng SanPham
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstSanPham, pageNumber, pageSize);
            return View(lst);
        }


        [Authentication]
        public IActionResult SanPhamTheoLoai(string maloai, int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstSanPham = db.TDanhMucSps.AsNoTracking().Where(x=>x.MaLoai==maloai).OrderBy(x => x.TenSp);

            ViewBag.maloai = maloai;
            //TDanhMucSp là bảng SanPham
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstSanPham, pageNumber, pageSize);
            return View(lst);
        }




        [Authentication]
        public async Task<IActionResult> ChiTietSanPham(string maSp)
        {
            var sanPham = await db.TDanhMucSps.FirstOrDefaultAsync(x => x.MaSp == maSp);
            if (sanPham == null)
            {
                return NotFound(); 
            }

            var anhSanPham = await db.TAnhSps.Where(x => x.MaSp == maSp).ToListAsync();
            ViewBag.anhSanPham = anhSanPham;
            return View(sanPham);
        }







        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
