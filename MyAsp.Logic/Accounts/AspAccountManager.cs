using System.Net;
using System.Net.Mail;
using MyAsp.Storage;

namespace MyAsp.Logic.Accounts
{
    public class AspAccountManager : IAspAccountManager
    {
        private readonly Context _context;
        public AspAccountManager(Context context)
        {
            _context = context;
        }

        #region "Запрос аккаунтов"
        public User? GetByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }
        public User? GetByMail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public List<Admin> GetAdmins()
        {
            return _context.Admins.Where(a => a.Login != "admin").ToList();
        }
        #endregion

        #region "Запрос стипендий"
        public List<Result> GetResults(User account)
        {
            return _context.Results.Where(res => res.UserID == account.Id).OrderBy(res => res.Type).ToList();
        }

        public Grant? GetAcademic(User account)
        {
            return _context.Grants.FirstOrDefault(g => g.UserID == account.Id && g.Name == "Академическая стипендия");
        }

        public Grant? GetSocial(User account)
        {
            return _context.Grants.FirstOrDefault(g => g.UserID == account.Id && g.Name == "Социальная стипендия");
        }

        public Grant? GetPresident(User account)
        {
            return _context.Grants.FirstOrDefault(g => g.UserID == account.Id && g.Name == "Президентская стипендия");
        }

        public Grant? GetGovernment(User account)
        {
            return _context.Grants.FirstOrDefault(g => g.UserID == account.Id && g.Name == "Правительственная стипендия");
        }

        public Grant? GetSupport(User account)
        {
            return _context.Grants.FirstOrDefault(g => g.UserID == account.Id && g.Name == "Материальная выплата");
        }
        #endregion

        #region "Запрос документов"
        public List<Document> GetDocuments(User account)
        {
            return _context.Documents.Where(d => d.UserID == account.Id).ToList();
        }
        #endregion

        #region "Запрос достижений"
        public Achievment? GetAchievmentByFile(string fileId)
        {
            return _context.Achievments.FirstOrDefault(f => f.Id == Convert.ToInt32(fileId));
        }

        public List<Achievment> GetAchievments(User account)
        {
            return _context.Achievments.Where(d => d.UserID == account.Id).ToList();
        }
        #endregion

        #region "Запрос расписаний"
        public List<Timetable> GetPairs(User account)
        {
            return _context.Timetables.Where(t => t.Type == "1" && t.Name.Contains(account.GNumber)).ToList();
        }
        public List<Timetable> GetExams(User account)
        {
            return _context.Timetables.Where(t => t.Type == "2" && t.Name.Contains(account.GNumber)).ToList();
        }
        public List<Timetable> GetCandidate()
        {
            return _context.Timetables.Where(t => t.Type == "3").ToList();
        }
        #endregion

        #region "Запрос научного рук."
        public Scientist? GetScienceById(int id)
        {
            return _context.Scientists.FirstOrDefault(s => s.Id == id);
        }
        #endregion

        #region "Достижения - общее"
        public async Task AddAchiev(Achievment achiev)
        {
            _context.Achievments.Add(achiev);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveAchiev(Achievment achiev)
        {
            _context.Achievments.Remove(achiev);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region "Новости"
        public List<New> GetNews()
        {
            return _context.News.ToList();
        }
        #endregion

        #region "Отправка писем на почту"
        public async Task<bool> SendPassword(User account, string tmpPass)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/mailAcc.txt");
            string login;
            string password;
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                login = lines[0];
                password = lines[1];
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            try
            {
                using (MailMessage mail = new MailMessage(new MailAddress(login, "Моя Аспирантура"), new MailAddress(account.Email)))
                {
                    mail.Subject = "Запрос на восстановление пароля";
                    mail.Body = $@"
                    <p>Здравствуйте, {account.Name}!</p>
                    <p>Мы получили запрос на восстановление пароля для вашей учетной записи в системе Моя Аспирантура.</p>


                    <p>Ваш временный пароль для восстановления доступа:</p>

                    <p><strong>{tmpPass}</strong></p>
                    
                    <p>Пароль действителен в течение 24 часов. Обязательно поменяйте пароль при следующем входе в систему.</p>


                    <p>Если вы не запрашивали сброс пароля, пожалуйста, проигнорируйте это сообщение. Возможно, кто-то указал ваши данные по ошибке.</p>
                    <p>Вы всё еще можете войти в систему, используя свой старый пароль.</p>

                    <p>С уважением,</p>
                    <p>Центр подготовки научно-педагогических кадров в аспирантуре</p>";

                    mail.IsBodyHtml = true;
                    try
                    {
                        using (SmtpClient smtp = new SmtpClient("smtp.rambler.ru", 587))
                        {
                            smtp.Credentials = new NetworkCredential(login, password);
                            smtp.Send(mail);
                            await Task.Delay(0);
                        }
                    }
                    catch (IOException ex)
                    {
                        return false;
                    }
                    catch (SmtpException ex)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (FormatException ex)
            {
                return false;
            }
            catch (ArgumentException ex)
            {
                return false;
            }
        }

        public async Task SetTempPass(User account, string pass) 
        {
            account.TmpPassword = pass;
            account.RequestTime = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        public async Task<bool> SendFeedBack(User account, string theme, string message)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/mailAcc.txt");
            string login;
            string password;
            string filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/feedbackAcc.txt");
            string emailTo;
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                login = lines[0];
                password = lines[1];
                string[] lines2 = File.ReadAllLines(filePath2);
                emailTo = lines2[0];
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
            try
            {
                using (MailMessage mail = new MailMessage(new MailAddress(login, "Обратная связь - Моя Аспирантура"), new MailAddress(emailTo)))
                {
                    mail.Subject = theme;
                    mail.Body = $@"
                    <p>{account.Name} {account.Surname}</p>
                    <p>Номер удостоверения {account.CNumber}</p>
                    <p></p>
                    <p>{message}</p>";

                    mail.IsBodyHtml = true;
                    try
                    {
                        using (SmtpClient smtp = new SmtpClient("smtp.rambler.ru", 587))
                        {
                            smtp.Credentials = new NetworkCredential(login, password);
                            smtp.Send(mail);
                            await Task.Delay(0);
                        }
                    }
                    catch (IOException ex)
                    {
                        return false;
                    }
                    catch (SmtpException ex)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (FormatException ex)
            {
                return false;
            }
            catch (ArgumentException ex)
            {
                return false;
            }
        }
        #endregion

        #region "Авторизация"
        public bool SignIn(string login, string password)
        {
            return _context.Users.FirstOrDefault(acc => acc.Login == login && acc.Password == password) != null;
        }
        public bool SignInTemp(string login, string password)
        {
            return _context.Users.FirstOrDefault(acc => acc.Login == login && acc.TmpPassword == password) != null;
        }
        #endregion

        #region "Менеджер фотографий"
        public bool OnModeration(int id)
        {
            return _context.PM.FirstOrDefault(pm => pm.UserID == id) != null;
        }
        #endregion

    }
}
