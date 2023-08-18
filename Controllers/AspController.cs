using MyAsp.Storage;
using MyAsp.Models;

using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using MyAsp.Storage.Entities;

using System.Drawing;
using System.Globalization;

using MyAsp.Logic.Accounts;
using MyAsp.Logic.Files;

using System.IO;
using System.Text.Json;

namespace MyAsp.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookieScheme")]
    public class AspController : Controller
    {
        private readonly IAccountManager _accmngr;
        private readonly IAspAccountManager _aspmngr;
        private readonly IAdmAccountManager _admmngr;
        private readonly IFileManager _filemngr;
        public AspController(IAccountManager accmngr, IFileManager filemngr, IAspAccountManager aspmngr, IAdmAccountManager admmngr)
        {
            _accmngr = accmngr;
            _admmngr = admmngr;
            _filemngr = filemngr;
            _aspmngr = aspmngr;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (User.Identity.Name != null)
            {
                if (_aspmngr.GetByLogin(User.Identity.Name) != null)
                    return RedirectToRoute(new { controller = "Asp", action = "Account" });
                else
                {
                    await HttpContext.SignOutAsync("UserCookieScheme");
                    return RedirectToAction("Login", "Asp");
                }
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var password = GetHashCode(model.Password, model.Login);
                var IsAccount = _aspmngr.SignIn(model.Login, password);
                if (IsAccount)
                {
                    await Authenticate(model.Login);
                    return RedirectToRoute(new { controller = "Asp", action = "Account" });
                }
                else
                {
                    var IsAccountTmp = _aspmngr.SignInTemp(model.Login, password);
                    if (IsAccountTmp)
                    {
                        await Authenticate(model.Login);
                        return RedirectToRoute(new { controller = "Asp", action = "Account" });
                    }
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Restore()
        {
            if (User.Identity.Name != null)
            {
                if (_aspmngr.GetByLogin(User.Identity.Name) != null)
                    return RedirectToRoute(new { controller = "Asp", action = "Account" });
                else
                {
                    await HttpContext.SignOutAsync("UserCookieScheme");
                    return RedirectToAction("Login", "Asp");
                }
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Restore(RequestModel model)
        {
            if (ModelState.IsValid)
            {
                var account = _aspmngr.GetByLogin(model.Login);
                if (account == null)
                {
                    var account2 = _aspmngr.GetByMail(model.Login);
                    if (account2 == null)
                    {
                        model.Sent = true;
                        ModelState.AddModelError("Login", "• Аккаунт не найден");
                    }
                    else
                    {
                        string tmpPass = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
                        var sent = await _aspmngr.SendPassword(account2, tmpPass);
                        if (sent)
                        {
                            await _aspmngr.SetTempPass(account2, GetHashCode(tmpPass, account2.Login));
                            model.Sent = false;
                            ModelState.AddModelError("Sent", "• Временный пароль отправлен на вашу почту");
                        }
                        else
                        {
                            model.Sent = true;
                            ModelState.AddModelError("Login", "• Невозможно сбросить пароль, повторите запрос позже");
                        }
                    }
                }
                else
                {
                    if (account.Email == null)
                    {
                        model.Sent = true;
                        ModelState.AddModelError("Login", "• Почта не привязана. Обратитесь в кабинет 219");
                    }
                    else
                    {
                        string tmpPass = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
                        var sent = await _aspmngr.SendPassword(account, tmpPass);
                        if (sent)
                        {
                            await _aspmngr.SetTempPass(account, GetHashCode(tmpPass, account.Login));
                            model.Sent = false;
                            ModelState.AddModelError("Sent", "• Временный пароль отправлен на вашу почту");
                        }
                        else
                        {
                            model.Sent = true;
                            ModelState.AddModelError("Login", "• Невозможно сбросить пароль, повторите запрос позже");
                        }
                    }
                }
            }
            else
            {
                model.Sent = true;
                ModelState.AddModelError("Login", "• Данные не введены");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Account()
        {
            var account = _aspmngr.GetByLogin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("UserCookieScheme");
                return RedirectToAction("Login", "Asp");
            }
            else
            {
                ViewBag.Account = account;
                ViewBag.PM = _aspmngr.OnModeration(account.Id);
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Education()
        {
            var account = _aspmngr.GetByLogin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("UserCookieScheme");
                return RedirectToAction("Login", "Asp");
            }
            else
            {
                ViewBag.Account = account;
                ViewBag.Results = _aspmngr.GetResults(account);
                ViewBag.Academic = _aspmngr.GetAcademic(account);
                ViewBag.Social = _aspmngr.GetSocial(account);
                ViewBag.President = _aspmngr.GetPresident(account);
                ViewBag.Government = _aspmngr.GetGovernment(account);
                ViewBag.Support = _aspmngr.GetSupport(account);
                await Task.Delay(0);
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Docs()
        {
            var account = _aspmngr.GetByLogin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("UserCookieScheme");
                return RedirectToAction("Login", "Asp");
            }
            else
            {
                ViewBag.Account = account;
                var docs = _aspmngr.GetDocuments(account);
                ViewBag.DocsThat = docs.Where(d => d.Date.Month == DateTime.Now.Month && d.Date.Year == DateTime.Now.Year).ToList();
                ViewBag.DocsLast = docs.Where(d => d.Date.Month == DateTime.Now.AddMonths(-1).Month && d.Date.Year == DateTime.Now.Year).ToList();
                ViewBag.DocsEarlier = docs.Where(d => (d.Date.Month != DateTime.Now.Month && d.Date.Year != DateTime.Now.Year) || (d.Date.Month != DateTime.Now.AddMonths(-1).Month && d.Date.Year != DateTime.Now.Year)).ToList();
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Achievments()
        {
            var account = _aspmngr.GetByLogin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("UserCookieScheme");
                return RedirectToAction("Login", "Asp");
            }
            else
            {
                ViewBag.Account = account;
                ViewBag.Docs = _aspmngr.GetAchievments(account);
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Timetable()
        {
            var account = _aspmngr.GetByLogin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("UserCookieScheme");
                return RedirectToAction("Login", "Asp");
            }
            else
            {
                ViewBag.Account = account;
                ViewBag.Pairs = _aspmngr.GetPairs(account);
                ViewBag.Exams = _aspmngr.GetExams(account);
                ViewBag.Candidate = _aspmngr.GetCandidate();
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> News()
        {
            var account = _aspmngr.GetByLogin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("UserCookieScheme");
                return RedirectToAction("Login", "Asp");
            }
            else
            {
                var ListOfNews = _aspmngr.GetNews();
                if (ListOfNews != null)
                {
                    ViewBag.Account = account;
                    ViewBag.New1 = ListOfNews[0];
                    ViewBag.New2 = ListOfNews[1];
                    ViewBag.New3 = ListOfNews[2];
                    ViewBag.New4 = ListOfNews[3];
                    ViewBag.New5 = ListOfNews[4];
                    ViewBag.New6 = ListOfNews[5];
                    return View();
                }
                else return RedirectToAction("Account", "Asp");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Contacts()
        {
            var account = _aspmngr.GetByLogin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("UserCookieScheme");
                return RedirectToAction("Login", "Asp");
            }
            else
            {
                try
                {

                    string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/director.json");
                    string jsonString = System.IO.File.ReadAllText(jsonPath);
                    ViewBag.Director = JsonSerializer.Deserialize<DirectorObject>(jsonString);

                    ViewBag.Account = account;
                    var admins = _aspmngr.GetAdmins();
                    ViewBag.Admins = admins.Select(a => new { Name = a.Name, Surname = a.Surname, Patronymic = a.Patronymic, Photo = a.Photo, Function = a.Function, Phone = a.Phone, Email = a.Email, Cabinet = a.Cabinet, Role = a.Role }).ToList();
                    return View();
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Account", "Asp");
                }
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("UserCookieScheme");
            return RedirectToAction("Login", "Asp");
        }



        /*View buttons and more*/

        [HttpPost]
        public async Task<IActionResult> ChangeInfo(string phone, string email)
        {
            var account = _aspmngr.GetByLogin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("UserCookieScheme");
                return RedirectToAction("Login", "Asp");
            }
            else
            {
                account.Phone = phone;
                account.Email = email;

                await _accmngr.UpdateContext();

                return RedirectToAction("Account", "Asp");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePass(string oldpass, string newpass, string newpass2)
        {
            var account = _aspmngr.GetByLogin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("UserCookieScheme");
                return RedirectToAction("Login", "Asp");
            }
            else
            {
                var hashstring = GetHashCode(oldpass, account.Login);
                if (hashstring == account.Password || hashstring == account.TmpPassword)
                {
                    if (newpass == newpass2)
                    {
                        account.Password = GetHashCode(newpass, account.Login);
                        await _accmngr.UpdateContext();
                    }
                }
                return RedirectToAction("Account", "Asp");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto(IFormFile fileupload, string filesize)
        {
            var account = _aspmngr.GetByLogin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("UserCookieScheme");
                return RedirectToAction("Login", "Asp");
            }
            else
            {
                if (fileupload != null)
                {
                    var aspaccount = _aspmngr.GetByLogin(User.Identity.Name);

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

                    await _filemngr.UpdatePhoto(aspaccount.Id, newFileName);
                    await _accmngr.UpdateContext();

                }
                return RedirectToAction("Account", "Asp");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto()
        {
            var account = _aspmngr.GetByLogin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("UserCookieScheme");
                return RedirectToAction("Login", "Asp");
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

                return RedirectToAction("Account", "Asp");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDoc(IFormFile fileupload, string filename)
        {
            var account = _aspmngr.GetByLogin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("UserCookieScheme");
                return RedirectToAction("Login", "Asp");
            }
            else
            {
                if (fileupload != null)
                {
                    Achievment achiev = new Achievment();
                    try
                    {
                        var aspaccount = _aspmngr.GetByLogin(User.Identity.Name);

                        var fileExtension = Path.GetExtension(fileupload.FileName).ToLower();
                        var newname = Convert.ToString(Guid.NewGuid());
                        var newFileName = newname + fileExtension;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/achievments/");
                        string fileNameWithPath = Path.Combine(path, newFileName);
                        using (var fileStream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            fileupload.CopyToAsync(fileStream);
                        }

                        if (filename != null)
                        {
                            achiev.Name = filename;
                        }
                        else
                        {
                            achiev.Name = "Документ";
                        }
                        achiev.File = newFileName;
                        achiev.Date = DateOnly.FromDateTime(DateTime.Now);
                        achiev.UserID = aspaccount.Id;

                        await _aspmngr.AddAchiev(achiev);
                    }
                    catch (Exception ex)
                    { 
                    }

                }
                return RedirectToAction("Achievments", "Asp");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDoc(string fileId)
        {
            var account = _aspmngr.GetByLogin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("UserCookieScheme");
                return RedirectToAction("Login", "Asp");
            }
            else
            {
                var file = _aspmngr.GetAchievmentByFile(fileId);
                if (file != null)
                {
                    string prevPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/achievments/", file.File);

                    try
                    {
                        FileInfo fileInfo = new FileInfo(prevPath);
                        if (fileInfo.Exists)
                            fileInfo.Delete();
                    }
                    catch (Exception ex)
                    { }
                }
                if (file != null)
                {
                    await _aspmngr.RemoveAchiev(file);
                }
                return RedirectToAction("Achievments", "Asp");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendFeedback(string theme, string message)
        {
            var account = _aspmngr.GetByLogin(User.Identity.Name);
            if (account == null)
            {
                await HttpContext.SignOutAsync("UserCookieScheme");
                return RedirectToAction("Login", "Asp");
            }
            else
            {
                if (message != null)
                {
                    await _aspmngr.SendFeedBack(account, theme, message);
                }
                return RedirectToAction("Contacts", "Asp");
            }
        }


        /*Private system actions*/
        


    }
}
