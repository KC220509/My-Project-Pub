using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QUANLYBANHANG_FOOD
{
    public class SANPHAM_XML
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string HinhAnh { get; set; }
        public decimal DonGiaBan { get; set; }
        public long SoLuongHienCon { get; set; }
        public Nullable<short> SoLuongCanDuoi { get; set; }
        public Nullable<int> MaDM { get; set; }
        public string MotaSP { get; set; }

        public static SANPHAM_XML ConvertFromSANPHAM(SANPHAM sanpham)
        {
            return new SANPHAM_XML
            {
                MaSP = sanpham.MaSP.ToString(),
                TenSP = sanpham.TenSP,
                HinhAnh = sanpham.HinhAnh,
                DonGiaBan = sanpham.DonGiaBan,
                SoLuongHienCon = sanpham.SoLuongHienCon,
                SoLuongCanDuoi = sanpham.SoLuongCanDuoi,
                MaDM = sanpham.MaDM,
                MotaSP = sanpham.MotaSP
            };
        }
    }
}