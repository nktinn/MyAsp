using MyAsp.Storage;
using MyAsp.Models;

using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text.Json;

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
    public class AdminController : Controller
    {
        private readonly IAccountManager _accmngr;
        private readonly IAdmAccountManager _admmngr;
        private readonly IFileManager _filemngr;
        public AdminController(IAccountManager accmngr, IFileManager filemngr, IAdmAccountManager admmngr)
        {
            _accmngr = accmngr;
            _filemngr = filemngr;
            _admmngr = admmngr;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (_admmngr.GetAdmin(User.Identity.Name) != null)
                return RedirectToRoute(new { controller = "Admin", action = "Account" });
            else
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return View();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var password = GetHashCode(model.Password, model.Login);
                var account = _admmngr.SignIn(model.Login, password);
                if (account)
                {
                    await Authenticate(model.Login);
                    return RedirectToRoute(new { controller = "Admin", action = "Account" });
                }
                else
                {
                    model.ModelError = false;
                    ModelState.AddModelError("ModelError", "• Неверно введены данные");
                }
            }
            else
            {
                model.ModelError = false;
                ModelState.AddModelError("ModelError", "• Неверно введены данные");
            }
            return View(model);
        }

        public async Task<IActionResult> Account()
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                ViewBag.Account = account;
                if (TempData["Message"] != null)
                    ViewBag.Message = TempData["Message"];
                else
                    ViewBag.Message = 0;
                return View();
            }
        }

        public async Task<IActionResult> Asps()
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                var asps = _admmngr.GetAsps();
                if (asps != null)
                {
                    ViewBag.Asps = asps.OrderBy(u => u.CNumber).ToList();
                }
                else
                    ViewBag.Asps = new List<User>();
                if (TempData["Message"] != null)
                    ViewBag.Message = TempData["Message"];
                else
                    ViewBag.Message = 0;
                ViewBag.Scientists = _admmngr.GetScientists();
                ViewBag.UniqueDir = _admmngr.GetUniqDir();
                ViewBag.Directions = _admmngr.GetDirections();
                ViewBag.Account = account;
                ViewBag.PM_Count = _admmngr.GetPMCount();
                return View();
            }
        }

        public async Task<IActionResult> Docs()
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                ViewBag.Account = account;
                var asps = _admmngr.GetAsps();
                if (asps != null)
                {
                    ViewBag.Asps = asps.OrderByDescending(u =>
                    {
                        string[] parts = u.CNumber.Split('/');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int year))
                        {
                            return year;
                        }
                        return 0;
                    }).ThenBy(u => u.CNumber).ToList();
                }
                else
                    ViewBag.Asps = new List<User>();
                if (TempData["Message"] != null)
                    ViewBag.Message = TempData["Message"];
                else
                    ViewBag.Message = 0;
                ViewBag.Asp1 = null;
                ViewBag.Asp2 = null;
                return View();
            }
        }

        public async Task<IActionResult> Timetable()
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                ViewBag.Account = account;
                if (TempData["Message"] != null)
                    ViewBag.Message = TempData["Message"];
                else
                    ViewBag.Message = 0;
                return View();
            }
        }

        public async Task<IActionResult> Grants()
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                ViewBag.Account = account;
                var asps = _admmngr.GetAsps();
                if (asps != null)
                {
                    ViewBag.Asps = asps.OrderByDescending(u =>
                    {
                        string[] parts = u.CNumber.Split('/');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int year))
                        {
                            return year;
                        }
                        return 0;
                    }).ThenBy(u => u.CNumber).ToList();
                }
                else
                    ViewBag.Asps = new List<User>();
                if (TempData["Message"] != null)
                    ViewBag.Message = TempData["Message"];
                else
                    ViewBag.Message = 0;
                ViewBag.Asp1 = null;
                ViewBag.Asp2 = null;
                return View();
            }
        }

        public async Task<IActionResult> Contacts()
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                ViewBag.Account = account;
                ViewBag.Admins = _admmngr.GetAdmins();
                if (TempData["Message"] != null)
                    ViewBag.Message = TempData["Message"];
                else
                    ViewBag.Message = 0;
                return View();
            }
        }

        public async Task<IActionResult> PhotoManagement()
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                ViewBag.Account = account;
                var pm = _admmngr.GetLastPM();
                ViewBag.PM = pm;
                if (pm != null)
                    ViewBag.Asp = _admmngr.GetAsp(pm.UserID);
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AdminCookieScheme");
            return RedirectToAction("Login", "Admin");
        }



        /*View buttons and more*/

        [HttpPost]
        public async Task<IActionResult> ChangeInfo(string phone, string email, string cabinet, string function, string fio)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                try
                {
                    string[] fullname = fio.Split(" ");
                    account.Surname = fullname[0];
                    account.Name = fullname[1];
                    if (fullname.Length > 2)
                        account.Patronymic = fullname[2];
                    account.Function = function;
                    account.Cabinet = cabinet;
                    account.Phone = phone;
                    account.Email = email;

                    await _accmngr.UpdateContext();
                    TempData["Message"] = 1;
                }
                catch (Exception ex)
                {
                    TempData["Message"] = 2;
                }

                return RedirectToAction("Account", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePass(string oldpass, string newpass, string newpass2)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (oldpass != null && newpass != null && newpass2 != null)
                {
                    if (GetHashCode(oldpass, account.Login) == account.Password)
                    {
                        if (newpass == newpass2)
                        {
                            account.Password = GetHashCode(newpass, account.Login);
                            await _accmngr.UpdateContext();
                            TempData["Message"] = 1;
                        }
                        else
                            TempData["Message"] = 2;
                    }
                    else
                        TempData["Message"] = 2;
                }
                else
                    TempData["Message"] = 2;
                return RedirectToAction("Account", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto(IFormFile fileupload, string filesize)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (fileupload != null)
                {
                    var aspaccount = _admmngr.GetAdmin(User.Identity.Name);

                    var fileExtension = Path.GetExtension(fileupload.FileName).ToLower();
                    var newname = Convert.ToString(Guid.NewGuid());
                    var newFileName = newname + fileExtension;

                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Avatars/");
                    string fileNameWithPath = Path.Combine(path, newFileName);

                    string[] dots = filesize.Split(" ");

                    using (Image originalImage = Image.FromStream(fileupload.OpenReadStream()))
                    {
                        int width = Convert.ToInt32(Convert.ToDecimal(dots[2], CultureInfo.InvariantCulture) - Convert.ToDecimal(dots[0], CultureInfo.InvariantCulture));
                        int height = Convert.ToInt32(Convert.ToDecimal(dots[5], CultureInfo.InvariantCulture) - Convert.ToDecimal(dots[1], CultureInfo.InvariantCulture));

                        using (Bitmap croppedImage = new Bitmap(width, height))
                        {

                            Graphics graphics = Graphics.FromImage(croppedImage);

                            Rectangle destinationRectangle = new Rectangle(0, 0, width, height);
                            Rectangle sourceRectangle = new Rectangle(Convert.ToInt32(Convert.ToDecimal(dots[0], CultureInfo.InvariantCulture)), Convert.ToInt32(Convert.ToDecimal(dots[1], CultureInfo.InvariantCulture)), width, height);

                            graphics.DrawImage(originalImage, destinationRectangle, sourceRectangle, GraphicsUnit.Pixel);

                            croppedImage.Save(fileNameWithPath);
                        }
                    }
                    if (account.Photo != "alt.png")
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
                    aspaccount.Photo = newFileName;

                    await _accmngr.UpdateContext();
                    TempData["Message"] = 1;

                }
                else
                    TempData["Message"] = 2;
                return RedirectToAction("Account", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto()
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (account.Photo != "alt.png")
                {
                    string prevPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Avatars/", account.Photo);

                    try
                    {
                        FileInfo fileInfo = new FileInfo(prevPath);
                        if (fileInfo.Exists)
                            fileInfo.Delete();
                    }
                    catch (Exception ex)
                    { }
                }
                account.Photo = "alt.png";

                await _accmngr.UpdateContext();
                TempData["Message"] = 1;

                return RedirectToAction("Account", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddStip(string stips, string ids)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (stips != null && ids != null)
                {
                    string[] allid = ids.Split(" ");
                    string[] allstips = stips.Split(" ");
                    int stipCount = allstips.Length / 3;

                    foreach (var id in allid)
                    {
                        for (int i = 0; i < stipCount; i++)
                        {
                            try
                            {
                                Grant gr = new Grant();
                                var aspaccount = _admmngr.GetAsp(Convert.ToInt32(id));
                                if (allstips[i * 3] == "1")
                                    gr.Name = "Академическая стипендия";
                                else if (allstips[i * 3] == "2")
                                    gr.Name = "Социальная стипендия";
                                else if (allstips[i * 3] == "3")
                                    gr.Name = "Правительственная стипендия";
                                else if (allstips[i * 3] == "4")
                                    gr.Name = "Президентская стипендия";
                                else if (allstips[i * 3] == "5")
                                    gr.Name = "Материальная выплата";

                                gr.Amount = Convert.ToInt32(allstips[i * 3 + 1]);
                                gr.StartDate = DateOnly.FromDateTime(DateTime.Now.Date);
                                gr.ExpiryDate = DateOnly.FromDateTime(Convert.ToDateTime(allstips[i * 3 + 2]));
                                gr.UserID = Convert.ToInt32(aspaccount.Id);

                                await _admmngr.AddGrant(gr);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    await _accmngr.UpdateContext();
                    TempData["Message"] = 1;
                }
                else
                    TempData["Message"] = 2;
                return RedirectToAction("Grants", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDoc(string file, string ids)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (file != null && ids != null)
                {
                    string[] allid = ids.Split(" ");

                    foreach (var id in allid)
                    {
                        try
                        {
                            Document doc = new Document();
                            var aspaccount = _admmngr.GetAsp(Convert.ToInt32(id));
                            doc.Name = file;
                            doc.Date = DateOnly.FromDateTime(DateTime.Now);
                            doc.UserID = aspaccount.Id;

                            await _admmngr.AddDoc(doc);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    TempData["Message"] = 1;
                    await _accmngr.UpdateContext();

                }
                else
                    TempData["Message"] = 2;
                return RedirectToAction("Docs", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAsp(IFormFile fileupload)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (fileupload != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        await fileupload.CopyToAsync(stream);
                        stream.Position = 0;

                        using (var readpackage = new XSSFWorkbook(stream))
                        {
                            ISheet readworksheet = readpackage.GetSheetAt(0);
                            using (var writepackage = new XSSFWorkbook())
                            {
                                writepackage.CreateSheet("Лист 1");
                                ISheet writeworksheet = writepackage.GetSheetAt(0);

                                IRow headerRow = writeworksheet.CreateRow(0);
                                headerRow.CreateCell(0).SetCellValue("Номер удостоверения");
                                headerRow.CreateCell(1).SetCellValue("Логин");
                                headerRow.CreateCell(2).SetCellValue("Пароль");


                                for (int i = 1; i <= readworksheet.LastRowNum; i++)
                                {
                                    try
                                    {
                                        User aspaccount = new User();
                                        IRow row = readworksheet.GetRow(i);
                                        aspaccount.Surname = row.GetCell(0).ToString();
                                        aspaccount.Name = row.GetCell(1).ToString();
                                        if (row.GetCell(2).ToString() != null)
                                            aspaccount.Patronymic = row.GetCell(2).ToString();
                                        else
                                            aspaccount.Patronymic = null;

                                        aspaccount.CNumber = row.GetCell(3).ToString();
                                        aspaccount.Login = $"asp{aspaccount.CNumber.Replace("/", "")}";
                                        aspaccount.GNumber = row.GetCell(4).ToString();
                                        aspaccount.StDirection = row.GetCell(5).ToString();
                                        if (row.GetCell(6).ToString() != null)
                                            aspaccount.ProfDirection = row.GetCell(6).ToString();
                                        else
                                            aspaccount.ProfDirection = null;
                                        if (row.GetCell(7).ToString() != null)
                                            aspaccount.WTheme = row.GetCell(7).ToString();
                                        else
                                            aspaccount.WTheme = null;
                                        if (row.GetCell(8).ToString() != null)
                                            aspaccount.Phone = row.GetCell(8).ToString();
                                        else
                                            aspaccount.Phone = null;
                                        if (row.GetCell(9).ToString() != null)
                                            aspaccount.Email = row.GetCell(9).ToString();
                                        else
                                            aspaccount.Email = null;
                                        aspaccount.Password = Guid.NewGuid().ToString("N").Substring(0, 10);
                                        aspaccount.Photo = "alt.png";

                                        IRow returnRow = writeworksheet.CreateRow(i + 1);

                                        returnRow.CreateCell(0).SetCellValue($"{aspaccount.CNumber}");
                                        returnRow.CreateCell(1).SetCellValue($"{aspaccount.Login}");
                                        returnRow.CreateCell(2).SetCellValue($"{aspaccount.Password}");

                                        aspaccount.Password = GetHashCode(aspaccount.Password, aspaccount.Login);

                                        await _admmngr.AddAsp(aspaccount);
                                    }
                                    catch (Exception ex)
                                    {
                                        IRow returnRow = writeworksheet.CreateRow(i);
                                        returnRow.CreateCell(0).SetCellValue($"Строка {i} из входного файла");
                                        returnRow.CreateCell(1).SetCellValue("Не добавлена");
                                        returnRow.CreateCell(2).SetCellValue($"{ex.Message}");
                                    }

                                }

                                using (var memoryStream = new MemoryStream())
                                {
                                    writepackage.Write(memoryStream);

                                    await _accmngr.UpdateContext();

                                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Новые аспиранты.xlsx");
                                }
                            }
                        }
                    }
                }
                return RedirectToAction("Asps", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddResult(IFormFile fileupload)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (fileupload != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        await fileupload.CopyToAsync(stream);
                        stream.Position = 0;
                        using (var readpackage = new XSSFWorkbook(stream))
                        {
                            ISheet readworksheet = readpackage.GetSheetAt(0);
                            using (var writepackage = new XSSFWorkbook())
                            {
                                writepackage.CreateSheet("Лист 1");
                                ISheet writeworksheet = writepackage.GetSheetAt(0);

                                IRow headerRow = writeworksheet.CreateRow(0);
                                headerRow.CreateCell(0).SetCellValue("Номер удостоверения");
                                headerRow.CreateCell(1).SetCellValue("Ошибка");

                                for (int i = 1; i <= readworksheet.LastRowNum; i++)
                                {
                                    try
                                    {
                                        Result result = new Result();
                                        IRow row = readworksheet.GetRow(i);

                                        var aspaccount = _admmngr.GetAspPassByC(row.GetCell(0).ToString());
                                        result.UserID = aspaccount.Id;
                                        result.SubjName = row.GetCell(1).ToString();
                                        result.Grade = row.GetCell(2).ToString();
                                        if (row.GetCell(3).ToString() == "Академический")
                                            result.Type = "1";
                                        else
                                            result.Type = "2";
                                        result.Date = DateOnly.FromDateTime(DateTime.Now.Date);

                                        await _admmngr.AddResult(result);
                                    }
                                    catch (Exception ex)
                                    {
                                        IRow returnRow = writeworksheet.CreateRow(i);
                                        returnRow.CreateCell(0).SetCellValue($"Строка {i + 1} из входного файла не добавлена");
                                        returnRow.CreateCell(0).SetCellValue($"{ex.Message}");
                                    }
                                }

                                using (var memoryStream = new MemoryStream())
                                {
                                    writepackage.Write(memoryStream);

                                    await _accmngr.UpdateContext();

                                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Новые результаты - ошибки.xlsx");
                                }
                            }

                        }
                    }
                }
            }
            return RedirectToAction("Asps", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DelOneAsp(string aspid)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (aspid != null)
                {
                    var aspaccount = _admmngr.GetAspPassByC(aspid);
                    if (aspaccount != null)
                    {
                        await RemoveAspCookie(aspaccount.Login);
                        await _admmngr.RemoveAsp(aspaccount);
                        await _accmngr.UpdateContext();
                        TempData["Message"] = 1;
                    }
                    else
                        TempData["Message"] = 2;
                }
                else
                    TempData["Message"] = 2;

                return RedirectToAction("Asps", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DelAspDiap(string firstasp, string lastasp)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (firstasp != null && lastasp != null)
                {
                    int first = Convert.ToInt32(firstasp.Replace("/", ""));
                    int last = Convert.ToInt32(lastasp.Replace("/", ""));

                    for (int i = first; i <= last; i++)
                    {
                        string cnumber = i.ToString().Substring(0, 4) + "/" + i.ToString().Substring(4);
                        var aspaccount = _admmngr.GetAspPassByC(cnumber);
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

                            await RemoveAspCookie(account.Login);
                            await _admmngr.RemoveAsp(aspaccount);
                            TempData["Message"] = 1;
                        }
                        else
                            TempData["Message"] = 2;
                    }
                    await _accmngr.UpdateContext();
                }
                else
                    TempData["Message"] = 2;

                return RedirectToAction("Asps", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DelAllTimet()
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                var timetables = _admmngr.GetTimetable();
                if (timetables.Count != 0)
                {
                    foreach (var tt in timetables)
                    {
                        string prevPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/timetables/", tt.File);

                        try
                        {
                            FileInfo fileInfo = new FileInfo(prevPath);
                            if (fileInfo.Exists)
                                fileInfo.Delete();
                        }
                        catch (Exception ex)
                        { }
                        await _admmngr.RemoveTimetable(tt);
                    }

                    TempData["Message"] = 1;
                    await _accmngr.UpdateContext();
                }
                else 
                    TempData["Message"] = 2;
                return RedirectToAction("Timetable", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DelCandTimet()
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                var timetables = _admmngr.GetCandidateTimetable();
                if (timetables.Count != 0)
                {
                    foreach (var tt in timetables)
                    {
                        string prevPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/timetables/", tt.File);

                        try
                        {
                            FileInfo fileInfo = new FileInfo(prevPath);
                            if (fileInfo.Exists)
                                fileInfo.Delete();
                        }
                        catch (Exception ex)
                        { }
                        await _admmngr.RemoveTimetable(tt);
                    }

                    TempData["Message"] = 1;
                    await _accmngr.UpdateContext();
                }
                else
                    TempData["Message"] = 2;
                return RedirectToAction("Timetable", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTimetForGroup(IFormFile fileupload)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (fileupload != null)
                {
                    Timetable tt = new Timetable();

                    var fileExtension = Path.GetExtension(fileupload.FileName).ToLower();
                    var newname = Convert.ToString(Guid.NewGuid());
                    var newFileName = newname + fileExtension;
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/timetables/");
                    string fileNameWithPath = Path.Combine(path, newFileName);
                    using (var fileStream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        fileupload.CopyToAsync(fileStream);
                    }
                    try
                    {
                        tt.Name = Path.GetFileNameWithoutExtension(fileupload.FileName);
                        tt.File = newFileName;
                        tt.Date = DateOnly.FromDateTime(DateTime.Now);
                        tt.Type = "1";

                        await _admmngr.AddTimetable(tt);
                        await _accmngr.UpdateContext();
                        TempData["Message"] = 1;
                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = 2;
                    }

                }
                else
                    TempData["Message"] = 2;
                return RedirectToAction("Timetable", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTimetForProf(IFormFile fileupload)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (fileupload != null)
                {
                    Timetable tt = new Timetable();

                    var fileExtension = Path.GetExtension(fileupload.FileName).ToLower();
                    var newname = Convert.ToString(Guid.NewGuid());
                    var newFileName = newname + fileExtension;
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/timetables/");
                    string fileNameWithPath = Path.Combine(path, newFileName);
                    using (var fileStream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        fileupload.CopyToAsync(fileStream);
                    }

                    try
                    {
                        tt.Name = Path.GetFileNameWithoutExtension(fileupload.FileName);
                        tt.File = newFileName;
                        tt.Date = DateOnly.FromDateTime(DateTime.Now);
                        tt.Type = "2";

                        await _admmngr.AddTimetable(tt);
                        await _accmngr.UpdateContext();
                        TempData["Message"] = 1;
                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = 2;
                    }

                }
                else
                    TempData["Message"] = 2;
                return RedirectToAction("Timetable", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTimetExam(IFormFile fileupload)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (fileupload != null)
                {
                    Timetable tt = new Timetable();

                    var fileExtension = Path.GetExtension(fileupload.FileName).ToLower();
                    var newname = Convert.ToString(Guid.NewGuid());
                    var newFileName = newname + fileExtension;
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/timetables/");
                    string fileNameWithPath = Path.Combine(path, newFileName);
                    using (var fileStream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        fileupload.CopyToAsync(fileStream);
                    }

                    try
                    {
                        tt.Name = Path.GetFileNameWithoutExtension(fileupload.FileName);
                        tt.File = newFileName;
                        tt.Date = DateOnly.FromDateTime(DateTime.Now);
                        tt.Type = "2";

                        await _admmngr.AddTimetable(tt);
                        await _accmngr.UpdateContext();
                        TempData["Message"] = 1;
                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = 2;
                    }

                }
                else
                    TempData["Message"] = 2;
                return RedirectToAction("Timetable", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTimetCandidate(IFormFile fileupload)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (fileupload != null)
                {
                    Timetable tt = new Timetable();

                    var fileExtension = Path.GetExtension(fileupload.FileName).ToLower();
                    var newname = Convert.ToString(Guid.NewGuid());
                    var newFileName = newname + fileExtension;
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/timetables/");
                    string fileNameWithPath = Path.Combine(path, newFileName);
                    using (var fileStream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        fileupload.CopyToAsync(fileStream);
                    }

                    try
                    {
                        tt.Name = Path.GetFileNameWithoutExtension(fileupload.FileName);
                        tt.File = newFileName;
                        tt.Date = DateOnly.FromDateTime(DateTime.Now);
                        tt.Type = "3";

                        await _admmngr.AddTimetable(tt);
                        await _accmngr.UpdateContext();
                        TempData["Message"] = 1;
                    }
                    catch (Exception ex)
                    {
                        TempData["Message"] = 2;
                    }

                }
                else
                    TempData["Message"] = 2;
                return RedirectToAction("Timetable", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAdmin(IFormFile fileupload)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (fileupload != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        await fileupload.CopyToAsync(stream);
                        stream.Position = 0;

                        using (var readpackage = new XSSFWorkbook(stream))
                        {
                            ISheet readworksheet = readpackage.GetSheetAt(0);
                            using (var writepackage = new XSSFWorkbook())
                            {
                                writepackage.CreateSheet("Лист 1");
                                ISheet writeworksheet = writepackage.GetSheetAt(0);

                                IRow headerRow = writeworksheet.CreateRow(0);
                                headerRow.CreateCell(0).SetCellValue("ФИО");
                                headerRow.CreateCell(1).SetCellValue("Логин");
                                headerRow.CreateCell(2).SetCellValue("Пароль");


                                for (int i = 1; i <= readworksheet.LastRowNum; i++)
                                {
                                    IRow returnRow = writeworksheet.CreateRow(i);

                                    try
                                    {
                                        Admin admaccount = new Admin();
                                        IRow row = readworksheet.GetRow(i);

                                        admaccount.Surname = row.GetCell(0).ToString();
                                        admaccount.Name = row.GetCell(1).ToString();
                                        admaccount.Patronymic = row.GetCell(2).ToString();
                                        admaccount.Email = row.GetCell(3).ToString();
                                        admaccount.Login = admaccount.Email;
                                        admaccount.Phone = row.GetCell(4).ToString();
                                        admaccount.Function = row.GetCell(5).ToString();
                                        admaccount.Cabinet = row.GetCell(6).ToString();
                                        admaccount.Password = Guid.NewGuid().ToString("N").Substring(0, 8);
                                        admaccount.Photo = "alt.png";
                                        admaccount.Role = "1";

                                        returnRow.CreateCell(0).SetCellValue($"{admaccount.Surname} {admaccount.Name} {admaccount.Patronymic}");
                                        returnRow.CreateCell(1).SetCellValue($"{admaccount.Login}");
                                        returnRow.CreateCell(2).SetCellValue($"{admaccount.Password}");

                                        admaccount.Password = GetHashCode(admaccount.Password, admaccount.Login);

                                        await _admmngr.AddAdmin(admaccount);

                                        string message = $"Администратор {admaccount.Login} - {admaccount.Surname} {admaccount.Name} добавлен";
                                        await _admmngr.SendInfo("Добавлен администратор", message);
                                    }
                                    catch (Exception ex)
                                    {
                                        returnRow.CreateCell(0).SetCellValue($"Строка {i + 1} из входного файла не добавлена");
                                        returnRow.CreateCell(1).SetCellValue($"{ex.Message}");
                                    }
                                }

                                using (var memoryStream = new MemoryStream())
                                {
                                    writepackage.Write(memoryStream);

                                    await _accmngr.UpdateContext();

                                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Новые сотрудники.xlsx");
                                }
                            }
                        }
                    }
                }
                return RedirectToAction("Contacts", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAdmin(string adminemail)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (adminemail != null)
                {
                    await RemoveAdmCookie(adminemail);
                    await _admmngr.RemoveAdmin(adminemail);
                    TempData["Message"] = 1;
                }
                else
                    TempData["Message"] = 2;

                return RedirectToAction("Contacts", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDirector(string function, string name, string phone, IFormFile photo)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                try
                {
                    string jsonPath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/director.json");
                    string jsonString1 = System.IO.File.ReadAllText(jsonPath1);
                    var jsonText = JsonSerializer.Deserialize<DirectorObject>(jsonString1);
                    string filename = jsonText.Photo;

                    if (photo != null)
                    {
                        string prevPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Avatars/", filename);

                        FileInfo fileInfo = new FileInfo(prevPath);
                        if (fileInfo.Exists)
                            fileInfo.Delete();


                        var fileExtension = Path.GetExtension(photo.FileName).ToLower();
                        filename = "director" + fileExtension;

                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Avatars/");
                        string fileNameWithPath = Path.Combine(path, filename);

                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }
                    }

                    var Director = new DirectorObject
                    {
                        Function = function,
                        Name = name,
                        Phone = phone,
                        Photo = filename
                    };

                    string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/director.json");
                    string jsonString = JsonSerializer.Serialize(Director, new JsonSerializerOptions { WriteIndented = true });
                    System.IO.File.WriteAllText(jsonPath, jsonString);
                    TempData["Message"] = 1;
                }
                catch (Exception ex)
                {
                    TempData["Message"] = 2;
                }

                return RedirectToAction("Contacts", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetAspPass(string cnumber)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (cnumber != null)
                {
                    var asp = _admmngr.GetAspPassByC(cnumber);
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
                            var pass = Guid.NewGuid().ToString("N").Substring(0, 10);
                            asp.Password = GetHashCode(pass, asp.Login);

                            IRow returnRow = writeworksheet.CreateRow(1);
                            returnRow.CreateCell(0).SetCellValue($"{cnumber}");
                            returnRow.CreateCell(1).SetCellValue($"{asp.Login}");
                            returnRow.CreateCell(2).SetCellValue($"{pass}");

                            await _accmngr.UpdateContext();
                        }
                        catch (Exception ex)
                        {
                            IRow returnRow = writeworksheet.CreateRow(1);
                            returnRow.CreateCell(0).SetCellValue($"Удостоверению {cnumber}");
                            returnRow.CreateCell(1).SetCellValue("Не сброшен пароль");
                            returnRow.CreateCell(2).SetCellValue($"{ex.Message}");
                        }

                        using (var memoryStream = new MemoryStream())
                        {
                            writepackage.Write(memoryStream);

                            return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Сброс пароля аспиранту.xlsx");
                        }
                    }
                }
                else
                {
                    TempData["Message"] = 2;
                    return RedirectToAction("Asps", "Admin");
                }
            }
        }

        public async Task<IActionResult> DownloadAllAsps()
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                var asps = _admmngr.GetAsps();

                using (var writepackage = new XSSFWorkbook())
                {
                    writepackage.CreateSheet("Лист 1");
                    ISheet writeworksheet = writepackage.GetSheetAt(0);

                    IRow headerRow = writeworksheet.CreateRow(0);
                    headerRow.CreateCell(0).SetCellValue("Номер удостоверения");
                    headerRow.CreateCell(1).SetCellValue("ФИО");
                    headerRow.CreateCell(2).SetCellValue("Номер группы");
                    headerRow.CreateCell(3).SetCellValue("Направление подготовки");
                    headerRow.CreateCell(4).SetCellValue("Профиль подготовки");
                    headerRow.CreateCell(5).SetCellValue("Тема работы");
                    headerRow.CreateCell(6).SetCellValue("Телефон");
                    headerRow.CreateCell(7).SetCellValue("Почта");

                    int i = 1;
                    foreach (var asp in asps)
                    {
                        IRow returnRow = writeworksheet.CreateRow(i);
                        try
                        {

                            returnRow.CreateCell(0).SetCellValue($"{asp.CNumber}");
                            returnRow.CreateCell(1).SetCellValue($"{asp.Name} {asp.Surname} {asp.Patronymic}");
                            returnRow.CreateCell(2).SetCellValue($"{asp.GNumber}");
                            returnRow.CreateCell(3).SetCellValue($"{asp.StDirection}");
                            returnRow.CreateCell(4).SetCellValue($"{asp.ProfDirection}");
                            returnRow.CreateCell(5).SetCellValue($"{asp.WTheme}");
                            returnRow.CreateCell(6).SetCellValue($"{asp.Phone}");
                            returnRow.CreateCell(7).SetCellValue($"{asp.Email}");

                            await _accmngr.UpdateContext();
                        }
                        catch (Exception ex)
                        {
                        }
                        i++;
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        writepackage.Write(memoryStream);

                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Выгрузка всех аспирантов.xlsx");
                    }
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendToAcademic(string cnumber, string firstDate, string secondDate)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                var asp = _admmngr.GetAspPassByC(cnumber);
                if (asp != null)
                {
                    var start = DateOnly.FromDateTime(Convert.ToDateTime(firstDate));
                    var end = DateOnly.FromDateTime(Convert.ToDateTime(secondDate));
                    asp.GNumber = $"В академическом отпуске с {start} по {end}";
                    await _accmngr.UpdateContext();
                    TempData["Message"] = 1;
                }
                else
                    TempData["Message"] = 2;
                return RedirectToAction("Asps", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DownloadAchievments(string cnumber)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                var asp = _admmngr.GetAspPassByC(cnumber);
                if (asp != null)
                {
                    string sourceFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/achievments/");
                    string zipFileName = $"Достижения {asp.Surname}.zip";
                    string zipFilePath = Path.Combine(Path.GetTempPath(), zipFileName);

                    var files = _admmngr.GetAllAchievments();

                    using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                    {
                        List<string> filesToAdd = new List<string>();
                        foreach (var filename in files)
                        {
                            if (filename.UserID == asp.Id)
                                filesToAdd.Add(Path.Combine(sourceFolderPath, filename.File));
                        }

                        foreach (string fileToAdd in filesToAdd)
                        {
                            string entryName = Path.GetFileName(fileToAdd);
                            zipArchive.CreateEntryFromFile(fileToAdd, entryName);
                        }
                    }

                    byte[] fileBytes = System.IO.File.ReadAllBytes(zipFilePath);
                    System.IO.File.Delete(zipFilePath);

                    return File(fileBytes, "application/zip", zipFileName);
                }
                return RedirectToAction("Asps", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAllResults()
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                var res = _admmngr.GetResults();
                if (res != null)
                    TempData["Message"] = 1;
                else
                    TempData["Message"] = 2;
                foreach (var result in res)
                {
                    if (result.Type != "2")
                        await _admmngr.RemoveResById(result.Id);
                }

                return RedirectToAction("Asps", "Admin");
            }

        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsp(string FIO, string cnumber, string gnumber, string stDir, string stProf, string theme, string scientist, string tel, string mail, string id)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
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
                    TempData["Message"] = 1;
                }
                catch (Exception ex)
                {
                    TempData["Message"] = 2;
                }
                return RedirectToAction("Asps", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string id, string lvl)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                var admin = _admmngr.GetAdminById(Convert.ToInt32(id));
                if (admin != null)
                {
                    admin.Role = lvl;
                    await _accmngr.UpdateContext();
                    TempData["Message"] = 1;
                }
                else
                    TempData["Message"] = 2;
                return RedirectToAction("Contacts", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddScientist(IFormFile file, string name, string department, string degree, string mail, string tel)
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(department) && !string.IsNullOrEmpty(degree) && !string.IsNullOrEmpty(mail) && !string.IsNullOrEmpty(tel))
                {
                    Scientist science = new Scientist();
                    science.Name = name;
                    science.Degree = degree;
                    science.Department = department;
                    science.Phone = tel;
                    science.Email = mail;

                    await _admmngr.AddScientist(science);

                    await _accmngr.UpdateContext();
                    TempData["Message"] = 1;
                }
                if (file != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        stream.Position = 0;

                        using (var readpackage = new XSSFWorkbook(stream))
                        {
                            ISheet readworksheet = readpackage.GetSheetAt(0);
                            using (var writepackage = new XSSFWorkbook())
                            {
                                writepackage.CreateSheet("Лист 1");
                                ISheet writeworksheet = writepackage.GetSheetAt(0);

                                IRow headerRow = writeworksheet.CreateRow(0);
                                headerRow.CreateCell(0).SetCellValue("Номер удостоверения");
                                headerRow.CreateCell(1).SetCellValue("Логин");
                                headerRow.CreateCell(2).SetCellValue("Пароль");


                                for (int i = 1; i <= readworksheet.LastRowNum; i++)
                                {
                                    try
                                    {
                                        Scientist science = new Scientist();
                                        IRow row = readworksheet.GetRow(i);

                                        science.Name = row.GetCell(0).ToString();
                                        science.Degree = row.GetCell(1).ToString();
                                        science.Department = row.GetCell(2).ToString();
                                        science.Phone = row.GetCell(3).ToString();
                                        science.Email = row.GetCell(4).ToString();

                                        await _admmngr.AddScientist(science);
                                    }
                                    catch (Exception ex)
                                    {
                                        IRow returnRow = writeworksheet.CreateRow(i);
                                        returnRow.CreateCell(0).SetCellValue($"Строка {i} из входного файла");
                                        returnRow.CreateCell(1).SetCellValue("Не добавлена");
                                        returnRow.CreateCell(2).SetCellValue($"{ex.Message}");
                                    }

                                }

                                using (var memoryStream = new MemoryStream())
                                {
                                    writepackage.Write(memoryStream);

                                    await _accmngr.UpdateContext();

                                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Науч. рук-и ошибки.xlsx");
                                }
                            }
                        }
                    }
                }

                return RedirectToAction("Contacts", "Admin");
            }
        }

        public async Task<IActionResult> GetResultsExcel()
        {
            var account = _admmngr.GetAdmin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("AdminCookieScheme");
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                using (var writepackage = new XSSFWorkbook())
                {
                    writepackage.CreateSheet("Лист 1");
                    ISheet writeworksheet = writepackage.GetSheetAt(0);

                    IRow headerRow = writeworksheet.CreateRow(0);
                    headerRow.CreateCell(0).SetCellValue("Предмет");
                    headerRow.CreateCell(1).SetCellValue("Оценка");
                    headerRow.CreateCell(2).SetCellValue("Тип");

                    var results = _admmngr.GetResults();
                    int i = 1;
                    foreach (var res in results.Where(r => r.Type == "1"))
                    {
                        try
                        {
                            IRow returnRow = writeworksheet.CreateRow(i);

                            returnRow.CreateCell(0).SetCellValue($"{res.SubjName}");
                            returnRow.CreateCell(1).SetCellValue($"{res.Grade}");
                            returnRow.CreateCell(2).SetCellValue("Академический");
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            i++;
                        }
                    }
                    foreach (var res in results.Where(r => r.Type == "2"))
                    {
                        try
                        {
                            IRow returnRow = writeworksheet.CreateRow(i);

                            returnRow.CreateCell(0).SetCellValue($"{res.SubjName}");
                            returnRow.CreateCell(1).SetCellValue($"{res.Grade}");
                            returnRow.CreateCell(2).SetCellValue("Кандидатский");
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            i++;
                        }
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        writepackage.Write(memoryStream);

                        await _accmngr.UpdateContext();

                        return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Выгрузка результатов.xlsx");
                    }
                }
            }
        }

        public async Task<IActionResult> PM_Accept(int id)
        {
            await _filemngr.AcceptPhoto(id);
            return RedirectToAction("PhotoManagement", "Admin");
        }
        public async Task<IActionResult> PM_Reject(int id)
        {
            await _filemngr.RejectPhoto(id);
            return RedirectToAction("PhotoManagement", "Admin");
        }


        /*Private system actions*/
        


    }
}