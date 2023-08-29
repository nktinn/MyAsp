using System.Net;
using System.Net.Mail;
using MyAsp.Storage;

namespace MyAsp.Logic.Accounts
{
    public class AdmAccountManager : IAdmAccountManager
    {
        private readonly Context _context;
        public AdmAccountManager(Context context)
        {
            _context = context;
        }

        #region "Администраторы"
        public Admin? GetAdmin(string login)
        {
            return _context.Admins.FirstOrDefault(admin => admin.Login == login);
        }
        public Admin? GetAdminById(int id)
        {
            return _context.Admins.FirstOrDefault(admin => admin.Id == id);
        }
        public List<Admin>? GetAdmins()
        {
            return _context.Admins.ToList();
        }
        public Admin? GetAdmPass(string login)
        {
            return _context.Admins.FirstOrDefault(admin => admin.Login == login);
        }
        public async Task AddAdmin(Admin admin)
        {
            await _context.Admins.AddAsync(admin);
        }
        public async Task RemoveAdmin(string email)
        {
            var admin = _context.Admins.FirstOrDefault(admin => admin.Email == email);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
                await _context.SaveChangesAsync();
            }
            else 
                await Task.Delay(0);
        }
        public async Task RemoveAdminById(int id)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.Id == id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
                await _context.SaveChangesAsync();
            }
            else
                await Task.Delay(0);
        }
        #endregion

        #region "Стипендии"
        public async Task AddGrant(Grant grant)
        {
            await _context.Grants.AddAsync(grant);
        }
        public List<Grant>? GetGrants()
        {
            return _context.Grants.ToList();
        }
        public async Task RemoveGrantById(int id) 
        {
            var grant = _context.Grants.FirstOrDefault(g => g.Id == id);
            if (grant != null)
            {
                _context.Grants.Remove(grant);
                await _context.SaveChangesAsync();
            }
            else
                await Task.Delay(0);
        }
        public Grant? GetGrantById(int id)
        {
            return _context.Grants.FirstOrDefault(g => g.Id == id);
        }
        #endregion

        #region "Документы"
        public async Task AddDoc(Document doc)
        {
            await _context.Documents.AddAsync(doc);
        }
        public List<Document>? GetDocs()
        {
            return _context.Documents.ToList();
        }
        public async Task RemoveDocById(int id)
        {
            var doc = _context.Documents.FirstOrDefault(d => d.Id == id);
            if (doc != null)
            {
                _context.Documents.Remove(doc);
                await _context.SaveChangesAsync();
            }
            else
                await Task.Delay(0);
        }
        public Document? GetDocById(int id)
        {
            return _context.Documents.FirstOrDefault(d => d.Id == id);
        }
        #endregion

        #region "Достижения"
        public List<Achievment> GetAchievments(User account)
        {
            return _context.Achievments.Where(a => a.UserID == account.Id).ToList();
        }
        public List<Achievment> GetAllAchievments()
        {
            return _context.Achievments.ToList();
        }
        public async Task RemoveAchievById(int id)
        {
            var achiev = _context.Achievments.FirstOrDefault(a => a.Id == id);
            if (achiev != null)
            {
                string prevPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/achievments/", achiev.File);

                try
                {
                    FileInfo fileInfo = new FileInfo(prevPath);
                    if (fileInfo.Exists)
                        fileInfo.Delete();
                }
                catch (Exception ex)
                { }
                _context.Achievments.Remove(achiev);
                await _context.SaveChangesAsync();
            }
            else
                await Task.Delay(0);
        }
        public Achievment? GetAchievById(int id)
        {
            return _context.Achievments.FirstOrDefault(a => a.Id == id);
        }
        #endregion

        #region "Аспиранты"
        public User? GetAsp(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
        public User? GetAspPass(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
        public User? GetAspPassByC(string cnumber)
        {
            return _context.Users.FirstOrDefault(u => u.CNumber == cnumber);
        }
        public List<User>? GetAsps()
        {
            return _context.Users.ToList();
        }
        public async Task AddAsp(User asp)
        {
            await _context.Users.AddAsync(asp);
        }
        public async Task RemoveAsp(User asp)
        {
            _context.Users.Remove(asp);
            await Task.Delay(0);
        }
        public async Task RemoveAspById(int id)
        {
            var asp = _context.Users.FirstOrDefault(u => u.Id == id);
            if (asp != null)
            {
                _context.Users.Remove(asp);
                await _context.SaveChangesAsync();
            }
            else
                await Task.Delay(0);

        }
        #endregion

        #region "Результаты экзменов и др."
        public async Task AddResult(Result result)
        {
            await _context.Results.AddAsync(result);
        }

        public List<Result>? GetResults()
        {
            return _context.Results.ToList();
        }
        public async Task RemoveResById(int id)
        {
            var res = _context.Results.FirstOrDefault(r => r.Id == id);
            if (res != null)
            {
                _context.Results.Remove(res);
                await _context.SaveChangesAsync();
            }
            else
                await Task.Delay(0);
        }
        public Result? GetResById(int id)
        {
            return _context.Results.FirstOrDefault(r => r.Id == id);
        }
        #endregion

        #region "Расписания"
        public List<Timetable> GetTimetable()
        {
            return _context.Timetables.Where(t => t.Type == "1" || t.Type == "2").ToList();
        }
        public List<Timetable> GetCandidateTimetable()
        {
            return _context.Timetables.Where(t => t.Type == "3").ToList();
        }
        public async Task RemoveTimetable(Timetable tt)
        {
            _context.Timetables.Remove(tt);
            await Task.Delay(0);
        }
        public async Task AddTimetable(Timetable tt)
        {
            var timet = _context.Timetables.Where(t => t.Name == tt.Name && t.Type == tt.Type).FirstOrDefault();
            if (timet != null)
            {
                string prevPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/timetables/", timet.File);

                try
                {
                    FileInfo fileInfo = new FileInfo(prevPath);
                    if (fileInfo.Exists)
                        fileInfo.Delete();
                }
                catch (Exception ex)
                { }
                _context.Timetables.Remove(timet);
            }
            await _context.Timetables.AddAsync(tt);
        }
        public List<Timetable>? GetTimetables()
        {
            return _context.Timetables.ToList();
        }
        public async Task RemoveTimetById(int id)
        {
            var timet = _context.Timetables.FirstOrDefault(t => t.Id == id);
            if (timet != null)
            {
                string prevPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/timetables/", timet.File);

                try
                {
                    FileInfo fileInfo = new FileInfo(prevPath);
                    if (fileInfo.Exists)
                        fileInfo.Delete();
                }
                catch (Exception ex)
                { }
                _context.Timetables.Remove(timet);
                await _context.SaveChangesAsync();
            }
            else
                await Task.Delay(0);
        }
        public Timetable? GetTimetById(int id)
        {
            return _context.Timetables.FirstOrDefault(t => t.Id == id);
        }
        #endregion

        #region "Менеджер Фото"
        public PhotoManagement? GetLastPM()
        {
            return _context.PM.FirstOrDefault();
        }
        public int GetPMCount()
        {
            return _context.PM.Count();
        }
        public List<PhotoManagement>? GetAllPM()
        {
            return _context.PM.ToList();
        }
        public async Task RemovePMById(int id)
        {
            var pm = _context.PM.FirstOrDefault(pm => pm.Id == id);
            if (pm != null)
            {
                string prevPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Avatars/", pm.File);

                try
                {
                    FileInfo fileInfo = new FileInfo(prevPath);
                    if (fileInfo.Exists)
                        fileInfo.Delete();
                }
                catch (Exception ex)
                { }
                _context.PM.Remove(pm);
                await _context.SaveChangesAsync();
            }
            else
                await Task.Delay(0);
        }
        public PhotoManagement? GetPMById(int id)
        {
            return _context.PM.FirstOrDefault(pm => pm.Id == id);
        }
        #endregion

        #region "Науч. рук-и"
        public List<Scientist>? GetScientists()
        {
            return _context.Scientists.ToList();
        }
        public async Task RemoveScienceById(int id)
        {
            var science = _context.Scientists.FirstOrDefault(s => s.Id == id);
            if (science != null)
            {
                _context.Scientists.Remove(science);
                await _context.SaveChangesAsync();
            }
            else
                await Task.Delay(0);
        }
        public Scientist? GetScienceById(int id)
        {
            return _context.Scientists.FirstOrDefault(s => s.Id == id);
        }
        public Scientist? GetScienceByName(string name)
        {
            return _context.Scientists.FirstOrDefault(s => s.Name == name);
        }
        public async Task AddScientist(Scientist scientist)
        {
            await _context.Scientists.AddAsync(scientist);
        }
        #endregion

        #region "Направления подготовки"
        public List<Direction>? GetDirections()
        {
            return _context.Directions.ToList();
        }
        public List<Direction>? GetUniqDir()
        {
            return _context.Directions
            .GroupBy(direction => direction.Name)
            .Select(group => group.First()) // Выбрать первую запись из каждой группы
            .ToList();
        }
        public async Task RemoveDirectById(int id)
        {
            var direct = _context.Directions.FirstOrDefault(d => d.Id == id);
            if (direct != null)
            {
                _context.Directions.Remove(direct);
                await _context.SaveChangesAsync();
            }
            else
                await Task.Delay(0);
        }
        public Direction? GetDirectById(int id)
        {
            return _context.Directions.FirstOrDefault(d => d.Id == id);
        }
        public async Task AddDirection(Direction direction)
        {
            await _context.Directions.AddAsync(direction);
        }
        #endregion

        #region "Новости"
        public List<New>? GetNews()
        {
            return _context.News.ToList();
        }
        public async Task RemoveNewById(int id)
        {
            var news = _context.News.FirstOrDefault(n => n.Id == id);
            if (news != null)
            {
                _context.News.Remove(news);
                await _context.SaveChangesAsync();
            }
            else
                await Task.Delay(0);
        }
        public New? GetNewById(int id)
        {
            return _context.News.FirstOrDefault(n => n.Id == id);
        }
        #endregion

        #region "Notifications when offline"
        public async Task SendInfo(string theme, string message)
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

                try
                {
                    using (MailMessage mail = new MailMessage(new MailAddress(login, "Важное уведомление - Моя Аспирантура"), new MailAddress(emailTo)))
                    {
                        mail.Subject = theme;
                        mail.Body = $@"<p>{message}</p>";

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
                        }
                        catch (SmtpException ex)
                        {
                        }
                    }

                }
                catch (FormatException ex)
                {
                }
                catch (ArgumentException ex)
                {
                }

            }
            catch (FileNotFoundException)
            {
            }
            catch (Exception ex)
            {
            }
            
        }
        #endregion

        public bool SignIn(string login, string password)
        {
            return _context.Admins.FirstOrDefault(acc => acc.Login == login && acc.Password == password) != null;
        }
    }
}
