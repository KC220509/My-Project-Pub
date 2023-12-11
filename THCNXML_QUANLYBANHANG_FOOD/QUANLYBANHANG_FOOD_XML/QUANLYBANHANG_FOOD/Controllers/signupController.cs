using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QUANLYBANHANG_FOOD.Controllers
{
    public class signupController : Controller
    {
        QUANLYBANHANG_FOOD_XMLEntities DB = new QUANLYBANHANG_FOOD_XMLEntities();
        // GET: signup
        public ActionResult index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckSignup(string taikhoan, string phone, string matkhau)
        {

            // Kiểm tra xem tên đăng nhập đã tồn tại chưa
            if (DB.ACCOUNT.Any(acc => acc.Username == taikhoan))
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại !";
                return View("index");
            }

            // Tạo một đối tượng ACCOUNT mới và thêm vào DB
            var newAccount = new ACCOUNT
            {
                Username = taikhoan,
                Pass = matkhau,
                SDT = phone
            };

            DB.ACCOUNT.Add(newAccount);

            // Lưu thay đổi vào cơ sở dữ liệu
            DB.SaveChanges();

            return RedirectToAction("Index", "login");
        }
    }
}