﻿
use master
drop database TrungTamTA
create database TrungTamTA
use TrungTamTA


CREATE TABLE [TaiKhoan] (
  [IDTaiKhoan] INT PRIMARY KEY,
  [TaiKhoan] VARCHAR(50) ,
  [MatKhau] VARCHAR(50) NOT NULL,
);

CREATE TABLE [ChuongTrinhHoc] (
  IDChuongTrinh INT PRIMARY KEY,
  TenChuongTrinh NVARCHAR(50),
  SoBuoiHoc NVARCHAR(50),
  ThoiLuong NVARCHAR(50),
  GiaTien FLOAT,
  MoTa NVARCHAR(50)
);

CREATE TABLE [DangKyHoc] (
  [IDDangKy] INT PRIMARY KEY,
  [HoTen] NVARCHAR(50),
  [NgaySinh] DATETIME,
  [DiaChi] NVARCHAR(50),
  [SoDienThoai] NVARCHAR(50),
  [Email] NVARCHAR(50),
  [IDChuongTrinh] INT,
  [GhiChu] NVARCHAR(MAX),
  [IDTaiKhoan] INT,
   FOREIGN KEY ([IDTaiKhoan]) REFERENCES [TaiKhoan]([IDTaiKhoan]),
   FOREIGN KEY ([IDChuongTrinh]) REFERENCES [ChuongTrinhHoc]([IDChuongTrinh]),
);

CREATE TABLE [TrangThaiHV] (
	IDTrangThai INT PRIMARY KEY,
	TenTrangThai  NVARCHAR(50),
);

CREATE TABLE [GiangVien] (
  IDGiangvien INT PRIMARY KEY,
  TenGiangVien NVARCHAR(50),
  DiaChi  NVARCHAR(MAX),
  SoDienThoai VARCHAR(20),
  Email VARCHAR(50) UNIQUE,
  Hinh VARCHAR(MAX),
  BangCap  NVARCHAR(MAX),
  Luong  FLOAT,
  IDTaiKhoan INT,
   FOREIGN KEY ([IDTaiKhoan]) REFERENCES [TaiKhoan]([IDTaiKhoan]),
);

CREATE TABLE [CacNgayTrongTuan] (
	IDNgay INT PRIMARY KEY,
	TenNgay NVARCHAR(50),
);

CREATE TABLE [LichHoc] (
	IDLichhoc INT PRIMARY KEY,
	TGBatDau NVARCHAR(50),
    TGKetThuc NVARCHAR(50),
	IDNgay INT,
	FOREIGN KEY (IDNgay) REFERENCES [CacNgayTrongTuan](IDNgay),
);

CREATE TABLE [LopHoc] (
  IDLophoc INT PRIMARY KEY,
  TenLopHoc NVARCHAR(50),
  IDGiangVien INT,
  IDChuongTrinh INT,
  SoLuong INT,
  FOREIGN KEY (IDGiangVien) REFERENCES GiangVien(IDGiangvien),
  FOREIGN KEY (IDChuongTrinh) REFERENCES ChuongTrinhHoc(IDChuongTrinh),
);

CREATE TABLE [HocVien] (
  IDHocvien INT PRIMARY KEY,
  TenHocVien NVARCHAR(50),
  NgaySinh DATETIME,
  DiaChi NVARCHAR(50),
  SoDienThoai NVARCHAR(50),
  Email NVARCHAR(50) UNIQUE,
  Hinh NVARCHAR(MAX),
  IDTrangThai INT,
  CapDo NVARCHAR(50),
  IDTaiKhoan INT,
   FOREIGN KEY (IDTrangThai) REFERENCES [TrangThaiHV](IDTrangThai),
   FOREIGN KEY ([IDTaiKhoan]) REFERENCES [TaiKhoan]([IDTaiKhoan])
);
drop table ChiTietLopHoc
CREATE TABLE ChiTietLopHoc (
	IDCTLophoc INT  PRIMARY KEY IDENTITY,
    IDLophoc INT NULL,
    IDHocVien INT NULL,
    DaThanhToan BIT,
    DiemNghe FLOAT,
    DiemNoi FLOAT,
    DiemViet FLOAT,
    DiemDoc FLOAT,
   DiemTB AS CEILING((DiemNghe + DiemNoi + DiemViet + DiemDoc) / 4.0 * 2) / 2.0,
    FOREIGN KEY (IDHocVien) REFERENCES HocVien(IDHocVien),
    FOREIGN KEY (IDLophoc) REFERENCES LopHoc(IDLophoc)
);


CREATE TABLE [ChiTietLichHoc] (
	IDLophoc INT,
	IDLichhoc INT,
	 PRIMARY KEY (IDLophoc, IDLichhoc),
	FOREIGN KEY (IDLichhoc) REFERENCES [LichHoc](IDLichhoc),
    FOREIGN KEY (IDLophoc) REFERENCES LopHoc(IDLophoc),
);

----chưa làm  -----
CREATE TABLE [PhuongThucThanhToan](
	[IdThanhToan] INT PRIMARY KEY,
	[TenPhuongThucThanhToan] NVARCHAR(50),
);
CREATE TABLE[HoaDonTT](
	[IdHoaDon] INT PRIMARY KEY,
	[NgayNopTien] DATETIME ,
	[SoTien] INT,
	[IdThanhToan] INT,
	[IDHocVien] INT,
	[IdLopHoc] INT,
	 FOREIGN KEY (IDHocVien) REFERENCES HocVien(IDHocVien),
    FOREIGN KEY (IDLophoc) REFERENCES LopHoc(IDLophoc)
);


----------THÊM DỮ LIỆU------------------
----------------------------------THÊM DỮ LIỆU--------------------------------
------------THÊM TRẠNG THÁI HỌC VIÊN----------------
INSERT INTO [TrangThaiHV]
VALUES (1, N'Đang học');
INSERT INTO [TrangThaiHV]
VALUES (2, N'Bỏ học');
INSERT INTO [TrangThaiHV]
VALUES (3, N'Nợ học phí');
INSERT INTO [TrangThaiHV]
VALUES (4, N'Bảo lưu');
--UPDATE [TrangThaiHV] SET TenTrangThai = N'Đang học' WHERE IDTrangThai=1 ;
----------------------------------------------------
------------THÊM THỨ---------------
INSERT INTO [CacNgayTrongTuan]
VALUES (1, N'Thứ 2');
INSERT INTO [CacNgayTrongTuan]
VALUES (2, N'Thứ 3');
INSERT INTO [CacNgayTrongTuan]
VALUES (3, N'Thứ 4');
INSERT INTO [CacNgayTrongTuan]
VALUES (4, N'Thứ 5');
INSERT INTO [CacNgayTrongTuan]
VALUES (5, N'Thứ 6');
INSERT INTO [CacNgayTrongTuan]
VALUES (6, N'Thứ 7');
INSERT INTO [CacNgayTrongTuan]
VALUES (7, N'Chủ Nhật');
--------------------------------------------------------
------------THÊM GIẢNG VIÊN----------------
INSERT INTO [GiangVien]
VALUES (112, N'Kiên',N'Châu thành','0976391970',N'kt78139@gmail.com',null,null,100000,null);
---------------------------------------------
------------THÊM HỌC VIÊN----------------
INSERT INTO [HocVien]
VALUES (1112, N'Kiên Trần',16/10/2002,N'Châu thành Tây Ninh','0976391970',N'kt78139@gmail.com',null,1,'2.5 toeic',null);
INSERT INTO [HocVien]
VALUES (11112, N'Kiên Trần1',16/10/2002,N'Châu thành Tây Ninh1','0976391970',N'kt781390@gmail.com',null,1,'2.5 toeic',null);
------------THÊM CHƯƠNG TRÌNH HỌC----------------
INSERT INTO [ChuongTrinhHoc]
VALUES (1, N'Toeic 2.5',N'2 Tuần',N'3 Tháng',10000,N'Dành cho các bạn TOEIC 2.5');
-------------------------------------------------
------------THÊM LỊCH HỌC----------------
INSERT INTO [LichHoc]
VALUES (1, N'07:00',N'09:00',1);
--------------------------------------
------------THÊM LỚP HỌC----------------
INSERT INTO [LopHoc]
VALUES (111, N'TOEIC 2.5',112,1,30);
------------CHI TIẾT LỚP HỌC----------------
INSERT INTO [ChiTietLopHoc]
VALUES (111,1112,0,6.5,6.5,5.0,7.5);

----------------------------------
------------THÊM CHI TIẾT LỊCH HỌC----------------
INSERT INTO [ChiTietLichHoc]
VALUES (111,1);
----------------------------------
-----------------------CHỌN BẢNG-------------------------
select * from [TrangThaiHV]
select * from [CacNgayTrongTuan]

	select * from [HocVien]
	select * from [GiangVien]
	select * from [ChuongTrinhHoc]
	select * from [LopHoc]
	select * from [LichHoc]
	select * from[ChiTietLopHoc]
	select * from [ChiTietLichHoc]
