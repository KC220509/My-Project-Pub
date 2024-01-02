--DROP DATABASE QUANLYBANHANGFOOD
CREATE DATABASE QUANLYBANHANGFOOD
GO
use QUANLYBANHANGFOOD
GO
SET DATEFORMAT DMY
--drop table ACCOUNT
CREATE TABLE ACCOUNT
(
    Username VARCHAR(50) CONSTRAINT PK_username PRIMARY KEY,
    Pass VARCHAR(50) NOT NULL CONSTRAINT CK_password CHECK(Pass <> ''),
    SDT varchar(11),
    CONSTRAINT UQ_Account_KhachHang UNIQUE (Username) -- Thêm ràng buộc duy nhất
);
-- Bảng KHACHHANG
CREATE TABLE KHACHHANG
(
    MaKH char(10) CONSTRAINT PK_MaKH PRIMARY KEY,
    TenKH nvarchar(40),
    NgaySinh date CONSTRAINT CK_ngaysinh CHECK (DATEDIFF(YEAR, NgaySinh, GETDATE()) >= 18),
    GioiTinh char(1) CONSTRAINT DF_GioiTinh DEFAULT 'A' CHECK(GioiTinh IN ('A', 'U', 'L')),
    DiaChi nvarchar(100),
    SDT_KH varchar(11),
	EmailKH VARCHAR(100),
    --EmailKH VARCHAR(100) CONSTRAINT UQ_Email_KH UNIQUE CHECK(EmailKH LIKE '%_@%_._%'),
    Username varchar(50) CONSTRAINT FK_username REFERENCES ACCOUNT(Username)
			on update
				cascade
			on delete
				cascade,
	CONSTRAINT UQ_KhachHang_Account UNIQUE (Username)
			
);
GO
--DROP TRIGGER IF EXISTS trg_NewKH;
CREATE TRIGGER trg_NewKH
ON ACCOUNT
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Lấy thông tin tài khoản mới thêm vào
    DECLARE @NewAccounts TABLE (
        Username VARCHAR(50),
        Pass VARCHAR(50),
        SDT varchar(11)
    );

    INSERT INTO @NewAccounts (Username, Pass, SDT)
    SELECT Username, Pass, SDT
    FROM inserted;

    -- Thêm vào bảng KHACHHANG nếu tài khoản chưa tồn tại trong bảng KHACHHANG
    INSERT INTO KHACHHANG (MaKH, TenKH, SDT_KH, Username)
    SELECT 'KH' + RIGHT('00000' + CAST(ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) + 1 AS VARCHAR), 5),
           NA.Username,
           NA.SDT,
           NA.Username
    FROM @NewAccounts AS NA
    WHERE NOT EXISTS (
        SELECT 1
        FROM KHACHHANG K
        WHERE K.Username = NA.Username
    );
END;



GO
--DROP TABLE DONHANG_HOADON
CREATE TABLE DONHANG_HOADON
(
	MaHD int identity(1, 1)
		constraint PK_mahd primary key,
	MaKH char(10) 
		constraint FK_kh_hddh foreign key references KHACHHANG(MaKH)
			on update
				cascade
			on delete
				cascade,
	NgayTaoDH DATETIME DEFAULT CURRENT_TIMESTAMP,
	DiaChiGiaoHang nvarchar(100) not null,
	SDTGiaoHang varchar(10),
	NgayThanhToan datetime not null,
	NgayGiaoHang datetime not null,
	constraint check_GH_TT check(Datediff(year, '0:0', NgayGiaoHang-NgayThanhToan)>=0),
	TrangThaiDonHang nvarchar(30) not null
		constraint CK_TTDH check(TrangThaiDonHang in (N'Chờ xử lý', N'Đang đóng gói', N'Đang vận chuyển', N'Đang giao hàng', N'Đã giao hàng')),
)
--DROP TABLE DANHMUC
GO

CREATE TABLE DANHMUC
(
	MaDM int
		constraint PK_madm primary key,
	TenDM nvarchar(50) not null,
	MOTA nvarchar(200)
)
GO 
--DROP TABLE SANPHAM
CREATE TABLE SANPHAM
(
	MaSP int identity(1, 1)
		constraint PK_masp primary key,
	TenSP nvarchar(100) not null,
	HinhAnh nvarchar(100),
	DonGiaBan numeric(18, 0) not null,
	SoLuongHienCon bigint not null 
		constraint CK_SLhien_con check(SoLuongHienCon >= 0),
	SoLuongCanDuoi smallint
		constraint DF_SLcanduoi default 5,
	MotaSP nvarchar(Max),
	MaDM int
		constraint FK_madm foreign key references DANHMUC(MaDM)
)
GO
--DROP TABLE CHITIETDH_HD
CREATE TABLE CHITIETDH_HD
(
	MaHD int 
		constraint FK_madhhd foreign key references DONHANG_HOADON (MaHD)
			on update
				cascade
			on delete 
				cascade,
	MaSP int
		constraint FK_masp_dh foreign key references SANPHAM (MaSP)
			on update
				cascade
			on delete
				cascade,
	constraint PK_DH_SP primary key(MaHD, MaSP),
	SoLuong int not null
		constraint CK_Soluongdat check(SoLuong >= 0),
	DonGia money
)
GO
CREATE TABLE CART
(
	MaKH char(10), 
    MaSP int, 
    Quantity int, 
    CONSTRAINT PK_Cart PRIMARY KEY (MaKH, MaSP), 
    CONSTRAINT FK_Cart_MaKH FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKH)
        on update cascade 
        on delete cascade, 
    CONSTRAINT FK_Cart_ProductID FOREIGN KEY (MaSP) REFERENCES SANPHAM(MaSP)
        on update cascade 
        on delete cascade 
)
GO
INSERT INTO ACCOUNT(Username, Pass, SDT)
	VALUES	
		('hoangphuc0205', '02052003', '0866545525'),
		('khanhcong09', '22052003', '0971578235'),
		('banha3005', '30052003', '0817133009'),
		('letrinh2610', '26102003', '0942507120'),
		('quangsang0503', '05032003', '0363407804')
--GO
--INSERT INTO KhachHang(MaKH, TenKH, NgaySinh, GioiTinh, DiaChi, SDT_KH, EmailKH, Username)
--	values
--		('05KH000001', N'Bùi Đức Công', '22/05/2003', 'A', N'Hà Tĩnh', '0971578235', 'khanhcong@gmail.com', 'khanhcong09'),
--		('05KH000002', N'Bùi Văn Phúc', '02/05/2003', 'A', N'Quảng Trị', '0866545525','hoangphuc@gmail.com', 'hoangphuc0205'),
--		('05KH000003', N'Trần Lê Trinh', '26/10/2003', 'U', N'Quảng Nam', '0342012377', 'letrinhtran@gmail.com', 'letrinh2610'),
--		('05KH000004', N'Trần Quang Sang', '02/05/2003', 'A', N'Quảng Ngãi', '0389327060', 'quangsangtran@gmail.com', 'quangsang0503'),
--		('05KH000005', N'Trần Bá Nhã', '30/05/2003', 'A', N'Quảng Trị', '0817133009', 'tranbanha@gmail.com', 'banha3005')
GO
INSERT INTO DONHANG_HOADON(MaKH, NgayTaoDH, DiaChiGiaoHang, SDTGiaoHang, NgayThanhToan, NgayGiaoHang, TrangThaiDonHang)
	VALUES	
		('KH00004', '10/07/2022', N'56 Phạm Như Xương, Q.Liên Chiểu, TP.Đà Nẵng', '0931234327', '10/07/2022', '15/07/2022', N'Đã giao hàng'),
		('KH00003', '12/08/2022', N'K82/87 Nguyễn Lương Bằng, Q.Liên Chiểu, TP.Đà Nẵng', '0971578235', '13/08/2022', '13/08/2022', N'Đang giao hàng'),
		('KH00002', '25/12/2022', N'Phú Long, Kỳ Phú, Kỳ Anh, Hà Tĩnh', '0864056815', '25/12/2022', '01/01/2023', N'Đang vận chuyển'),
		('KH00005', '03/04/2023', N'266 Nguyễn Tri Phương, TP.Đà Nẵng', '0376127631', '05/04/2023', '10/04/2023', N'Đang đóng gói'),
		('KH00004', '05/11/2022', N'16 Trần Cao Vân, TP.Đà Nẵng', '0834381436', '06/11/2022', '11/11/2022', N'Chờ xử lý'),
		('KH00005', '15/02/2023', N'120/2 Trưng Nữ Vương, Hải Châu, Đà Nẵng', '0854753596', '15/02/2023', '18/02/2023', N'Đã giao hàng'),
		('KH00004', '22/03/2023', N'34 Hàng Trống, 111 Hàng Gai, Hoàn Kiếm, Hà Nội', '0834381436', '24/03/2023', '27/03/2023', N'Đang giao hàng'),
		('KH00003', '24/04/2023', N'Số 44 ngõ 444, Đội Cấn, Ba Đình, Hà Nội', '0758378295', '25/04/2023', '30/04/2023', N'Đang giao hàng')
GO
INSERT INTO DANHMUC(MaDM, TenDM, MOTA)
 VALUES 
		(1, N'Mới nhất', N'Danh mục chứa các món ăn mới nhất'),
		(2, N'Yêu thích', N'Danh mục chứa các sản phẩm được nhiều người yêu thích'),
		(3, N'Bán chạy', N'Danh mục chứa các sản phẩm bán chạy'),
		(4, N'Khai vị', N'Danh mục chứa các món khai vị'),
		(5, N'Món chay', N'Danh mục chứa các món chay'),
		(6, N'Thực phẩm', N'Danh mục chứa các loại thực phẩm'),
		(7, N'Đồ ăn nhanh', N'Danh mục chứa các đồ ăn nhanh'),
		(8, N'Lẩu', N'Danh mục chứa các món ăn lẩu(về mùa lạnh)'),
		(9, N'Đồ uống', N'Danh mục chứa các loại đồ uống'),
		(10, N'Sữa', N'Danh mục chứa các loại sữa')

--DROP TABLE SANPHAM
--DELETE FROM SANPHAM
GO
INSERT INTO SANPHAM(TenSP, HinhAnh, DonGiaBan, SoLuongHienCon, Soluongcanduoi, MotaSP, MaDM)
	VALUES 
		(N'Chả cá', N'chacathailat.jpg', 300000, 70, default, 'Có thẻ null', 2),
		(N'Bò bít tết', N'bobittet.jpg', 300000, 70, default, N'Bò bít tết (Beefsteak trong tiếng Anh hoặc bifteck trong tiếng Pháp) được chế biến từ thịt thăn bò. Món này thể ăn kèm sốt, hành tây, khoai tây chiên, salad,...', 1),
		(N'Lẩu cá mú', N'laucamu.jpg', 450000, 50, default, N'Món lẩu cá mú thường được người dân chế biến vào dịp cuối thu, đầu đông. Thịt cá mú béo và ngon nhất trong năm do nước biển nhiều thức ăn, giàu dinh dưỡng, nhiều phù sa, tốt cho cá sinh trưởng. Trời mùa đông, khi gió rét về, ngồi xì xụp bên nồi lẩu cá mú, uống rượu bứa ngâm, thưởng thức vị chua của bứa, vị thơm ngậy của cá... quả là một trải nghiệm khó quên.', 8),
		(N'Cháo gà', N'chaoga.jpg', 450000, 50, default, N'Cháo gà là món ăn ngon và cực kỳ dễ ăn. Tôi chắc chắn với các bạn rằng, hầu hết mọi người trên đất nước Việt Nam này đều đã thưởng thức qua món cháo gà và đều ưa thích nó. Bạn đừng quên rằng, cháo gà không chỉ là món ngon buổi sáng, mà dùng vào buổi trưa hay buổi tối đều cực kỳ phù hợp. Chúc các bạn thành công với món cháo gà nhé.', 1),
		(N'Cải ngồng RH', N'caingongRH.png', 15000, 95, default, N'Rau cải ngồng có nguồn gốc ban đầu từ Trung Quốc nên có tên tiếng Anh như trên. Theo chuyên gia, giá trị dinh dưỡng của cải ngồng rất đa dạng, giàu dưỡng chất bổ dưỡng cho cơ thể.', 6),
		(N'Hành khô', N'hanhkhokinhmon.png', 200000, 60, 10, N'Hành lát khô được chế biến từ củ hành trắng hoặc hành tím ta. Củ hành được chọn lọc, sơ chế, thái thành lát mỏng, sấy khô. Do đó, sản phẩm hành khô có mùi thơm đậm đặc, giữ được hương vị lâu, lại giòn giòn, rất dễ sử dụng.', 6),
		(N'Dưa hấu có hạt', N'duahaucohat.png', 350000, 100, 10, N'Dưa hấu Pepino có nguồn gốc từ Nam Mỹ, chỉ bé bằng nắm tay người lớn, hình thon dài, nặng khoảng vài lạng, có mùi thơm nhẹ, vị thanh mát. Điểm đặc biệt của loại quả này là tùy theo từng giai đoạn sẽ mang mùi vị của 10 loại trái cây khác nhau, như vị chuối, mít, ổi, xoài, kiwi, dưa lưới, dưa lê, dưa hấu, măng cụt, lê...', 6),
		(N'Mỳ hộp', N'myhop.jpg', 90000, 140, default, N'Mì ăn liền là một loại thực phẩm tiện lợi và phổ biến, được sản xuất dưới dạng gói hoặc hộp. Thực phẩm này thường bao gồm mì sợi đã được nấu chín trước đó và kèm theo bột gia vị hoặc nước sôi để tạo nên một bữa ăn nhanh chóng và dễ chế biến. Mì ăn liền được ưa chuộng vì tính tiện lợi, đa dạng vị, và có thể được lưu trữ lâu dài.', 7),
		(N'Quýt đường', N'quytduong.jpg', 110000, 54, default, N'Quýt đường là một loại trái cây có hình dáng tròn hoặc hình bầu dục, với mặt vỏ màu cam hoặc vàng. Trái quýt đường có thể biến thiên về kích thước tùy thuộc vào loại và chủng loại. Vỏ của quýt đường có thể dễ bóc hoặc khó bóc tùy thuộc vào sự chín mọng.', 6),
		(N'Thịt bò thăn', N'thitbothan.png', 70000, 99, default, N'Thịt bò thăn là phần cơ của con bò, được chế biến từ cả hai bên đùi sau. Nó có cấu trúc hữu ích cho nấu nướng và nên chứa ít mỡ hơn so với các phần khác của thịt bò.', 6),
		(N'Sữa Milo', N'suamilo.jpg', 130000, 50, default, N'Sữa Milo là sự kết hợp giữa sữa và bột Milo, tạo nên một đồ uống dinh dưỡng với hương vị thơm ngon của sô cô la và malt. Nó thường được xem là một lựa chọn ngon miệng và năng lượng.', 10),
		(N'Nước Coca Cola', N'cocacola.png', 80000, 80, default, N'Coca Cola là một loại đồ uống có ga, có hương vị ngọt ngào và một chút cảm giác cay. Được biết đến trên toàn thế giới, Coca Cola là một trong những đồ uống có hình ảnh mạnh mẽ và thường được sử dụng như một loại nước ngọt.', 9),
		(N'Thịt diềm bò', N'thitdiembo.jpg', 110000, 200, default, N'Thịt diềm bò là một phần từ đùi sau của con bò, có cấu trúc thịt đồng đều với một lớp mỡ bao phủ. Đây là một phần thịt thơm ngon và thích hợp cho nhiều phương pháp nấu ăn, từ nướng đến hấp.', 6),
		(N'Sữa Cacao', N'cacaosuada.jpg', 550000, 140, 10, N'Sữa cacao là đồ uống được làm từ sữa và bột cacao, tạo nên hương vị ngon ngọt và thơm của cacao, thường được thưởng thức làm đồ uống sáng hoặc ăn kèm với bánh.', 10),
		(N'Rau mồng tơi', N'raumongtoi.png', 100000, 95, 10, N'Rau mồng tơi là một loại rau xanh, có lá hình trái tim và thường được sử dụng trong các món xào, luộc hoặc chế biến thành các món ăn khác.', 6),
		(N'Trứng gà quê', N'trungga.jpg', 1500000, 67, 15, N'Trứng gà quê là trứng được sản xuất từ gà sống chăn nuôi tại nông thôn, thường có lòng đỏ màu đậm và hương vị đặc trưng.', 6),
		(N'Tôm chiên giòn', N'tomchiengion.jpg', 110000, 200, default, N'Tôm chiên giòn là món ăn được làm từ tôm được phủ lớp bột chiên giòn, tạo nên vị ngon, giòn rụm và thường được ăn kèm với sốt.', 1),
		(N'Bò nướng lá lốt', N'bonuonglalot.jpg', 550000, 140, 10, N'Bò nướng lá lốt là một món ăn truyền thống, trong đó thịt bò được cuốn trong lá lốt và nướng, tạo nên hương vị độc đáo và thơm ngon.', 1),
		(N'Súp bào ngư', N'supbaongu.png', 100000, 95, 10, N'Súp bào ngư là một món súp chứa ngư, thường được nấu với nước dùng thơm ngon và các thành phần khác như rau củ.', 1),
		(N'Gan ngỗng áp chảo', N'ganngonapchao.png', 1500000, 67, 15, N'Gan ngỗng áp chảo là món ăn chế biến từ gan ngỗng, thường được áp chảo nhanh chóng với các gia vị để giữ lại hương vị tự nhiên của gan.', 1),
		(N'Sa lát cá hồi', N'saladcahoi.png', 200000, 159, 50, N'Sa lát cá hồi là một món sa lát chứa các lớp rau xanh, hỗn hợp cá hồi tươi ngon và sốt phù hợp, tạo nên một bữa ăn nhẹ và giàu chất dinh dưỡng.', 1)
GO
INSERT INTO CART(MaKH,MaSP, Quantity)
VALUES
	('KH00005', 4, 5),
	('KH00002', 5, 6),
	('KH00002', 4, 10)
GO
SELECT
    C.MaKH,
    C.MaSP,
    SP.TenSP,
    SP.DonGiaBan,
    C.Quantity,
    SP.DonGiaBan * C.Quantity AS ThanhTien
FROM
    CART C
JOIN
    SANPHAM SP ON C.MaSP = SP.MaSP
WHERE
    C.MaKH = 'KH00002';  -- Điều kiện để lấy thông tin giỏ hàng của người dùng có mã 'KH001'
 
GO
--Cập nhật sản phẩm
UPDATE SANPHAM
SET MaDM = 8
WHERE MaSP = 4;

GO
--drop table CHITIETDH_HD
INSERT INTO CHITIETDH_HD(MaHD, MaSP, SoLuong, DonGia)
	VALUES 
		(2, 5, 10, 200000),
		(3, 2, 15, 450000),
		(6, 19, 13, 1500000),
		(2, 18, 8, 100000),
		(3, 18, 9, 100000)
--go
--thong ke san pham ban chay
select SP.HinhAnh, SP.TenSP, SP.DonGiaBan,  sum(SoLuong) as TongSoLuong
FROM SANPHAM AS SP, CHITIETDH_HD AS HD
WHERE SP.MaSP=HD.MaSP
group by SP.HinhAnh, SP.TenSP, SP.DonGiaBan
ORDER BY TongSoLuong DESC
--GO

go
CREATE Proc [dbo].[psGetTableSANPHAM] (@MASANPHAM INT)
as
BEGIN TRAN
IF(@MASANPHAM is null)
select * from SANPHAM
ELSE
select * from SANPHAM where MaSP = @MASANPHAM
IF(@@ERROR<>0)
	ROLLBACK TRAN
ELSE
COMMIT TRAN

GO

CREATE PROC [dbo].[psGettableDANHMUC](@MADANHMUC INT)
AS
BEGIN TRAN
IF (@MADANHMUC IS NULL)
SELECT ROW_NUMBER() OVER(ORDER BY TenDM) as autoincrement, MaDM, TenDM, MOTA FROM DANHMUC
ELSE
SELECT * FROM DANHMUC WHERE MaDM = @MADANHMUC
IF (@@ERROR <> 0)
ROLLBACK TRAN
ELSE
COMMIT TRAN

GO

create proc [dbo].[psgetTableLOGIN] (@USERNAME NVARCHAR(30),@PASSWORD NVARCHAR(30))
AS
BEGIN TRAN
SELECT * FROM ACCOUNT WHERE Username = @USERNAME AND Pass = @PASSWORD
IF(@@ERROR <>0)
	ROLLBACK TRAN
ELSE
COMMIT TRAN

GO

create proc [dbo].[psInsertRecordSANPHAM] (@TENSANPHAM NVARCHAR(300),
@DONGIA numeric(18,0),
@SOLUONG INT, @SOLUONGCANDUOI INT, @HINHANH nvarchar(100), @MOTA nvarchar(MAX), @MADANHMUC INT)
AS
BEGIN TRAN
INSERT INTO SANPHAM(TenSP, HinhAnh, DonGiaBan, SoLuongHienCon, SoLuongCanDuoi, MotaSP, MaDM)
VALUES (@TENSANPHAM, @HINHANH, @DONGIA, @SOLUONG, @SOLUONGCANDUOI, @MOTA, @MADANHMUC)
IF(@@ERROR<>0)
	ROLLBACK TRAN
ELSE
COMMIT TRAN
GO

CREATE PROC [dbo].[psUpdateRecordSANPHAM] 
    @MASANPHAM INT, 
    @TENSANPHAM NVARCHAR(300),
    @DONGIA NUMERIC(18, 0),
    @SOLUONG INT, 
    @HINHANH NVARCHAR(100), 
    @MADANHMUC INT
AS
BEGIN
    BEGIN TRAN;

    UPDATE SANPHAM 
    SET 
        TenSP = @TENSANPHAM, 
        DonGiaBan = @DONGIA, 
        SoLuongHienCon = @SOLUONG, 
        HINHANH = @HINHANH, 
        MaDM = @MADANHMUC
    WHERE MaSP = @MASANPHAM;

    COMMIT TRAN;
END;

GO

create proc [dbo].[psDeleteRecordSANPHAM] (@MASANPHAM INT)
AS
BEGIN TRAN
DELETE FROM SANPHAM WHERE MaSP = @MASANPHAM
IF(@@ERROR <> 0)
	ROLLBACK TRAN
ELSE
	COMMIT TRAN



