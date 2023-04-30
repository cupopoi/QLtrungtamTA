using TrungTamTA.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                ViewData["Loi1"] = "Đã có";
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
            // Lấy danh sách các học viên từ cơ sở dữ liệu
            var giangvien = data.GiangViens.FirstOrDefault(c => c.IDGiangvien == IDgv);
            if (giangvien == null)
            {
                return HttpNotFound();
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