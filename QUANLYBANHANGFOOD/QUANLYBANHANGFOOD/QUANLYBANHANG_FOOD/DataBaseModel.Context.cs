﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QUANLYBANHANG_FOOD
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class QUANLYBANHANG_FOODEntities1 : DbContext
    {
        public QUANLYBANHANG_FOODEntities1()
            : base("name=QUANLYBANHANGFOODEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ACCOUNT> ACCOUNT { get; set; }
        public virtual DbSet<CHITIETDH_HD> CHITIETDH_HD { get; set; }
        public virtual DbSet<DANHMUC> DANHMUC { get; set; }
        public virtual DbSet<DONHANG_HOADON> DONHANG_HOADON { get; set; }
        public virtual DbSet<KHACHHANG> KHACHHANG { get; set; }
        public virtual DbSet<SANPHAM> SANPHAM { get; set; }
        public virtual DbSet<CART> CART { get; set; }
    
        public virtual int psDeleteRecordSANPHAM(Nullable<int> mASANPHAM)
        {
            var mASANPHAMParameter = mASANPHAM.HasValue ?
                new ObjectParameter("MASANPHAM", mASANPHAM) :
                new ObjectParameter("MASANPHAM", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("psDeleteRecordSANPHAM", mASANPHAMParameter);
        }
    
        public virtual ObjectResult<psGettableDANHMUC_Result> psGettableDANHMUC(Nullable<int> mADANHMUC)
        {
            var mADANHMUCParameter = mADANHMUC.HasValue ?
                new ObjectParameter("MADANHMUC", mADANHMUC) :
                new ObjectParameter("MADANHMUC", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<psGettableDANHMUC_Result>("psGettableDANHMUC", mADANHMUCParameter);
        }
    
        public virtual ObjectResult<psgetTableLOGIN_Result> psgetTableLOGIN(string uSERNAME, string pASSWORD)
        {
            var uSERNAMEParameter = uSERNAME != null ?
                new ObjectParameter("USERNAME", uSERNAME) :
                new ObjectParameter("USERNAME", typeof(string));
    
            var pASSWORDParameter = pASSWORD != null ?
                new ObjectParameter("PASSWORD", pASSWORD) :
                new ObjectParameter("PASSWORD", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<psgetTableLOGIN_Result>("psgetTableLOGIN", uSERNAMEParameter, pASSWORDParameter);
        }
    
        public virtual ObjectResult<psGetTableSANPHAM_Result> psGetTableSANPHAM(Nullable<int> mASANPHAM)
        {
            var mASANPHAMParameter = mASANPHAM.HasValue ?
                new ObjectParameter("MASANPHAM", mASANPHAM) :
                new ObjectParameter("MASANPHAM", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<psGetTableSANPHAM_Result>("psGetTableSANPHAM", mASANPHAMParameter);
        }
    
        public virtual int psInsertRecordSANPHAM(string tENSANPHAM, Nullable<decimal> dONGIA, Nullable<int> sOLUONG, Nullable<int> sOLUONGCANDUOI, string hINHANH, string mOTA, Nullable<int> mADANHMUC)
        {
            var tENSANPHAMParameter = tENSANPHAM != null ?
                new ObjectParameter("TENSANPHAM", tENSANPHAM) :
                new ObjectParameter("TENSANPHAM", typeof(string));
    
            var dONGIAParameter = dONGIA.HasValue ?
                new ObjectParameter("DONGIA", dONGIA) :
                new ObjectParameter("DONGIA", typeof(decimal));
    
            var sOLUONGParameter = sOLUONG.HasValue ?
                new ObjectParameter("SOLUONG", sOLUONG) :
                new ObjectParameter("SOLUONG", typeof(int));
    
            var sOLUONGCANDUOIParameter = sOLUONGCANDUOI.HasValue ?
                new ObjectParameter("SOLUONGCANDUOI", sOLUONGCANDUOI) :
                new ObjectParameter("SOLUONGCANDUOI", typeof(int));
    
            var hINHANHParameter = hINHANH != null ?
                new ObjectParameter("HINHANH", hINHANH) :
                new ObjectParameter("HINHANH", typeof(string));
    
            var mOTAParameter = mOTA != null ?
                new ObjectParameter("MOTA", mOTA) :
                new ObjectParameter("MOTA", typeof(string));
    
            var mADANHMUCParameter = mADANHMUC.HasValue ?
                new ObjectParameter("MADANHMUC", mADANHMUC) :
                new ObjectParameter("MADANHMUC", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("psInsertRecordSANPHAM", tENSANPHAMParameter, dONGIAParameter, sOLUONGParameter, sOLUONGCANDUOIParameter, hINHANHParameter, mOTAParameter, mADANHMUCParameter);
        }
    
        public virtual int psUpdateRecordSANPHAM(Nullable<int> mASANPHAM, string tENSANPHAM, Nullable<decimal> dONGIA, Nullable<int> sOLUONG, string hINHANH, Nullable<int> mADANHMUC)
        {
            var mASANPHAMParameter = mASANPHAM.HasValue ?
                new ObjectParameter("MASANPHAM", mASANPHAM) :
                new ObjectParameter("MASANPHAM", typeof(int));
    
            var tENSANPHAMParameter = tENSANPHAM != null ?
                new ObjectParameter("TENSANPHAM", tENSANPHAM) :
                new ObjectParameter("TENSANPHAM", typeof(string));
    
            var dONGIAParameter = dONGIA.HasValue ?
                new ObjectParameter("DONGIA", dONGIA) :
                new ObjectParameter("DONGIA", typeof(decimal));
    
            var sOLUONGParameter = sOLUONG.HasValue ?
                new ObjectParameter("SOLUONG", sOLUONG) :
                new ObjectParameter("SOLUONG", typeof(int));
    
            var hINHANHParameter = hINHANH != null ?
                new ObjectParameter("HINHANH", hINHANH) :
                new ObjectParameter("HINHANH", typeof(string));
    
            var mADANHMUCParameter = mADANHMUC.HasValue ?
                new ObjectParameter("MADANHMUC", mADANHMUC) :
                new ObjectParameter("MADANHMUC", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("psUpdateRecordSANPHAM", mASANPHAMParameter, tENSANPHAMParameter, dONGIAParameter, sOLUONGParameter, hINHANHParameter, mADANHMUCParameter);
        }
    }
}
