﻿using TrungTamTA.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace QLtrungtam.Controllers
{
    public class QLHocvienController : Controller
    {
        dbQLtrungtamDataContext data = new dbQLtrungtamDataContext();
        // GET: QLHocvien
        #region Show học viên

        public ActionResult Hocvien()
        {
            var hocvien = from hv in data.HocViens select hv;
            return View(hocvien);
        }

        #endregion
        #region Thêm xóa sửa
        private bool IsValidEmail(string email)
        {
            // Biểu thức chính quy kiểm tra định dạng email
            string pattern = @"^[a-zA-Z0-9.-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Kiểm tra xem email có trùng khớp với biểu thức chính quy hay không
            return Regex.IsMatch(email, pattern);
        }
      public bool CheckIsNumber(string input)
        {
            return double.TryParse(input, out _);
        }
        private bool checkid(string id)
        {
            if (CheckIsNumber(id) && id.Length <= 8)
            {
                return data.HocViens.Count(x => x.IDHocvien == int.Parse(id)) > 0;
            }
            else {
                return false;            
            }
        }

        private bool checkemail(string email)
        {
            return data.GiangViens.Any(x => x.Email == email);
        }
  

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }

        [HttpGet]
        public ActionResult Themhocvien()
        {
            ViewBag.IDTrangThai = new SelectList(data.TrangThaiHVs.ToList().OrderBy(n => n.TenTrangThai), "IDTrangThai", "TenTrangThai");
            return View();
        }
        [HttpPost]
        public ActionResult Themhocvien(FormCollection collection, HocVien hocvien, int IDtrangthai)
        {
            var idhocvien = collection["IDHocvien"];
            var tenhocvien = collection["Tenhocvien"];            
            var diachi = collection["DiaChi"];
            var sodt = collection["Phone"];
            var email = collection["Email"];

            if (string.IsNullOrEmpty(idhocvien))
            {
                ViewData["Loi1"] = "Vui lòng nhập mã học viên";
            }
            else  if (checkid(idhocvien) == false)
            {
                ViewData["Loi1"] = "Vui lòng nhập mã học viên là số";
            }
            else if (!checkid(idhocvien))
            {
                ViewData["Loi1"] = "Mã của học viên không quá 8 số";
            }
            else if (checkid(idhocvien))
            {
                ViewData["Loi1"] = "Mã học viên đã tồn tại";
            }
            else if (string.IsNullOrEmpty(tenhocvien))
            {
                ViewData["Loi2"] = "Vui lòng nhập tên học viên";
            }
           
            else if (string.IsNullOrEmpty(Request.Form["NgaySinh"]))
            {
                ViewData["Loi3"] = "Vui lòng nhập ngày sinh";
            }
            else if (!DateTime.TryParseExact(Request.Form["NgaySinh"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                ViewData["Loi3"] = "Ngày sinh không hợp lệ";
            }
            else if (string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi4"] = "Vui lòng nhập địa chỉ";
            }
            else if (string.IsNullOrEmpty(sodt))
            {
                ViewData["Loi5"] = "Vui lòng nhập số điện thoại";
            }
            else if (string.IsNullOrEmpty(email))
            {
                ViewData["Loi6"] = "Vui lòng nhập email";
            }
            else if (checkemail(email))
            {
                ViewData["Loi6"] = "Email đã có";
            }
            else if (!IsValidEmail(email))
            {
                ViewData["Loi6"] = "Email không hợp lệ";
            }
            else
            { 
                hocvien.IDHocvien = int.Parse(idhocvien);
                hocvien.TenHocVien = tenhocvien;
                // Chuyển đổi chuỗi thành kiểu DateTime với định dạng "dd/MM/yyyy"
                hocvien.NgaySinh = DateTime.ParseExact(Request.Form["NgaySinh"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                hocvien.DiaChi = diachi;
                hocvien.SoDienThoai = sodt;
                hocvien.Email = email;
                hocvien.Hinh = collection["Hinh"];
                hocvien.IDTrangThai = IDtrangthai;
                data.HocViens.InsertOnSubmit(hocvien);
                data.SubmitChanges();
                return RedirectToAction("Hocvien");
            }
            return this.Themhocvien();
        }

        public ActionResult Xoahocvien(int IDhv)
        {
            var hocvien = data.HocViens.FirstOrDefault(c => c.IDHocvien == IDhv);
            if (hocvien == null)
            {
                return HttpNotFound();
            }

            var chiTietLopHocs = data.ChiTietLopHocs.Where(c => c.IDHocVien == hocvien.IDHocvien);
            data.ChiTietLopHocs.DeleteAllOnSubmit(chiTietLopHocs);
            data.SubmitChanges(); // Lưu thay đổi sau khi xóa các bản ghi liên quan

            data.HocViens.DeleteOnSubmit(hocvien);
            data.SubmitChanges(); // Lưu thay đổi sau khi xóa học viên

            TempData["SuccessMessage"] = "Đã xóa!";
            return RedirectToAction("Hocvien", "QLHocvien");
        }


        public ActionResult Xemchitiet(int IDhv)
        {
            ViewBag.IDhv = IDhv;
            var show1hocvien = data.HocViens.FirstOrDefault(s => s.IDHocvien == IDhv);
            if (show1hocvien == null)
            {
                return HttpNotFound();
            }
            return View(show1hocvien);

        }


        [HttpGet]
        public ActionResult Suahocvien(int IDhv)
        {
            var trangThaiList = data.TrangThaiHVs.ToList().OrderBy(n => n.TenTrangThai);
            ViewBag.IDTrangThai = new SelectList(trangThaiList, "IDTrangThai", "TenTrangThai");

            HocVien hocvien = data.HocViens.SingleOrDefault(c => c.IDHocvien == IDhv);
            if (hocvien == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(hocvien);
            }
        }

        [HttpPost]
        public ActionResult Suahocvien(HocVien editHocvien)
        {
            if (ModelState.IsValid)
            {
                HocVien hocvien = data.HocViens.SingleOrDefault(c => c.IDHocvien == editHocvien.IDHocvien);
                if (hocvien != null)
                {
                    hocvien.TenHocVien = editHocvien.TenHocVien;
                    hocvien.NgaySinh = editHocvien.NgaySinh;
                    hocvien.DiaChi = editHocvien.DiaChi;
                    hocvien.SoDienThoai = editHocvien.SoDienThoai;
                    hocvien.Email = editHocvien.Email;
                    hocvien.Hinh = editHocvien.Hinh;
                    hocvien.CapDo = editHocvien.CapDo;
                    hocvien.IDTrangThai = editHocvien.IDTrangThai;

                    data.SubmitChanges();
                    return RedirectToAction("Hocvien");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                var trangThaiList = data.TrangThaiHVs.ToList().OrderBy(n => n.TenTrangThai);
                ViewBag.IDTrangThai = new SelectList(trangThaiList, "IDTrangThai", "TenTrangThai", editHocvien.IDTrangThai);
                return View(editHocvien);
            }
        }
        public ActionResult StudentTimetable(int IDhv)
        {
            ViewBag.IDhv = IDhv;
            var nameHv = data.HocViens.FirstOrDefault(x => x.IDHocvien == IDhv).TenHocVien.ToString();
            ViewBag.nameHv = nameHv;
            //danh sách rỗng của ChiTietLichHoc được khởi tạo để lưu trữ thông tin về lịch học của sinh viên.
            List<ChiTietLichHoc> listChiTietLichHoc = new List<ChiTietLichHoc>();
            // Danh sách ChiTietLopHoc của sinh viên được lấy từ cơ sở dữ liệu có IDHocVien bằng với IDhocvien được truyền vào.
            List<ChiTietLopHoc> listChiTietLopHoc = data.ChiTietLopHocs.Where(item => item.IDHocVien == IDhv).ToList();
            //lặp qua từng đối tượng ChiTietLopHoc trong danh sách.
            foreach (ChiTietLopHoc item in listChiTietLopHoc)
            {
                //LopHoc tương ứng với IDLophoc của đối tượng ChiTietLopHoc được lấy từ cơ sở dữ liệu với điều kiện là IDLophoc 
                LopHoc lopHoc = data.LopHocs.SingleOrDefault(x => x.IDLophoc == item.IDLophoc);
                if (lopHoc != null)
                {
                    //ChitietLichHoc tương ứng với LopHoc đó được lấy từ cơ sở dữ liệu IDLophoc
                    List<ChiTietLichHoc> listChiTietLichHocForLop = data.ChiTietLichHocs.Where(x => x.IDLophoc == lopHoc.IDLophoc).ToList();
                    foreach (ChiTietLichHoc chiTietLichHoc in listChiTietLichHocForLop)
                    {
                        listChiTietLichHoc.Add(chiTietLichHoc);
                    }
                }
            }

            return View(listChiTietLichHoc);
        }

            #endregion
        }
}
