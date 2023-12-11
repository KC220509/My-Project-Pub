using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace QUANLYBANHANG_FOOD
{
    public class DataExporter
    {
        SqlConnection conn = new SqlConnection("Data Source=LAPTOP-M3U7O1K3\\USER_KC;Initial Catalog=QUANLYBANHANG_FOOD_XML;Persist Security Info=True;User ID=User_KhanhCong;Password=220305");
        public void ExportDataToXml()
        {
            ExportDataToXml("select * from ACOUNT", "ACOUNTDATA");
            ExportDataToXml("select * from KHACHHANG", "KHACHHANG");
            ExportDataToXml("select * from DANHMUC", "DANHMUC");
            ExportDataToXml("select * from CHITIETDH_HD", "CHITIETDH_HD");
            ExportDataToXml("select * from DONHANG_HOADON", "DONHANG_HOADON");
            ExportDataToXml("select * from SANPHAM", "SANPHAM");
            
        }
        public void ExportDataToXml(String query, String XMLName)
        {
            try
            {

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand(query, conn);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                DataSet ds = new DataSet();
                da.Fill(ds, XMLName);
                ds.DataSetName = XMLName + "DATA";
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", XMLName + "DATA.xml");

                ds.WriteXml(filePath);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void ExportSanphamToXml(List<SANPHAM> sanpham, string XMLName)
        {
            try
            {
                var sanphamXml = new XElement("SANPHAMDATA",
                sanpham.Select(item => new XElement("SANPHAM",
                    new XElement("MaSP", item.MaSP),
                    new XElement("TenSP", item.TenSP),
                    new XElement("HinhAnh", item.HinhAnh),
                    new XElement("DonGiaBan", item.DonGiaBan),
                    new XElement("SoLuongHienCon", item.SoLuongHienCon),
                    new XElement("SoLuongCanDuoi", item.SoLuongCanDuoi),
                    new XElement("MaDM", item.MaDM),
                    new XElement("MotaSP", item.MotaSP)
                ))
            );

                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", XMLName + "DATA.xml");
                sanphamXml.Save(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void ExportCartToXml(List<CartItem> cart, string XMLName)
        {
            try
            {
                var cartXml = new XElement("Cart",
                cart.Select(item => new XElement("CartItem",
                    new XElement("MaSP", item.shoppingsp.MaSP),
                    new XElement("TenSP", item.shoppingsp.TenSP),
                    new XElement("HinhAnh", item.shoppingsp.HinhAnh),
                    new XElement("DonGiaBan", item.shoppingsp.DonGiaBan),
                    new XElement("SoLuongHienCon", item.shoppingsp.SoLuongHienCon),
                    new XElement("SoLuongCanDuoi", item.shoppingsp.SoLuongCanDuoi),
                    new XElement("MaDM", item.shoppingsp.MaDM),
                    new XElement("MotaSP", item.shoppingsp.MotaSP),
                    new XElement("Quantity", item.Quantity)
                ))
            );

                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", XMLName + "DATA.xml");
                cartXml.Save(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
