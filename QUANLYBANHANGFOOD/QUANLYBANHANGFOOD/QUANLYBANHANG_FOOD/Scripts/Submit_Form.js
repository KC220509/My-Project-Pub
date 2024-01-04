
function submitLogin() {
    var formLogin = document.getElementById("form-login");
    var username = document.getElementById("username");
    var pass = document.getElementById("pass");
    var isFormValid = true;

    if (username.value.trim() === "") {
        showError("Vui lòng nhập tên tài khoản !", e_user);
        isFormValid = false;
    }
    if (pass.value.trim() === "") {
        showError("Vui lòng nhập mật khẩu !", e_pass);
        isFormValid = false;
    }


    if (!isFormValid) {
        return false; // Ngăn chặn sự kiện submit khi form không hợp lệ
    }
    else {
        formLogin.submit();
    }
    // Nếu form hợp lệ, tiếp tục submit
    return true;
}


function submitSignup() {
    var formSignup = document.getElementById("form-login");
    var username = document.getElementById("username");

    var phone = document.getElementById("phone");
    var pass = document.getElementById("pass");
    var repass = document.getElementById("repass");
    var isFormValid = true;

    if (username.value.trim() === "") {
        showError("Vui lòng nhập tên tài khoản !", e_user);
        isFormValid = false;
    }
    if (phone.value.trim() === "") {
        showError("Vui lòng nhập số điện thoại !", e_phone);
        isFormValid = false;
    }
    if (pass.value.trim() === "") {
        showError("Vui lòng nhập mật khẩu !", e_pass);
        isFormValid = false;
    }
    if (repass.value.trim() === "") {
        showError("Vui lòng nhập lại mật khẩu !", e_repass);
        isFormValid = false;
    }

    if (!isFormValid) {
        return false; // Ngăn chặn sự kiện submit khi form không hợp lệ
    }
    else {
        if (pass.value.trim() !== repass.value.trim()) {
            showError("Mật khẩu nhập lại không trùng khớp !", e_check);
            return false;
        }
        formSignup.submit();
    }
    // Nếu form hợp lệ, tiếp tục submit
    return true;
}
function submitInsert() {
    var formInsert = document.getElementById("form-insert");

    var tensp = document.getElementById("tensp");
    var danhmuc = document.getElementById("danhmuc");
    var hinhanh = document.getElementById("hinhanh");
    var soluonghiencon = document.getElementById("soluonghiencon");
    var dongiaban = document.getElementById("dongiaban");

    var e_tensp = document.getElementById("e_tensp");
    var e_danhmuc = document.getElementById("e_danhmuc");
    var e_hinhanh = document.getElementById("e_hinhanh");
    var e_soluonghc = document.getElementById("e_soluonghc");
    var e_dongiaban = document.getElementById("e_dongiaban");

    var isFormValid = true;

    if (!tensp || tensp.value.trim() === "") {
        showError("Vui lòng nhập tên sản phẩm !", e_tensp);
        isFormValid = false;
    }
    if (!danhmuc || danhmuc.value.trim() === "") {
        showError("Vui lòng chọn danh mục sản phẩm !", e_danhmuc);
        isFormValid = false;
    }
    if (!hinhanh || hinhanh.value.trim() === "") {
        showError("Vui lòng chọn tệp hình ảnh !", e_hinhanh);
        isFormValid = false;
    }
    if (!soluonghiencon || soluonghiencon.value.trim() === "") {
        showError("Vui lòng nhập số lượng sản phẩm !", e_soluonghc);
        isFormValid = false;
    }
    if (!dongiaban || dongiaban.value.trim() === "") {
        showError("Vui lòng nhập đơn giá của sản phẩm !", e_dongiaban);
        isFormValid = false;
    }

    if (!isFormValid) {
        return false; // Ngăn chặn sự kiện submit khi form không hợp lệ
    } else {
            formInsert.submit();
    }

    // Nếu form hợp lệ, tiếp tục submit
    return true;
}

function submitUpdate() {
    var formUpdate = document.getElementById("form-update");

    var tensp = document.getElementById("tensp_up");
    var danhmuc = document.getElementById("danhmuc_up");
    var hinhanh = document.getElementById("hinhanh_up");
    var soluonghiencon = document.getElementById("soluonghiencon_up");
    var dongiaban = document.getElementById("dongiaban_up");

    //var e_tensp = document.getElementById("e_tenspup");
    //var e_danhmuc = document.getElementById("e_danhmucup");
    //var e_hinhanh = document.getElementById("e_hinhanhup");
    //var e_soluonghc = document.getElementById("e_soluonghcup");
    //var e_dongiaban = document.getElementById("e_dongiabanup");

    var isFormValid = true;

    if (!tensp || tensp.value.trim() === "") {
        showError("Vui lòng nhập tên sản phẩm !", e_tenspup);
        isFormValid = false;
    }
    if (!danhmuc || danhmuc.value.trim() === "") {
        showError("Vui lòng chọn danh mục sản phẩm !", e_danhmucup);
        isFormValid = false;
    }
    if (!hinhanh || hinhanh.value.trim() === "") {
        showError("Vui lòng chọn tệp hình ảnh !", e_hinhanhup);
        isFormValid = false;
    }
    if (!soluonghiencon || soluonghiencon.value.trim() === "") {
        showError("Vui lòng nhập số lượng sản phẩm !", e_soluonghcup);
        isFormValid = false;
    }
    if (!dongiaban || dongiaban.value.trim() === "") {
        showError("Vui lòng nhập đơn giá của sản phẩm !", e_dongiabanup);
        isFormValid = false;
    }

    if (!isFormValid) {
        return false; // Ngăn chặn sự kiện submit khi form không hợp lệ
    } else {
        formUpdate.submit();
    }

    // Nếu form hợp lệ, tiếp tục submit
    return true;
}




function showError(errorMessage, id) {
    $(id).text(errorMessage);
    setTimeout(hideErrors, 3000);
}

function hideErrors() {
    $('.rq').text('');
}
