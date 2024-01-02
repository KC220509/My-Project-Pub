using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QUANLYBANHANG_FOOD
{
    [Serializable]
    public class CartItem
    {
        public SANPHAM shoppingsp { get; set; }
        public int Quantity { get; set; }
    }

}