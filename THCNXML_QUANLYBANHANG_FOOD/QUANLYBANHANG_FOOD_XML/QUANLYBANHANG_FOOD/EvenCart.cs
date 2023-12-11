using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace QUANLYBANHANG_FOOD
{
    [Serializable]
    public class CartItem
    {
        public SANPHAM shoppingsp { get; set; }
        public int Quantity { get; set; }
    }
    public class cartManager
    {
        public static List<CartItem> CartFromXml(string xmlFilePath)
        {
            XDocument doc = XDocument.Load(xmlFilePath);

            var cartItems = (from item in doc.Descendants("CartItem")
                             select new CartItem
                             {
                                 shoppingsp = new SANPHAM
                                 {
                                     MaSP = (int)item.Element("MaSP"),
                                     TenSP = item.Element("TenSP").Value,
                                     HinhAnh = item.Element("HinhAnh").Value,
                                     DonGiaBan = (decimal)item.Element("DonGiaBan"),
                                     SoLuongHienCon = (long)item.Element("SoLuongHienCon"),
                                     SoLuongCanDuoi = (short?)item.Element("SoLuongCanDuoi"),
                                     MaDM = (int?)item.Element("MaDM"),
                                     MotaSP = item.Element("MotaSP").Value,
                                 },
                                 Quantity = (int)item.Element("Quantity")
                             }).ToList();

            return cartItems;
        }
    }
}