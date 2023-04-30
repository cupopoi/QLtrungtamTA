using TrungTamTA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Linq;
using Antlr.Runtime.Misc;


namespace TrungTamTA.Controllers
{
    public class QLLophocController : Controller
    {
        dbQLtrungtamDataContext data = new dbQLtrungtamDataContext();
        // GET: QLLophoc
        #region Show lớp học
        public ActionResult Lophoc()
        {
            var lophoc = from lh in data.LopHocs select lh;
            return View(lophoc);
        }
        #endregion
        #region Thêm xóa sửa
        private bool checkid(int id)
        {
            return data.LopHocs.Count(x => x.IDLophoc == id) > 0;
        }

       
       //hiện form thêm lớp học
        public ActionResult Themlophoc()
        {
            return View();
        }
        //xử lý yêu cầu để hiện qua danh sách giảng viên
        public ActionResult Themgvvaolop(int? idaction, int idlh)
        {
            return RedirectToAction("Giangvien", "QLGiangvien",new { idaction, idlh });
        }
        //xử lý yêu cầu để hiện qua danh sách các chương trình học( có đem theo giá trị của giảng viên)
        public ActionResult Themctvaolop(int? Idremen, int? idaction, int? idlh)
        {
           
            return RedirectToAction("Chuongtrinhhoc", "QLChuongtrinhhoc", new { IDmember = Idremen, idaction, idlh });

        }

        //xử lý yêu cầu khi đã lấy được giảng viên
        public ActionResult Themlophocdacogv(int IDgv)
        {

            var namegv = data.GiangViens.FirstOrDefault(x => x.IDGiangvien == IDgv).TenGiangVien;
            ViewBag.idgv = IDgv;
            ViewBag.namegv = namegv;
            return View();
        }

        [HttpGet]
        public ActionResult Themlophocdacoct(int Idremen, int IDct)
        {
            var namegv = data.GiangViens.FirstOrDefault(x => x.IDGiangvien == Idremen).TenGiangVien;
            var namect = data.ChuongTrinhHocs.FirstOrDefault(x => x.IDChuongTrinh == IDct).TenChuongTrinh;
            ViewBag.idgv = Idremen;
            ViewBag.namegv = namegv;
            ViewBag.idct = IDct;
            ViewBag.namect = namect;
            return View();
        }
        [HttpPost]
        public ActionResult Themlophocdacoct(FormCollection collection, LopHoc lophoc, int Idremen, int IDct)
        {

            var idlophoc = collection["IDLophoc"];
            var tenlophoc = collection["Tenlophoc"];
            
            if (checkid(int.Parse(idlophoc)))
            {
                ViewData["Loi1"] = "Đã có";
            }
            else
            {
              
                lophoc.IDLophoc = int.Parse(idlophoc);
                lophoc.TenLopHoc = tenlophoc;
                lophoc.IDGiangVien = Idremen;
                lophoc.IDChuongTrinh = IDct;
                data.LopHocs.InsertOnSubmit(lophoc);
                data.SubmitChanges();
                return RedirectToAction("Lophoc");
            }
            return this.Themlophoc();
        }
        public ActionResult Xoalophoc(int IDlh)
        {
            // Lấy danh sách các học viên từ cơ sở dữ liệu
            var lophoc = data.LopHocs.FirstOrDefault(c => c.IDLophoc == IDlh);
            if (lophoc == null)
            {
                return HttpNotFound();
            }

            data.LopHocs.DeleteOnSubmit(lophoc);
            data.SubmitChanges();
            TempData["SuccessMessage"] = "Đã xóa!";
            return RedirectToAction("Lophoc", "QLLophoc");
        }
        //SỬA LỚP HỌC 
      

        // xử lý yêu cầu khi có được giảng viên trong view sửa
        public ActionResult Sualophoccogv(int IDlh, int IDgv)
        {
            ViewBag.Idlh = IDlh;
            var namegv = data.GiangViens.FirstOrDefault(x => x.IDGiangvien == IDgv).TenGiangVien;
            ViewBag.idgv = IDgv;
            ViewBag.namegv = namegv;
            LopHoc lophoc = data.LopHocs.SingleOrDefault(c => c.IDLophoc == IDlh);
            if (lophoc == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(lophoc);
            }
           
        }
        //hiển thị 1 lớp học yêu cầu để sửa
    
        public ActionResult Sualophoc(int IDlh)
        {
            ViewBag.IDlh = IDlh;
            LopHoc lophoc = data.LopHocs.SingleOrDefault(c => c.IDLophoc == IDlh);
            if (lophoc == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(lophoc);
            }
        }


         [HttpGet]
        public ActionResult Sualophoccoct(int IDlh, int? IDct, int? Idremen)
        {
            ViewBag.Idlh = IDlh;
            var namegv = data.GiangViens.FirstOrDefault(x => x.IDGiangvien == Idremen).TenGiangVien;
            var namect = data.ChuongTrinhHocs.FirstOrDefault(x => x.IDChuongTrinh == IDct).TenChuongTrinh;
            ViewBag.idgv = Idremen;
            ViewBag.namegv = namegv;
            ViewBag.idct = IDct;
            ViewBag.namect = namect;
            LopHoc lophoc = data.LopHocs.SingleOrDefault(c => c.IDLophoc == IDlh);
            if (lophoc == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(lophoc);
            }

        }

        [HttpPost]
        public ActionResult Sualophoccoct(LopHoc editlophoc, int? IDct, int? Idremen,int IDlh)
        {
          
            if (ModelState.IsValid)
            {
                LopHoc lophoc = data.LopHocs.SingleOrDefault(c => c.IDLophoc == IDlh);
                if (lophoc != null)
                {
                    lophoc.IDLophoc = editlophoc.IDLophoc;
                    lophoc.TenLopHoc = editlophoc.TenLopHoc;
                    lophoc.IDGiangVien = Idremen ?? lophoc.IDGiangVien;
                    lophoc.IDChuongTrinh = IDct ?? lophoc.IDChuongTrinh;
                    data.SubmitChanges();
                    return RedirectToAction("Lophoc");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return View(editlophoc);
            }
        }
        #endregion
    }
}