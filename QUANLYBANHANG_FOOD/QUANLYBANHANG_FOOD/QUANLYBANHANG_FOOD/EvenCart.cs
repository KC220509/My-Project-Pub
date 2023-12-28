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

    //public class EvenCart
    //{
    //    List<CartItem> items = new List<CartItem>();
    //    //public IEnumerable<CartItem> Items
    //    //{
    //    //    get
    //    //    {
    //    //        return items;
    //    //    }
    //    //}
    //    public void RemoveCart(int productId)
    //    {
    //        // Tìm sản phẩm trong danh sách và xóa nó
    //        var itemToRemove = items.FirstOrDefault(item => item.shoppingsp.MaSP == productId);

    //        if (itemToRemove != null)
    //        {
    //            items.Remove(itemToRemove);
    //        }
    //    }
    //}
}