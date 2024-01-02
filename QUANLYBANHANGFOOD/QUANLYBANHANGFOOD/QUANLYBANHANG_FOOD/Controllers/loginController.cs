using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QUANLYBANHANG_FOOD.Controllers
{
    
    public class loginController : Controller
    {
        QUANLYBANHANG_FOODEntities1 DB = new QUANLYBANHANG_FOODEntities1();
        // GET: login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult checkLogin(string taikhoan, string matkhau)
        {
                // Kiểm tra xem có tài khoản nào có tên đăng nhập tương ứng không
                var user = DB.ACCOUNT.FirstOrDefault(x => x.Username.Equals(taikhoan));
            //var idKH = DB.KHACHHANG.FirstOrDefault(p => p.Username.Equals(taikhoan));
                // Kiểm tra xem người dùng được tìm thấy và mật khẩu khớp không
                if (user != null && user.Pass.Equals(matkhau))
                {
                    Session["IsAuthenticated"] = true;
                    var khachhang = DB.KHACHHANG.FirstOrDefault(p => p.Username.Equals(taikhoan));
                    
                    Session["khachhangID"] = khachhang.MaKH;
                // Đăng nhập thành công, chuyển hướng đến trang chủ
                //ViewBag.TitleName = user;
                return RedirectToAction("Index", "Trangchu");
                }
                ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng !";
                return View("Index");
        }
        public ActionResult Logout()
        {
            // Xóa session khi đăng xuất
            Session["IsAuthenticated"] = null;

            // Chuyển hướng đến trang đăng nhập
            return RedirectToAction("Index");
        }
    }
}