
using System.Linq;
using System.Web.Mvc;

namespace QUANLYBANHANG_FOOD.Controllers
{
    
    public class loginController : Controller
    {
        QUANLYBANHANG_FOOD_XMLEntities DB = new QUANLYBANHANG_FOOD_XMLEntities();
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

                // Kiểm tra xem người dùng được tìm thấy và mật khẩu có trùng khớp không
                if (user != null && user.Pass.Equals(matkhau))
                {
                Session["IsAuthenticated"] = true;
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