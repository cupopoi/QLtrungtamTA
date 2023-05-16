﻿using TrungTamTA.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace QLtrungtam.Controllers
{
    public class QLGiangvienController : Controller
    {
        dbQLtrungtamDataContext data = new dbQLtrungtamDataContext();
        // GET: QLHocvien
        #region Show Giảng viên

        public ActionResult Giangvien(int? idaction, int? idlh)
        {
            ViewBag.idaction = idaction;
            ViewBag.IDlh = idlh;
            var giangvien = from gv in data.GiangViens select gv;
            return View(giangvien);
        }
        #endregion
        #region Thêm xóa sửa
        private bool checkid(int id)
        {
            return data.GiangViens.Count(x => x.IDGiangvien == id) > 0;
        }
        private bool checkemail(string email)
        {
            return data.GiangViens.Any(x => x.Email == email);
        }
        private bool IsValidEmail(string email)
        {
            // Biểu thức chính quy kiểm tra định dạng email
            string pattern = @"^[a-zA-Z0-9.-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Kiểm tra xem email có trùng khớp với biểu thức chính quy hay không
            return Regex.IsMatch(email, pattern);
        }

        //được sử dụng để upload hình ảnh
        public string ProcessUpload(HttpPostedFileBase file)
        {
            //kiểm tra file nếu không có tệp tin nào được tải lên, và phương thức trả về một chuỗi rỗng ""
            if (file == null)
            {
                return "";
            }
            //nếu không thì sẽ lưu vào đường dẫn và thực hiện phương thức SaveAs ( với đường dẫn là "~/Content/images/" + file.FileName)
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
           //Sau khi tệp tin được lưu thành công
           // trả về đường dẫn của tệp tin hình ảnh đã lưu, bắt đầu bằng "/Content/images/" + tên tệp tin file.FileName.
            return "/Content/images/" + file.FileName;
        }

        [HttpGet]
        public ActionResult Themgiangvien()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Themgiangvien(FormCollection collection, GiangVien giangvien)
        {


            var idgiangvien = collection["IDGiangvien"];
            var tengiangvien = collection["Tengiangvien"];
            var diachi = collection["DiaChi"];
            var sodt = collection["Phone"];
            var email = collection["Email"];
            var luong = collection["Luong"];
            if (checkid(int.Parse(idgiangvien)))
            {
                ViewData["Loi1"] = "Mã giảng viên đã tồn tại";
            }
            else if (string.IsNullOrEmpty(idgiangvien))
            {
                ViewData["Loi1"] = "Vui lòng nhập mã giảng viên";
            }
            else if (string.IsNullOrEmpty(tengiangvien))
            {
                ViewData["Loi2"] = "Vui lòng nhập tên giảng viên";
            }
            else if (string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi3"] = "Vui lòng nhập địa chỉ";
            }
            else if (string.IsNullOrEmpty(sodt))
            {
                ViewData["Loi4"] = "Vui lòng nhập số điện thoại";
            }
            else if (string.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Vui lòng nhập email";
            }
            else if (checkemail(email))
            {
                ViewData["Loi5"] = "Email đã có";
            }
            else if (!IsValidEmail(email))
            {
                ViewData["Loi5"] = "Email không hợp lệ";
            }
            else if (string.IsNullOrEmpty(luong))
            {
                ViewData["Loi6"] = "Vui lòng nhập lương";
            }
            else
            {
                giangvien.IDGiangvien = int.Parse(idgiangvien);
                giangvien.TenGiangVien = tengiangvien;
                giangvien.DiaChi = diachi;
                giangvien.SoDienThoai = sodt;
                giangvien.Email = email;
                giangvien.Hinh = collection["Hinh"];
                giangvien.BangCap = collection["BangCap"];
                giangvien.Luong = int.Parse(luong);
                data.GiangViens.InsertOnSubmit(giangvien);
                data.SubmitChanges();
                return RedirectToAction("Giangvien");
            }
            return this.Themgiangvien();
        }
        public ActionResult Xoagiangvien(int IDgv)
        {
            var giangvien = data.GiangViens.FirstOrDefault(c => c.IDGiangvien == IDgv);
            if (giangvien == null)
            {
                return HttpNotFound();
            }
            var lopHoc = data.LopHocs.FirstOrDefault(l => l.IDGiangVien == giangvien.IDGiangvien);
            if (lopHoc != null)
            {
                TempData["ErrorMessage"] = "Không thể xóa giảng viên vì giảng viên này đã có trong lớp học, hãy tạo mới";
                return RedirectToAction("Giangvien", "QLGiangvien");
            }


            data.GiangViens.DeleteOnSubmit(giangvien);
            data.SubmitChanges();

            TempData["SuccessMessage"] = "Đã xóa!";
            return RedirectToAction("Giangvien", "QLGiangvien");
        }


            public ActionResult Xemchitiet(int IDgv)
        {

            var show1giangvien = data.GiangViens.FirstOrDefault(s => s.IDGiangvien == IDgv);
            if (show1giangvien == null)
            {
                return HttpNotFound();
            }
            return View(show1giangvien);

        }
        public ActionResult Suagiangvien(int IDgv)
        {

            GiangVien giangvien = data.GiangViens.SingleOrDefault(c => c.IDGiangvien == IDgv);
            if (giangvien == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(giangvien);
            }
        }

        [HttpPost]
        public ActionResult Suagiangvien(GiangVien editGiangvien)
        {
            if (ModelState.IsValid)
            {
                string hinhUrl = Request.Form["Hinh"];
                string bangCapUrl = Request.Form["BangCap"];
                GiangVien giangvien = data.GiangViens.SingleOrDefault(c => c.IDGiangvien == editGiangvien.IDGiangvien);
                if (giangvien != null)
                {
                    giangvien.IDGiangvien = editGiangvien.IDGiangvien;
                    giangvien.TenGiangVien = editGiangvien.TenGiangVien;
                    giangvien.DiaChi = editGiangvien.DiaChi;
                    giangvien.SoDienThoai = editGiangvien.SoDienThoai;
                    giangvien.Email = editGiangvien.Email;
                    giangvien.Hinh = editGiangvien.Hinh;
                      giangvien.BangCap = editGiangvien.BangCap;
                    giangvien.Luong = editGiangvien.Luong;
                    data.SubmitChanges();
                    return RedirectToAction("Giangvien");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return View(editGiangvien);
            }
        }
        public ActionResult add(int IDgv)
        {
            TempData["IDgv"] = IDgv;
            return RedirectToAction("showid", "QLLophoc");
        }

        #endregion
    }
}