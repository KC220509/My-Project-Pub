using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace QUANLYBANHANG_FOOD.Controllers
{

    public class TrangchuController : Controller
    {
        QUANLYBANHANG_FOODEntities1 DB = new QUANLYBANHANG_FOODEntities1();

        private string khachhangID;
        private string tenKhachHang;
        private void getKhachHangID()
        {
            khachhangID = Session["khachhangID"]?.ToString();
            var khachHang = DB.KHACHHANG.FirstOrDefault(kh => kh.MaKH == khachhangID);
            tenKhachHang = khachHang != null ? khachHang.TenKH : "";
        }
        // GET: Trangchu
        public ActionResult Index(int? page)
        {
            int pagesize = 6;
            int pagenumber = page == null || page < 0 ? 1 : page.Value;
            var listsp = DB.SANPHAM.ToList();
            PagedList<SANPHAM> list = new PagedList<SANPHAM>(listsp, pagenumber, pagesize);
            // Thêm dòng này
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"])
            {
                // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
                return RedirectToAction("Index", "Login");
            }
            getKhachHangID();
            
            ViewBag.CartQuantity = GetCartQuantity(khachhangID);
            ViewBag.tenKhachHang = tenKhachHang;

            return View(list);
        }
        
        public ActionResult ProducsCate(int? page, int? categoryId)
        {
            if (categoryId == null)
            {
                // Nếu categoryId không được cung cấp, chuyển hướng hoặc xử lý theo ý của bạn.
                return RedirectToAction("Index");
            }
            int pagesize = 6;
            int pagenumber = page == null || page < 0 ? 1 : page.Value;

            var listSPDM = DB.SANPHAM.Where(p => p.MaDM == categoryId).ToList();
            PagedList<SANPHAM> list = new PagedList<SANPHAM>(listSPDM, pagenumber, pagesize);

            ViewBag.Page = pagenumber;
            ViewBag.CategoryId = categoryId;
            getKhachHangID();
            ViewBag.CartQuantity = GetCartQuantity(khachhangID);
            ViewBag.tenKhachHang = tenKhachHang;

            // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"])
            {

                return RedirectToAction("Index", "Login");
            }

            // Trả về PartialView chứa danh sách sản phẩm
            return View(list);
        }
        public ActionResult Search(string input_search, int? page, int? categoryId)
        {
            var products = DB.SANPHAM.ToList();

            if (!string.IsNullOrEmpty(input_search))
            {
                // Chuyển chuỗi tìm kiếm về chữ thường để so sánh không phân biệt hoa thường
                input_search = input_search.ToLower();
                products = products.Where(p => p.TenSP.ToLower().Contains(input_search) || p.DonGiaBan.ToString().Contains(input_search)).ToList();
            }


            int pageSize = 6;
            int pageNumber = (page ?? 1);
            ViewBag.CategoryId = categoryId;
            getKhachHangID();
            ViewBag.CartQuantity = GetCartQuantity(khachhangID);
            ViewBag.tenKhachHang = tenKhachHang;

            if (categoryId != null)
            {
                var productsByCategory = products.Where(p => p.MaDM == categoryId).ToList();
                return View("ProducsCate", productsByCategory.ToPagedList(pageNumber, pageSize));
            }

            return View("Index", products.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Detail(int? productId)
        {
            if (productId == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            
            var product = DB.SANPHAM.Where(p => p.MaSP == productId).ToList();
            if (product == null)
            {
                return HttpNotFound(); // Xử lý khi không tìm thấy sản phẩm
            }
            getKhachHangID();
            ViewBag.CartQuantity = GetCartQuantity(khachhangID);
            ViewBag.tenKhachHang = tenKhachHang;

            // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"])
            {

                return RedirectToAction("Index", "Login");
            }
            return View(product);
        }
        public ActionResult cart()
        {
            // Kiểm tra đăng nhập
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"])
            {
                return RedirectToAction("Index", "Login");
            }
            getKhachHangID();
            var cartItems = DB.CART
                 .Where(c => c.MaKH == khachhangID)
                 .Select(c => new CartItem
                 {
                     shoppingsp = c.SANPHAM,
                     Quantity = (int)c.Quantity
                 })
                 .ToList();
            ViewBag.CartQuantity = GetCartQuantity(khachhangID);
            ViewBag.tenKhachHang = tenKhachHang;
            // Truyền dữ liệu đến view
            return View(cartItems);
        }
        public ActionResult AddItem(int productId, int quantity)
        {
            // Kiểm tra đăng nhập
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"])
            {
                return RedirectToAction("Index", "Login");
            }

            getKhachHangID();
            // Kiểm tra sản phẩm đã tồn tại trong giỏ hàng chưa
            var existingCartItem = DB.CART.FirstOrDefault(c => c.MaKH == khachhangID && c.MaSP == productId);

            if (existingCartItem != null)
            {
                // Sản phẩm đã tồn tại, cập nhật số lượng
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // Sản phẩm chưa tồn tại, thêm mới vào giỏ hàng
                var newCartItem = new CART
                {
                    MaKH = khachhangID,
                    MaSP = productId,
                    Quantity = quantity
                };

                DB.CART.Add(newCartItem);
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            DB.SaveChanges();

            return RedirectToAction("cart");
        }


        [HttpPost]
        public ActionResult UpdateCartItem(int[] productIds, int[] quantities)
        {

            // Lấy thông tin khách hàng từ Session
            getKhachHangID();

            // Lấy danh sách sản phẩm trong giỏ hàng từ cơ sở dữ liệu
            var cartItems = DB.CART.Where(c => c.MaKH == khachhangID).ToList();

            if (cartItems != null && cartItems.Count > 0)
            {
                for (int i = 0; i < productIds.Length && i < quantities.Length; i++)
                {
                    var productId = productIds[i];
                    var quantity = quantities[i];

                    var cartItem = cartItems.FirstOrDefault(x => x.MaSP == productId);

                    if (cartItem != null)
                    {
                        // Cập nhật số lượng
                        cartItem.Quantity = quantity;
                    }
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                DB.SaveChanges();

                ViewBag.CartQuantity = GetCartQuantity(khachhangID);
            }

            // Chuyển hướng về trang giỏ hàng
            return RedirectToAction("cart");
        }

        
        private int GetCartQuantity(string khachhangID)
        {
            // Lấy số lượng sản phẩm trong giỏ hàng dựa trên MaKH
            var cartQuantity = DB.CART
                .Where(c => c.MaKH == khachhangID)
                .Sum(c => (int?)c.Quantity) ?? 0;

            return cartQuantity;
        }
        

        [HttpPost]
        public ActionResult RemoveCartItem(int[] selectedProductIds)
        {
            // Lấy thông tin khách hàng từ Session
            getKhachHangID();

            if (selectedProductIds != null && selectedProductIds.Length > 0)
            {
                // Lấy danh sách sản phẩm trong giỏ hàng từ cơ sở dữ liệu
                var cartItems = DB.CART.Where(c => c.MaKH == khachhangID && selectedProductIds.Contains(c.MaSP)).ToList();

                if (cartItems != null && cartItems.Count > 0)
                {
                    // Xóa các sản phẩm khỏi cơ sở dữ liệu
                    foreach (var cartItem in cartItems)
                    {
                        DB.CART.Remove(cartItem);
                    }

                    // Lưu thay đổi vào cơ sở dữ liệu
                    DB.SaveChanges();
                }
            }

            ViewBag.CartQuantity = GetCartQuantity(khachhangID);

            return RedirectToAction("cart");
        }

        
        public ActionResult quanlysanpham(int? page)
        {
            int pagesize = 5;
            int pagenumber = page == null || page < 0 ? 1 : page.Value;
            var ds = DB.SANPHAM.ToList();
            PagedList<SANPHAM> dssp = new PagedList<SANPHAM>(ds, pagenumber, pagesize);

            getKhachHangID();
            ViewBag.CartQuantity = GetCartQuantity(khachhangID);
            ViewBag.tenKhachHang = tenKhachHang;

            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"])
            {
                // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
                return RedirectToAction("Index", "Login");
            }
            return View(dssp);
        }
        [HttpPost]
        public ActionResult Insert(FormCollection form)
        {
            try
            {
                // Lấy dữ liệu từ form
                string tensp = form["tensanpham"];
                int madm = int.Parse(form["danhmuc"]);
                string hinhanh = form["hinhanh"];
                int soluonghiencon = int.Parse(form["soluonghiencon"]);
                short soluongcanduoi = short.Parse(form["soluongcanduoi"]);
                int dongiaban = int.Parse(form["dongiaban"]);
                string mota = form["mota"];


                if (mota == null)
                {
                    mota = "";
                }
                if (IsProductNameExists(tensp))
                {
                    // Tên sản phẩm đã tồn tại, xử lý thông báo hoặc thực hiện các hành động khác
                    ViewBag.ErrorName = "Tên sản phẩm đã tồn tại. Vui lòng chọn tên khác.";
                    return View(); // hoặc chuyển hướng tới trang khác
                }

                // Tạo đối tượng sản phẩm và đặt giá trị
                SANPHAM sanpham = new SANPHAM
                {
                    TenSP = tensp,
                    MaDM = madm,
                    HinhAnh = hinhanh,
                    SoLuongHienCon = soluonghiencon,
                    SoLuongCanDuoi = soluongcanduoi,
                    DonGiaBan = dongiaban,
                    MotaSP = mota
                };

                // Thêm sản phẩm vào cơ sở dữ liệu
                DB.SANPHAM.Add(sanpham);
                DB.SaveChanges();

                // Chuyển hướng hoặc thực hiện các hành động khác
                return RedirectToAction("quanlysanpham", "Trangchu");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                ViewBag.ErrorName = ex.Message;
                return View("quanlysanpham", "Trangchu"); // Trả về view hoặc chuyển hướng tới một trang khác
            }

        }
        private bool IsProductNameExists(string productName)
        {
            // Kiểm tra xem tên sản phẩm đã tồn tại trong cơ sở dữ liệu hay không
            return DB.SANPHAM.Any(p => p.TenSP == productName);
        }

        public ActionResult Delete(int? productId)
        {
            try
            {
                if (productId == null)
                {
                    // If productId is null, return a bad request status
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var product = DB.SANPHAM.Find(productId);
                if (product == null)
                {
                    // If product is not found, return a not found status
                    return HttpNotFound();
                }

                DB.SANPHAM.Remove(product);
                DB.SaveChanges();

                // Trả về kết quả xóa
                return RedirectToAction("quanlysanpham", "Trangchu"); // Hoặc chuyển hướng đến trang khác
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                ViewBag.Error = ex.Message;
                return View();
            }
        }


        [HttpGet]
        public ActionResult GetProductById(int productId)
        {
            try
            {
                // Tìm sản phẩm theo productId trong cơ sở dữ liệu
                var product = DB.SANPHAM.FirstOrDefault(p => p.MaSP == productId);

                if (product != null)
                {
                    // Trả về dữ liệu sản phẩm dưới dạng JSON
                    return Json(new
                    {
                        product.MaSP,
                        product.TenSP,
                        product.MaDM,
                        product.HinhAnh,
                        product.SoLuongHienCon,
                        product.SoLuongCanDuoi,
                        product.DonGiaBan,
                        product.MotaSP
                        // Thêm các trường khác tương tự nếu cần
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Update(FormCollection form)
        {
            try
            {
                // Lấy dữ liệu từ form
                int masp = int.Parse(form["masanphamup"]);
                string tensp = form["tensanphamup"];
                int madm = int.Parse(form["danhmucup"]);
                string hinhanh = form["hinhanhup"];
                int soluonghiencon = int.Parse(form["soluonghienconup"]);
                short soluongcanduoi;
                if (!short.TryParse(form["soluongcanduoiup"], out soluongcanduoi))
                {
                    soluongcanduoi = 5; // Giá trị mặc định nếu không thể chuyển đổi
                }
                int dongiaban = int.Parse(form["dongiabanup"]);
                string mota = form["motaup"];

                // Lấy sản phẩm cần cập nhật từ cơ sở dữ liệu
                var productToUpdate = DB.SANPHAM.SingleOrDefault(p => p.MaSP == masp);

                // Kiểm tra xem sản phẩm có tồn tại không
                if (productToUpdate != null)
                {
                    // Cập nhật thông tin sản phẩm
                    productToUpdate.TenSP = tensp;
                    productToUpdate.MaDM = madm;
                    productToUpdate.HinhAnh = hinhanh; // Đặc biệt với trường hình ảnh
                    productToUpdate.SoLuongHienCon = soluonghiencon;
                    productToUpdate.SoLuongCanDuoi = soluongcanduoi;
                    productToUpdate.DonGiaBan = dongiaban;
                    productToUpdate.MotaSP = mota;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    DB.SaveChanges();
                }

                // Chuyển hướng đến một action hoặc view khác sau khi cập nhật
                return RedirectToAction("quanlysanpham"); // Thay đổi "Index" thành action mong muốn
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Đã xảy ra lỗi khi cập nhật sản phẩm: " + ex.Message;
                return View("quanlysanpham", form);
            }
        }

        public ActionResult SearchQLSP(string input_search, int? page)
        {
            var products = DB.SANPHAM.ToList();

            if (!string.IsNullOrEmpty(input_search))
            {
                // Chuyển chuỗi tìm kiếm về chữ thường để so sánh không phân biệt hoa thường
                input_search = input_search.ToLower();
                // Chuỗi tìm kiếm theo tên hoặc theo giá bán
                products = products.Where(p => p.TenSP.ToLower().Contains(input_search) || p.DonGiaBan.ToString().Contains(input_search)).ToList();
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            getKhachHangID();
            ViewBag.CartQuantity = GetCartQuantity(khachhangID);

            return View("quanlysanpham", products.ToPagedList(pageNumber, pageSize));
        }        
    }
}
