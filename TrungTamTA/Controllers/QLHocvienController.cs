using TrungTamTA.Models;
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
        private bool checkid(int id)
        {
            return data.HocViens.Count(x => x.IDHocvien == id) > 0;
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
            string ngaysinh = collection["Ngaysinh"];
            var diachi = collection["DiaChi"];
            var sodt = collection["Phone"];
            var email = collection["Email"];
            if (checkid(int.Parse(idhocvien)))
            {
                ViewData["Loi1"] = "Đã có";
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
            // Lấy danh sách các học viên từ cơ sở dữ liệu
            var hocvien = data.HocViens.FirstOrDefault(c => c.IDHocvien == IDhv);
            if (hocvien == null)
            {
                return HttpNotFound();
            }

            data.HocViens.DeleteOnSubmit(hocvien);
            data.SubmitChanges();
            TempData["SuccessMessage"] = "Đã xóa!";
            return RedirectToAction("Hocvien", "QLHocvien");
        }
        public ActionResult Xemchitiet(int IDhv)
        {

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
                    hocvien.NgaySinh = DateTime.ParseExact(editHocvien.NgaySinh.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
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


        #endregion
    }
}
