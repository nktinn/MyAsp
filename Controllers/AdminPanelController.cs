using MyAsp.Storage;
using MyAsp.Models;

using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using System.Diagnostics;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Drawing;
using System.Globalization;
using MyAsp.Storage.Entities;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

using MyAsp.Logic.Accounts;
using MyAsp.Logic.Files;

namespace MyAsp.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookieScheme")]
    public class AdminPanelController : Controller
    {
        private readonly IAccountManager _accmngr;
        private readonly IAdmAccountManager _admmngr;
        private readonly IFileManager _filemngr;
        public AdminPanelController(IAccountManager accmngr, IFileManager filemngr, IAdmAccountManager admmngr)
        {
            _accmngr = accmngr;
            _filemngr = filemngr;
            _admmngr = admmngr;
        }

        public async Task<IActionResult> Statistics()
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (account.Role == "3" || account.Role == "admin")
                {
                    ViewBag.Account = account;
                    ViewBag.Asps = _admmngr.GetAsps();
                    ViewBag.Adms = _admmngr.GetAdmins();
                    ViewBag.Grants = _admmngr.GetGrants();
                    ViewBag.Results = _admmngr.GetResults();
                    ViewBag.Timetables = _admmngr.GetTimetables();
                    ViewBag.Achievments = _admmngr.GetAllAchievments();
                    ViewBag.Docs = _admmngr.GetDocs();
                    ViewBag.Scientists = _admmngr.GetScientists();
                    ViewBag.Directions = _admmngr.GetDirections();
                    ViewBag.News = _admmngr.GetNews();
                    ViewBag.PM = _admmngr.GetAllPM();
                    return View();
                }
                else
                {
                    return RedirectToAction("Account", "Admin");
                }
            }
        }

        #region "Добавления"
        public async Task<IActionResult> AddAsp(string FIO, string cnumber, string gnumber, string stDir, string stProf, string theme, string scientist, string tel, string mail)
        {
            using (var writepackage = new XSSFWorkbook())
            {
                writepackage.CreateSheet("Лист 1");
                ISheet writeworksheet = writepackage.GetSheetAt(0);

                IRow headerRow = writeworksheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Номер удостоверения");
                headerRow.CreateCell(1).SetCellValue("Логин");
                headerRow.CreateCell(2).SetCellValue("Пароль");


                try
                {
                    string[] fio_array = FIO.Split(" ");
                    User aspaccount = new User();
                    aspaccount.Surname = fio_array[0];
                    aspaccount.Name = fio_array[1];
                    if (fio_array.Length > 2)
                        aspaccount.Patronymic = fio_array[2];
                    else
                        aspaccount.Patronymic = null;

                    aspaccount.CNumber = cnumber;
                    aspaccount.Login = $"asp{aspaccount.CNumber.Replace("/", "")}";
                    aspaccount.GNumber = gnumber;
                    aspaccount.StDirection = stDir;
                    if (stProf != null)
                        aspaccount.ProfDirection = stProf;
                    else
                        aspaccount.ProfDirection = null;
                    if (theme != null)
                        aspaccount.WTheme = theme;
                    else
                        aspaccount.WTheme = null;
                    if (tel != null)
                        aspaccount.Phone = tel;
                    else
                        aspaccount.Phone = null;
                    if (mail != null)
                        aspaccount.Email = mail;
                    else
                        aspaccount.Email = null;
                    aspaccount.Password = Guid.NewGuid().ToString("N").Substring(0, 10);
                    aspaccount.Photo = "alt.png";

                    IRow returnRow = writeworksheet.CreateRow(1);

                    returnRow.CreateCell(0).SetCellValue($"{aspaccount.CNumber}");
                    returnRow.CreateCell(1).SetCellValue($"{aspaccount.Login}");
                    returnRow.CreateCell(2).SetCellValue($"{aspaccount.Password}");

                    aspaccount.Password = GetHashCode(aspaccount.Password, aspaccount.Login);

                    await _admmngr.AddAsp(aspaccount);
                }
                catch (Exception ex)
                {
                    IRow returnRow = writeworksheet.CreateRow(1);

                    returnRow.CreateCell(0).SetCellValue("Пользователь не добавлен");
                    returnRow.CreateCell(1).SetCellValue($"{ex.Message}");
                }

                using (var memoryStream = new MemoryStream())
                {
                    writepackage.Write(memoryStream);

                    await _accmngr.UpdateContext();

                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Новый аспирант.xlsx");
                }
            }
        }
        public async Task<IActionResult> AddAdmin(string FIO, string funtion, string cab, string roleLvl, string tel, string mail)
        {
            using (var writepackage = new XSSFWorkbook())
            {
                writepackage.CreateSheet("Лист 1");
                ISheet writeworksheet = writepackage.GetSheetAt(0);

                IRow headerRow = writeworksheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("ФИО");
                headerRow.CreateCell(1).SetCellValue("Логин");
                headerRow.CreateCell(2).SetCellValue("Пароль");


                try
                {
                    Admin admaccount = new Admin();

                    string[] fio_array = FIO.Split(" ");

                    admaccount.Surname = fio_array[0];
                    admaccount.Name = fio_array[1];

                    if (fio_array.Length > 2)
                        admaccount.Patronymic = fio_array[2];
                    else
                        admaccount.Patronymic = null;
                    admaccount.Email = mail;
                    admaccount.Login = admaccount.Email;
                    admaccount.Phone = tel;
                    admaccount.Function = funtion;
                    admaccount.Cabinet = cab;
                    admaccount.Password = Guid.NewGuid().ToString("N").Substring(0, 8);
                    admaccount.Photo = "alt.png";
                    admaccount.Role = roleLvl;


                    IRow returnRow = writeworksheet.CreateRow(1);

                    returnRow.CreateCell(0).SetCellValue($"{admaccount.Surname} {admaccount.Name} {admaccount.Patronymic}");
                    returnRow.CreateCell(1).SetCellValue($"{admaccount.Login}");
                    returnRow.CreateCell(2).SetCellValue($"{admaccount.Password}");

                    admaccount.Password = GetHashCode(admaccount.Password, admaccount.Login);

                    string message = $"Администратор {admaccount.Login} - {admaccount.Surname} {admaccount.Name} добавлен";
                    await _admmngr.SendInfo("Добавлен администратор", message);

                    await _admmngr.AddAdmin(admaccount);
                }
                catch (Exception ex)
                {
                    IRow returnRow = writeworksheet.CreateRow(1);

                    returnRow.CreateCell(0).SetCellValue("Администратор не добавлен");
                    returnRow.CreateCell(1).SetCellValue($"{ex.Message}");
                }

                using (var memoryStream = new MemoryStream())
                {
                    writepackage.Write(memoryStream);

                    await _accmngr.UpdateContext();

                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Новые сотрудники.xlsx");
                }

            }
        }
        public async Task<IActionResult> AddGrant(string name, string amount, string startDate, string endDate, string aspId)
        {
            Grant grant = new Grant();
            grant.Name = name;
            grant.Amount = Convert.ToDouble(amount);
            grant.StartDate = DateOnly.FromDateTime(Convert.ToDateTime(startDate));
            grant.ExpiryDate = DateOnly.FromDateTime(Convert.ToDateTime(endDate));
            grant.UserID = Convert.ToInt32(aspId);

            await _admmngr.AddGrant(grant);

            await _accmngr.UpdateContext();
            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> AddResult(string subj, string result, string type, string date, string aspId)
        {
            Result res = new Result();
            res.SubjName = subj;
            res.UserID = Convert.ToInt32(aspId);
            res.Grade = result;
            res.Type = type;
            res.Date = DateOnly.FromDateTime(Convert.ToDateTime(date));

            await _admmngr.AddResult(res);

            await _accmngr.UpdateContext();
            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> AddScientist(string FIO, string degree, string depart, string tel, string mail)
        {
            Scientist scientist = new Scientist();
            scientist.Degree = degree;
            scientist.Name = FIO;
            scientist.Department = depart;
            scientist.Phone = tel;
            scientist.Email = mail;

            await _admmngr.AddScientist(scientist);

            await _accmngr.UpdateContext();
            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> AddDirection(string name, string prof, string depart)
        {
            Direction direction = new Direction();
            direction.Name = name;
            direction.Profile = prof;
            direction.Department = depart;

            await _admmngr.AddDirection(direction);

            await _accmngr.UpdateContext();
            return RedirectToAction("Statistics", "AdminPanel");
        }
        #endregion

        #region "Удаления"
        public async Task<IActionResult> DeleteAsp(string id)
        {
            var aspaccount = _admmngr.GetAsp(Convert.ToInt32(id));
            if (aspaccount != null)
            {
                if (aspaccount.Photo != "alt.png")
                {
                    string prevPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Avatars/", aspaccount.Photo);
                    try
                    {
                        FileInfo fileInfo = new FileInfo(prevPath);
                        if (fileInfo.Exists)
                            fileInfo.Delete();
                    }
                    catch (Exception ex)
                    { }
                }
                var achiev = _admmngr.GetAchievments(aspaccount);
                foreach (var ach in achiev)
                {
                    string prevPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/achievments/", ach.File);
                    try
                    {
                        FileInfo fileInfo = new FileInfo(prevPath);
                        if (fileInfo.Exists)
                            fileInfo.Delete();
                    }
                    catch (Exception ex)
                    { }
                }
                await _accmngr.UpdateContext();
            }
            await _admmngr.RemoveAspById(Convert.ToInt32(id));
            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> DeleteAdmin(string id)
        {
            await _admmngr.RemoveAdminById(Convert.ToInt32(id));

            var admin = _admmngr.GetAdminById(Convert.ToInt32(id));
            string message = $"Администратор {admin.Login} - {admin.Surname} {admin.Name} удален";
            await _admmngr.SendInfo("Удален администратор", message);

            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> DeleteGrant(string id)
        {
            await _admmngr.RemoveGrantById(Convert.ToInt32(id));
            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> DeleteResult(string id)
        {
            await _admmngr.RemoveResById(Convert.ToInt32(id));
            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> DeleteTimetable(string id)
        {
            await _admmngr.RemoveTimetById(Convert.ToInt32(id));
            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> DeleteAchievment(string id)
        {
            await _admmngr.RemoveAchievById(Convert.ToInt32(id));
            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> DeleteDoc(string id)
        {
            await _admmngr.RemoveDocById(Convert.ToInt32(id));
            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> DeleteScientist(string id)
        {
            await _admmngr.RemoveScienceById(Convert.ToInt32(id));
            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> DeleteDirection(string id)
        {
            await _admmngr.RemoveDirectById(Convert.ToInt32(id));
            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> DeleteNew(string id)
        {
            await _admmngr.RemoveNewById(Convert.ToInt32(id));
            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> DeletePM(string id)
        {
            await _admmngr.RemovePMById(Convert.ToInt32(id));
            return RedirectToAction("Statistics", "AdminPanel");
        }
        #endregion

        #region "Обновления"
        public async Task<IActionResult> UpdateAsp(string FIO, string cnumber, string gnumber, string stDir, string stProf, string theme, string scientist, string tel, string mail, string id)
        {
            var asp = _admmngr.GetAsp(Convert.ToInt32(id));
            try
            {
                string[] fio_array = FIO.Split(" ");
                asp.Surname = fio_array[0];
                asp.Name = fio_array[1];
                if (fio_array.Length > 2)
                    asp.Patronymic = fio_array[2];
                else
                    asp.Patronymic = null;
                asp.CNumber = cnumber;
                asp.GNumber = gnumber;
                asp.StDirection = stDir;
                asp.ProfDirection = stProf;
                asp.WTheme = theme;
                asp.Phone = tel;
                asp.Email = mail;

                var science = _admmngr.GetScienceByName(scientist);
                if (science != null)
                    asp.ScientistID = science.Id;
                else
                    asp.ScientistID = null;

                await _accmngr.UpdateContext();
            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> UpdateAdmin(string FIO, string funtion, string cab, string roleLvl, string tel, string mail, string id)
        {
            var admin = _admmngr.GetAdminById(Convert.ToInt32(id));

            try
            {
                string[] fio_array = FIO.Split(" ");
                admin.Surname = fio_array[0];
                admin.Name = fio_array[1];
                if (fio_array.Length > 2)
                    admin.Patronymic = fio_array[2];
                else
                    admin.Patronymic = null;
                admin.Function = funtion;
                admin.Role = roleLvl;
                admin.Cabinet = cab;
                admin.Email = mail;
                admin.Phone = tel;

                await _accmngr.UpdateContext();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                string message = $"Администратор {admin.Login} - {admin.Surname} {admin.Name} изменен";
                await _admmngr.SendInfo("Изменен администратор", message);
            }

            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> UpdateGrant(string name, string amount, string startDate, string endDate, string aspId, string id)
        {
            var grant = _admmngr.GetGrantById(Convert.ToInt32(id));

            try
            {
                grant.Name = name;
                grant.Amount = Convert.ToDouble(amount);
                grant.StartDate = DateOnly.FromDateTime(Convert.ToDateTime(startDate));
                grant.ExpiryDate = DateOnly.FromDateTime(Convert.ToDateTime(endDate));
                grant.UserID = Convert.ToInt32(id);

                await _accmngr.UpdateContext();
            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> UpdateResult(string subj, string result, string type, string date, string aspId, string id)
        {
            var res = _admmngr.GetResById(Convert.ToInt32(id));

            try
            {
                res.SubjName = subj;
                res.Grade = result;
                res.Type = type;
                res.Date = DateOnly.FromDateTime(Convert.ToDateTime(date));
                res.UserID = Convert.ToInt32(id);

                await _accmngr.UpdateContext();
            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> UpdateTimetable(string name, string date, string type, string id)
        {
            var timet = _admmngr.GetTimetById(Convert.ToInt32(id));

            try
            {
                timet.Name = name;
                timet.Date = DateOnly.FromDateTime(Convert.ToDateTime(date));
                timet.Type = type;

                await _accmngr.UpdateContext();
            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> UpdateAchievment(string name, string date, string aspId, string id)
        {
            var achiev = _admmngr.GetAchievById(Convert.ToInt32(id));

            try
            {
                achiev.Name = name;
                achiev.Date = DateOnly.FromDateTime(Convert.ToDateTime(date));
                achiev.UserID = Convert.ToInt32(aspId);

                await _accmngr.UpdateContext();
            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> UpdateDoc(string name, string date, string aspId, string id)
        {
            var doc = _admmngr.GetDocById(Convert.ToInt32(id));

            try
            {
                doc.Name = name;
                doc.Date = DateOnly.FromDateTime(Convert.ToDateTime(date));
                doc.UserID = Convert.ToInt32(aspId);

                await _accmngr.UpdateContext();
            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> UpdateScientist(string FIO, string degree, string depart, string tel, string mail, string id)
        {
            var science = _admmngr.GetScienceById(Convert.ToInt32(id));

            try
            {
                science.Phone = tel;
                science.Name = FIO;
                science.Degree = degree;
                science.Department = depart;
                science.Email = mail;

                await _accmngr.UpdateContext();
            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> UpdateDirection(string name, string prof, string depart, string id)
        {
            var dir = _admmngr.GetDirectById(Convert.ToInt32(id));

            try
            {
                dir.Name = name;
                dir.Profile = prof;
                dir.Department = depart;

                await _accmngr.UpdateContext();
            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> UpdateNew(string info, string link, string date, string id)
        {
            var news = _admmngr.GetNewById(Convert.ToInt32(id));

            try
            {
                news.Link = link;
                news.Info = info;
                news.Date = DateOnly.FromDateTime(Convert.ToDateTime(date));

                await _accmngr.UpdateContext();
            }
            catch
            {
            }

            return RedirectToAction("Statistics", "AdminPanel");
        }
        public async Task<IActionResult> UpdatePM(string file, string date, string aspId, string id)
        {
            var pm = _admmngr.GetPMById(Convert.ToInt32(id));

            try
            {
                pm.File = file;
                pm.Date = Convert.ToDateTime(date);
                pm.UserID = Convert.ToInt32(id);

                await _accmngr.UpdateContext();
            }
            catch (Exception ex)
            {
            }

            return RedirectToAction("Statistics", "AdminPanel");
        }
        #endregion


        
    }
}
