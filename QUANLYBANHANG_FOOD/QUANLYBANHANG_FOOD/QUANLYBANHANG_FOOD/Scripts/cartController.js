////function updateQuantity() {
////    var productIds = [];
////    var quantities = [];

////    // Lặp qua tất cả các input để lấy thông tin sản phẩm
////    $('.quantity-input').each(function () {
////        var productId = $(this).data('product-id');
////        var quantity = $(this).val();

////        productIds.push(productId);
////        quantities.push(quantity);
////    });

////    // Gửi yêu cầu cập nhật số lượng thông qua Ajax
////    $.ajax({
////        url: '@Url.Action("UpdateCartItem", "Trangchu")',
////        type: 'POST',
////        traditional: true,
////        data: { productIds: productIds, quantities: quantities },
////        success: function () {
////            // Thực hiện các bước cập nhật giao diện nếu cần
////            location.reload(); // Ví dụ: làm mới trang sau khi cập nhật
////        }
////    });
////}