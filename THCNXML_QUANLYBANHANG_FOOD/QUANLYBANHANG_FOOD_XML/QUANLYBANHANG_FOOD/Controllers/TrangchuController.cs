using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;
using PagedList;

namespace QUANLYBANHANG_FOOD.Controllers
{

    public class TrangchuController : Controller
    {
        QUANLYBANHANG_FOOD_XMLEntities DB = new QUANLYBANHANG_FOOD_XMLEntities();

        // GET: Trangchu

        // Đường dẫn đến tệp XML chứa dữ liệu sản phẩm
        string xmlFilePathSP = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "SANPHAMDATA.xml");

        // Đường dẫn đến tệp XML chứa dữ liệu sản phẩm trong giỏ hàng
        string xmlFilePathCart = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "CARTDATA.xml");
        private List<SANPHAM> SANPHAMFromXml(string xmlFilePath)
        {
            XDocument doc = XDocument.Load(xmlFilePath);

            var products = (from p in doc.Descendants("SANPHAM")
                            select new SANPHAM
                            {
                                MaSP = (int)p.Element("MaSP"),
                                TenSP = p.Element("TenSP").Value,
                                HinhAnh = p.Element("HinhAnh").Value,
                                DonGiaBan = (decimal)p.Element("DonGiaBan"),
                                SoLuongHienCon = (long)p.Element("SoLuongHienCon"),
                                SoLuongCanDuoi = (short?)p.Element("SoLuongCanDuoi"),
                                MaDM = (int?)p.Element("MaDM"),
                                MotaSP = p.Element("MotaSP").Value,
                            }).ToList();

            return products;
        }

        public ActionResult Index(int? page)
        {
            DataExporter dataExporter = new DataExporter();
            dataExporter.ExportDataToXml();

            int pageSize = 6;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;


            // Đọc dữ liệu từ tệp XML và chuyển đổi thành danh sách đối tượng sản phẩm
            var products = SANPHAMFromXml(xmlFilePathSP);

            // Tạo PagedList từ danh sách sản phẩm
            PagedList<SANPHAM> productList = new PagedList<SANPHAM>(products, pageNumber, pageSize);

            ViewBag.CartQuantity = GetCartQuantity();

            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"])
            {
                // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
                return RedirectToAction("Index", "Login");
            }

            return View(productList);
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


            // Đọc dữ liệu từ tệp XML và chuyển đổi thành danh sách đối tượng sản phẩm
            var sanphamXML = SANPHAMFromXml(xmlFilePathSP);

            var listSPDM = sanphamXML.Where(p => p.MaDM == categoryId).ToList();
            PagedList<SANPHAM> list = new PagedList<SANPHAM>(listSPDM, pagenumber, pagesize);

            ViewBag.Page = pagenumber;
            ViewBag.CategoryId = categoryId;

            ViewBag.CartQuantity = GetCartQuantity(); // Thêm dòng này

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
            var products = SANPHAMFromXml(xmlFilePathSP);

            if (!string.IsNullOrEmpty(input_search))
            {
                // Chuyển chuỗi tìm kiếm về chữ thường để so sánh không phân biệt hoa thường
                input_search = input_search.ToLower();
                products = products.Where(p => p.TenSP.ToLower().Contains(input_search) || p.DonGiaBan.ToString().Contains(input_search)).ToList();
            }


            int pageSize = 6;
            int pageNumber = (page ?? 1);
            ViewBag.CategoryId = categoryId;
            ViewBag.CartQuantity = GetCartQuantity();

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


            var sanphamXML = SANPHAMFromXml(xmlFilePathSP);

            var product = sanphamXML.Where(p => p.MaSP == productId).ToList();
            if (product == null)
            {
                return HttpNotFound(); // Xử lý khi không tìm thấy sản phẩm
            }

            ViewBag.CartQuantity = GetCartQuantity(); // Thêm dòng này

            // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"])
            {

                return RedirectToAction("Index", "Login");
            }
            return View(product);
        }

        public ActionResult cart()
        {

            // Tải giỏ hàng từ tệp XML
            var cartXML = cartManager.CartFromXml(xmlFilePathCart);

            ViewBag.CartQuantity = GetCartQuantity();

            // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"])
            {
                return RedirectToAction("Index", "Login");
            }

            return View(cartXML);
        }
        public ActionResult AddItem(int productId, int quantity)
        {
            DataExporter dataExporter = new DataExporter();


            // Đọc dữ liệu sản phẩm từ tệp XML
            var sanphamXML = SANPHAMFromXml(xmlFilePathSP);

            // Tìm sản phẩm theo productId
            var product = sanphamXML.FirstOrDefault(c => c.MaSP == productId);


            // Tải giỏ hàng từ tệp XML
            var cart = cartManager.CartFromXml(xmlFilePathCart);

            if (cart.Exists(x => x.shoppingsp.MaSP == productId))
            {
                foreach (var item in cart)
                {
                    if (item.shoppingsp.MaSP == productId)
                    {
                        item.Quantity += quantity;
                    }
                }
            }
            else
            {
                var item = new CartItem
                {
                    shoppingsp = product,
                    Quantity = quantity
                };

                cart.Add(item);
            }

            // Lưu giỏ hàng vào tệp XML sau khi thay đổi
            dataExporter.ExportCartToXml(cart, "CART");

            return RedirectToAction("cart");
        }


        [HttpPost]
        public ActionResult UpdateCartItem(int[] productIds, int[] quantities)
        {
            DataExporter dataExporter = new DataExporter();

            var cart = cartManager.CartFromXml(xmlFilePathCart);

            if (cart != null)
            {
                // Cập nhật số lượng cho từng sản phẩm
                for (int i = 0; i < productIds.Length && i < quantities.Length; i++)
                {
                    var productId = productIds[i];
                    var quantity = quantities[i];

                    var cartItem = cart.FirstOrDefault(x => x.shoppingsp.MaSP == productId);

                    if (cartItem != null)
                    {
                        cartItem.Quantity = quantity;
                    }
                }

                // Lưu giỏ hàng vào tệp XML sau khi cập nhật
                dataExporter.ExportCartToXml(cart, "CART");

                ViewBag.CartQuantity = GetCartQuantity();
            }

            // Chuyển hướng về trang giỏ hàng
            return RedirectToAction("cart");
        }

        private int GetCartQuantity()
        {
            var cart = cartManager.CartFromXml(xmlFilePathCart);

            // Nếu giỏ hàng không null, trả về tổng số lượng sản phẩm, ngược lại trả về 0
            return cart?.Sum(item => item.Quantity) ?? 0;
        }

        [HttpPost]
        public ActionResult removeCartItem(int[] selectedProductIds)
        {
            DataExporter dataExporter = new DataExporter();


            if (selectedProductIds != null)
            {
                //var cart = Session[CartSession];
                var cart = cartManager.CartFromXml(xmlFilePathCart);
                if (cart != null)
                {

                    if (cart != null && cart.Any())
                    {
                        // Loại bỏ các sản phẩm có MaSP nằm trong giỏ hàng
                        cart = cart.Where(item => !selectedProductIds.Contains(item.shoppingsp.MaSP)).ToList();

                        // Lưu lại giỏ hàng sau khi xóa sản phẩm
                        dataExporter.ExportCartToXml(cart, "CART");
                    }
                }
            }

            // Sau khi xóa, có thể làm điều gì đó như redirect hoặc trả về một view
            return RedirectToAction("cart");
        }
        public ActionResult quanlysanpham(int? page)
        {
            int pagesize = 5;
            int pagenumber = page == null || page < 0 ? 1 : page.Value;


            var ds = SANPHAMFromXml(xmlFilePathSP);

            PagedList<SANPHAM> dssp = new PagedList<SANPHAM>(ds, pagenumber, pagesize);

            ViewBag.CartQuantity = GetCartQuantity();

            if (Session["IsAuthenticated"] == null || !(bool)Session["IsAuthenticated"])
            {
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

                // Tạo đối tượng sản phẩm và gán giá trị ỏ form cho đối tượng sản phẩm mới
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

                //Thêm sản phẩm vào dữ liệu SQL
                DB.SANPHAM.Add(sanpham);
                DB.SaveChanges();

                // Thêm sản phẩm vào dữ liệu XML
                DataExporter dataExporter = new DataExporter();
                var sp = SANPHAMFromXml(xmlFilePathSP);
                sp.Add(sanpham);
                dataExporter.ExportSanphamToXml(sp, "SANPHAM");

                return RedirectToAction("quanlysanpham", "Trangchu");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorName = ex.Message;
                return View("quanlysanpham", "Trangchu");
            }
        }

        public ActionResult Delete(int? productId)
        {
            try
            {
                if (productId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var sanphamXML = SANPHAMFromXml(xmlFilePathSP);

                // Tìm sản phẩm trong danh sách sản phẩm
                var product = sanphamXML.FirstOrDefault(p => p.MaSP == productId);

                if (product == null)
                {
                    return HttpNotFound();
                }

                // Xóa sản phẩm từ cơ sở dữ liệu SQL
                var dbProduct = DB.SANPHAM.Find(productId);
                if (dbProduct != null)
                {
                    DB.SANPHAM.Remove(dbProduct);
                    DB.SaveChanges();
                }

                // Xóa sản phẩm từ danh sách trong tệp XML
                DataExporter dataExporter = new DataExporter();
                sanphamXML.Remove(product);

                // Cập nhật lại danh sách sản phẩm trong tệp XML
                dataExporter.ExportSanphamToXml(sanphamXML, "SANPHAM");

                // Sau khi xóa xong quay lại trang quanlysanpham
                return RedirectToAction("quanlysanpham", "Trangchu"); 
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
                var sanphamXML = SANPHAMFromXml(xmlFilePathSP);
                // Tìm sản phẩm theo productId trong dữ liệu XML
                var product = sanphamXML.FirstOrDefault(p => p.MaSP == productId);

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
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
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
                    soluongcanduoi = 5; // Gán giá trị mặc định cho số lượng cần dưới
                }
                int dongiaban = int.Parse(form["dongiabanup"]);
                string mota = form["motaup"];


                // Lấy sản phẩm cần cập nhật từ danh sách sản phẩm ở tệp XML
                var sanphamXML = SANPHAMFromXml(xmlFilePathSP);
                var productToUpdate = sanphamXML.FirstOrDefault(p => p.MaSP == masp);

                // Kiểm tra xem sản phẩm cần cập nhật có tồn tại không
                if (productToUpdate != null)
                {
                    // Cập nhật thông tin sản phẩm
                    productToUpdate.TenSP = tensp;
                    productToUpdate.MaDM = madm;
                    productToUpdate.HinhAnh = hinhanh;
                    productToUpdate.SoLuongHienCon = soluonghiencon;
                    productToUpdate.SoLuongCanDuoi = soluongcanduoi;
                    productToUpdate.DonGiaBan = dongiaban;
                    productToUpdate.MotaSP = mota;


                    // Lưu dữ liệu XML sau khi cập nhật
                    DataExporter dataExporter = new DataExporter();
                    dataExporter.ExportSanphamToXml(sanphamXML, "SANPHAM");
                }

                // Lấy sản phẩm cần cập nhật từ cơ sở dữ liệu SQL
                var productSql = DB.SANPHAM.SingleOrDefault(p => p.MaSP == masp);
                if (productSql != null)
                {
                    // Cập nhật thông tin sản phẩm trong SQL
                    productSql.TenSP = tensp;
                    productSql.MaDM = madm;
                    productSql.HinhAnh = hinhanh;
                    productSql.SoLuongHienCon = soluonghiencon;
                    productSql.SoLuongCanDuoi = soluongcanduoi;
                    productSql.DonGiaBan = dongiaban;
                    productSql.MotaSP = mota;

                    // Lưu dữ liệu SQL sau khi cập nhật
                    DB.SaveChanges();
                }

                return RedirectToAction("quanlysanpham"); 
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Đã xảy ra lỗi khi cập nhật sản phẩm: " + ex.Message;
                return View("quanlysanpham", form);
            }
        }
        public ActionResult SearchQLSP(string input_search, int? page)
        {
            var products = SANPHAMFromXml(xmlFilePathSP);

            if (!string.IsNullOrEmpty(input_search))
            {
                // Chuyển chuỗi tìm kiếm về chữ thường để so sánh không phân biệt hoa thường
                input_search = input_search.ToLower();
                // Chuỗi tìm kiếm theo tên hoặc theo giá bán
                products = products.Where(p => p.TenSP.ToLower().Contains(input_search) || p.DonGiaBan.ToString().Contains(input_search)).ToList();
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            ViewBag.CartQuantity = GetCartQuantity();

            return View("quanlysanpham", products.ToPagedList(pageNumber, pageSize));
        }

        //Chuyển đổi trực tiếp qua dạng xem XML
        public ContentResult ResultXML()
        {
            var sanphamXML = XDocument.Load(xmlFilePathSP);

            // Trả về nội dung XML trực tiếp
            return Content(sanphamXML.ToString(), "application/xml");
        }


        //Xuất file ở dạng XML (file sanpham.xml)
        public FileResult ExportToXml()
        {
            var sanphamXML = XDocument.Load(xmlFilePathSP);

            // Xuất file XML
            byte[] byteArray = Encoding.UTF8.GetBytes(sanphamXML.ToString());
            return File(byteArray, "application/xml", "sanpham.xml");
        }
       

    }
}
