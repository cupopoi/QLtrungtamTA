using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrungTamTA.Models;

namespace TrungTamTA.Controllers
{
    public class QLLichhocController : Controller
    {
        dbQLtrungtamDataContext data = new dbQLtrungtamDataContext();
        // GET: QLLichhoc
        #region Show lớp học
        public ActionResult Lichhoc()
        {
            var lichhoc = from lhoc in data.ChiTietLichHocs select lhoc;
            return View(lichhoc);
        }
        #endregion
    }
}