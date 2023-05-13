using TrungTamTA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Linq;
using Antlr.Runtime.Misc;
using System.Data;
using System.Globalization;
using Microsoft.Owin.Security.Provider;

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
        #region funtion
        private bool checkid(int id)
        {
            return data.LopHocs.Count(x => x.IDLophoc == id) > 0;
        }


        #endregion
        #region Thêm xóa sửa



        //hiện form thêm lớp học
        public ActionResult Themlophoc()
        {
            return View();
        }
        //xử lý yêu cầu để hiện qua danh sách giảng viên
        public ActionResult Themgvvaolop(int? idaction, int idlh)
        {
            return RedirectToAction("Giangvien", "QLGiangvien", new { idaction, idlh });
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
        //Hiển thị về view danh sách đã có lưu id của giảng viên và cả lớp học và chương trình
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
        public ActionResult Sualophoccoct(LopHoc editlophoc, int? IDct, int? Idremen, int IDlh)
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

        //thêm học viên vào lớp học
        public ActionResult themhvvaolop(int IDlh)
        {
            ViewBag.Idlh = IDlh;
            string namelh = data.LopHocs.FirstOrDefault(x => x.IDLophoc == IDlh).TenLopHoc.ToString();
            ViewBag.namelh = namelh;
            bool checkSL = data.ChiTietLopHocs.Count(x => x.IDLophoc == IDlh) >= 30;
            if (checkSL)
            {
                TempData["AlertMessage"] = "Lớp đầy";
                return RedirectToAction("Lophoc");
            }
            else {
                //!data.ChiTietLopHocs.Any(ct => cxt.IDHocVien == hv.IDHocvien) để lọc ra các học viên không có bản ghi tương ứng trong bảng ChiTietLopHoc.
                var hocvienList = data.HocViens.Where(hv => !data.ChiTietLopHocs.Any(ct => ct.IDHocVien == hv.IDHocvien || (ct.IDHocVien == hv.IDHocvien && ct.DiemTB == null))).ToList();
                return View(hocvienList);
            }
        }
        // nút xem danh sách học sinh trong lớp
        public ActionResult Xemhvtronglop(int IDlh)
        {
            ViewBag.Idlh = IDlh;

            var namelh = data.LopHocs.FirstOrDefault(x => x.IDLophoc == IDlh)?.TenLopHoc;
            ViewBag.namelh = namelh;

            //List<HocVien> hocvienList = data.HocViens
            //    //Dòng này thực hiện phép nối (Join) giữa hai bảng "HocVien" và "ChiTietLopHoc".
            //    .Join(data.ChiTietLopHocs,
            //          //Tham số thứ nhất hocvien => hocvien.IDHocvien chỉ định cột trong bảng "HocVien" để so khớp.
            //          hocvien => hocvien.IDHocvien,
            //          //Tham số thứ hai chitiet => chitiet.IDHocVien chỉ định cột trong bảng "ChiTietLopHoc" để so khớp.
            //          chitiet => chitiet.IDHocVien,
            //          //(hocvien, chitiet) => new { HocVien = hocvien, ChiTiet = chitiet }
            //          //tạo một đối tượng mới từ kết quả nối,
            //          //trong đó lưu trữ thông tin của học viên và chi tiết lớp học tương ứng.
            //          (hocvien, chitiet) => new { HocVien = hocvien, ChiTiet = chitiet })
            //    .Where(x => x.ChiTiet.IDLophoc == IDlh)
            //    .Select(x => x.HocVien)
            //    .ToList();
            List<HocVien> hocvienList = new List<HocVien>();
            List<ChiTietLopHoc> listIdHocVien = data.ChiTietLopHocs.Where(item => item.IDLophoc == IDlh).ToList();
            foreach (ChiTietLopHoc item in listIdHocVien) {
                HocVien hocVien = data.HocViens.SingleOrDefault(x => x.IDHocvien == item.IDHocVien);
                hocvienList.Add(hocVien);
            }
            int soHocSinh = hocvienList.Count;
            ViewBag.count = soHocSinh;
            return View(hocvienList);
        }


        //nút action AddToClass
        public ActionResult addtoclass(int IDlh, int IDhv)
        {
            ChiTietLopHoc hocvien = new ChiTietLopHoc();
            hocvien.IDLophoc = IDlh;
            hocvien.IDHocVien = IDhv;
            data.ChiTietLopHocs.InsertOnSubmit(hocvien);
            data.SubmitChanges();
            return RedirectToAction("themhvvaolop", new { IDlh });
        }

        //nút xóa học sinh ra khỏi lớp
        public ActionResult DeleteStudent(int? IDlh, int? IDhv)
        {
            if (IDlh != null && IDhv != null)
            {
                ChiTietLopHoc hocvien = data.ChiTietLopHocs.FirstOrDefault(x => x.IDHocVien == IDhv && x.IDLophoc == IDlh);
                if (hocvien != null)
                {
                    data.ChiTietLopHocs.DeleteOnSubmit(hocvien);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("Xemhvtronglop", new { IDlh });
        }

        //nút thêm điểm cho học viên
        [HttpGet]
        public ActionResult addScore(int IDlh, int IDhv)
        {
            ViewBag.IDlh = IDlh;
            ViewBag.IDhv = IDhv;
            string namelh = data.LopHocs.FirstOrDefault(x => x.IDLophoc == IDlh).TenLopHoc.ToString();
            ViewBag.namelh = namelh;
            string namehv = data.HocViens.FirstOrDefault(x => x.IDHocvien == IDhv).TenHocVien.ToString();
            ViewBag.namehv = namehv;
            return View();
        }

        [HttpPost]
        public ActionResult addScore(FormCollection collection, int IDlh, int IDhv)
        {
            var hocVien = data.HocViens.FirstOrDefault(x => x.IDHocvien == IDhv);
            var lopHoc = data.LopHocs.FirstOrDefault(x => x.IDLophoc == IDlh);

            float diemnghe = float.Parse(collection["Diemnghe"]);
            float diemnoi = float.Parse(collection["Diemnoi"]);
            float diemdoc = float.Parse(collection["Diemdoc"]);
            float diemviet = float.Parse(collection["Diemviet"]);

            if (diemnghe > 10 && diemnoi > 10 && diemdoc > 10 && diemviet > 10)
            {
                ViewData["Loi1"] = "Điểm phải nhỏ hơn 10";
            }
            else if (hocVien != null && lopHoc != null)
            {
                // Kiểm tra xem đã tồn tại bản ghi trong ChiTietLopHoc có cùng IDHocVien và IDLophoc hay chưa
                var chiTietLopHoc = data.ChiTietLopHocs.FirstOrDefault(x => x.IDHocVien == hocVien.IDHocvien && x.IDLophoc == lopHoc.IDLophoc);

                if (chiTietLopHoc != null)
                {
                    // Cập nhật các cột phụ trong bản ghi hiện có
                    chiTietLopHoc.DiemNghe = diemnghe;
                    chiTietLopHoc.DiemNoi = diemnoi;
                    chiTietLopHoc.DiemDoc = diemdoc;
                    chiTietLopHoc.DiemViet = diemviet;
                }
                else
                {
                    // Tạo một bản ghi mới và gán giá trị
                    ChiTietLopHoc chitietlophoc = new ChiTietLopHoc();
                    chitietlophoc.IDHocVien = hocVien.IDHocvien;
                    chitietlophoc.IDLophoc = lopHoc.IDLophoc;
                    chitietlophoc.DiemNghe = diemnghe;
                    chitietlophoc.DiemNoi = diemnoi;
                    chitietlophoc.DiemDoc = diemdoc;
                    chitietlophoc.DiemViet = diemviet;

                    // Thêm bản ghi mới vào ChiTietLopHoc
                    data.ChiTietLopHocs.InsertOnSubmit(chitietlophoc);
                }

                // Lưu các thay đổi vào cơ sở dữ liệu
                data.SubmitChanges();
                return RedirectToAction("Xemhvtronglop", new { IDlh });
            }

            // Nếu không tìm thấy hocVien hoặc lopHoc
            return this.addScore(IDlh, IDhv);
        }


        public ActionResult viewScore(int IDlh, int IDhv)
        {
            ViewBag.IDlh = IDlh;
            ViewBag.IDhv = IDhv;
            string namelh = data.LopHocs.FirstOrDefault(x => x.IDLophoc == IDlh).TenLopHoc.ToString();
            ViewBag.namelh = namelh;
            string namehv = data.HocViens.FirstOrDefault(x => x.IDHocvien == IDhv).TenHocVien.ToString();
            ViewBag.namehv = namehv;
            var score = from sc in data.ChiTietLopHocs select sc;
            return View(score);
        }
       
            #endregion
        }
}