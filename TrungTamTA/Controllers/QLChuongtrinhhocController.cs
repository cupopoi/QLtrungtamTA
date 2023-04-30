using TrungTamTA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace QLtrungtam.Controllers
{
    public class QLChuongtrinhhocController : Controller
    {
        dbQLtrungtamDataContext data = new dbQLtrungtamDataContext();
        // GET: QLHocvien
        #region Show chương trình học

        public ActionResult Chuongtrinhhoc(int? IDmember, int? idaction, int? idlh)
        {
            ViewBag.idgv = IDmember;
            ViewBag.idaction = idaction;
            ViewBag.IDlh = idlh;
            var chuongtrinh = from ct in data.ChuongTrinhHocs select ct;
            return View(chuongtrinh);
        }
        #endregion
        #region Thêm xóa sửa
        private bool checkid(int id)
        {
            return data.ChuongTrinhHocs.Count(x => x.IDChuongTrinh == id) > 0;
        }

        [HttpGet]
        public ActionResult Themchuongtrinh()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Themchuongtrinh(FormCollection collection, ChuongTrinhHoc chuongtrinh)
        {


            var idchuongtrinh = collection["IDchuongtrinh"];
            var tenchuongtrinh = collection["Tenchuongtrinh"];
            var sobuoihoc = collection["Sobuoihoc"];
            var thoiluong = collection["Thoiluong"];
            var giatien = collection["Giatien"];
            var mota = collection["Mota"];
            if (checkid(int.Parse(idchuongtrinh)))
            {
                ViewData["Loi1"] = "Đã có";
            }
            else
            {
                chuongtrinh.IDChuongTrinh = int.Parse(idchuongtrinh);
                chuongtrinh.TenChuongTrinh = tenchuongtrinh;
                chuongtrinh.SoBuoiHoc = sobuoihoc;
                chuongtrinh.ThoiLuong = thoiluong;
                chuongtrinh.GiaTien = int.Parse(giatien);
                chuongtrinh.MoTa = mota;
                data.ChuongTrinhHocs.InsertOnSubmit(chuongtrinh);
                data.SubmitChanges();
                return RedirectToAction("Chuongtrinhhoc");
            }
            return this.Themchuongtrinh();
        }
        public ActionResult Xoachuongtrinh(int IDct)
        {
            // Lấy danh sách các học viên từ cơ sở dữ liệu
            var chuongtrinh = data.ChuongTrinhHocs.FirstOrDefault(c => c.IDChuongTrinh == IDct);
            if (chuongtrinh == null)
            {
                return HttpNotFound();
            }

            data.ChuongTrinhHocs.DeleteOnSubmit(chuongtrinh);
            data.SubmitChanges();
            TempData["SuccessMessage"] = "Đã xóa!";
            return RedirectToAction("Chuongtrinhhoc", "QLChuongtrinhhoc");
        }
        public ActionResult Xemchitiet(int IDct)
        {

            var show1chuongtrinh = data.ChuongTrinhHocs.FirstOrDefault(s => s.IDChuongTrinh == IDct);
            if (show1chuongtrinh == null)
            {
                return HttpNotFound();
            }
            return View(show1chuongtrinh);

        }
        public ActionResult Suachuongtrinh(int IDct)
        {

            ChuongTrinhHoc chuongtrinh = data.ChuongTrinhHocs.SingleOrDefault(c => c.IDChuongTrinh == IDct);
            if (chuongtrinh == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(chuongtrinh);
            }
        }

        [HttpPost]
        public ActionResult Suachuongtrinh(ChuongTrinhHoc editchuongtrinh)
        {
            if (ModelState.IsValid)
            {
                ChuongTrinhHoc chuongtrinh = data.ChuongTrinhHocs.SingleOrDefault(c => c.IDChuongTrinh == editchuongtrinh.IDChuongTrinh);
                if (chuongtrinh != null)
                {
                    chuongtrinh.IDChuongTrinh = editchuongtrinh.IDChuongTrinh;
                    chuongtrinh.TenChuongTrinh = editchuongtrinh.TenChuongTrinh;
                    chuongtrinh.SoBuoiHoc = editchuongtrinh.SoBuoiHoc;
                    chuongtrinh.ThoiLuong = editchuongtrinh.ThoiLuong;
                    chuongtrinh.GiaTien = editchuongtrinh.GiaTien;
                    chuongtrinh.MoTa = editchuongtrinh.MoTa;
                    data.SubmitChanges();
                    return RedirectToAction("Chuongtrinhhoc");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return View(editchuongtrinh);
            }
        }
        #endregion
    }
}