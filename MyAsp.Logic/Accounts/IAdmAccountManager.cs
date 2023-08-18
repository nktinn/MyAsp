namespace MyAsp.Logic.Accounts
{
    public interface IAdmAccountManager
    {
        #region "Администраторы"
        public Admin? GetAdmin(string login);
        public Admin? GetAdminById(int id);
        public List<Admin>? GetAdmins();
        public Admin? GetAdmPass(string login);
        public Task AddAdmin(Admin admin);
        public Task RemoveAdmin(string email);
        public Task RemoveAdminById(int id);
        #endregion

        #region "Стипендии"
        public Task AddGrant(Grant grant);
        public List<Grant>? GetGrants();
        public Task RemoveGrantById(int id);
        public Grant? GetGrantById(int id);
        #endregion

        #region "Документы"
        public Task AddDoc(Document doc);
        public List<Document>? GetDocs();
        public Task RemoveDocById(int id);
        public Document? GetDocById(int id);
        #endregion

        #region "Достижения"
        public List<Achievment> GetAchievments(User account);
        public List<Achievment> GetAllAchievments();
        public Task RemoveAchievById(int id);
        public Achievment? GetAchievById(int id);
        #endregion

        #region "Аспиранты"
        public User? GetAsp(int id);
        public User? GetAspPass(int id);
        public User? GetAspPassByC(string cnumber);
        public List<User>? GetAsps();
        public Task AddAsp(User asp);
        public Task RemoveAsp(User asp);
        public Task RemoveAspById(int id);
        #endregion

        #region "Результаты экзаменов и др."
        public Task AddResult(Result result);
        public List<Result>? GetResults();
        public Task RemoveResById(int id);
        public Result? GetResById(int id);
        #endregion

        #region "Расписания"
        public List<Timetable> GetTimetable();
        public List<Timetable> GetCandidateTimetable();
        public Task RemoveTimetable(Timetable tt);
        public Task AddTimetable(Timetable tt);
        public List<Timetable>? GetTimetables();
        public Task RemoveTimetById(int id);
        public Timetable? GetTimetById(int id);
        #endregion

        #region "Менеджер Фото"
        public PhotoManagement? GetLastPM();
        public int GetPMCount();
        public List<PhotoManagement>? GetAllPM();
        public Task RemovePMById(int id);
        public PhotoManagement? GetPMById(int id);
        #endregion

        #region "Науч. рук-и"
        public List<Scientist>? GetScientists();
        public Task RemoveScienceById(int id);
        public Task AddScientist(Scientist scientist);
        public Scientist? GetScienceById(int id);
        public Scientist? GetScienceByName(string name);
        #endregion

        #region "Направления подготовки"
        public List<Direction>? GetDirections();
        public Task RemoveDirectById(int id);
        public Task AddDirection(Direction direction);
        public Direction? GetDirectById(int id);
        #endregion

        #region "Новости"
        public List<New>? GetNews();
        public Task RemoveNewById(int id);
        public New? GetNewById(int id);
        #endregion

        #region "Notifications when offline"
        public Task SendInfo(string theme, string message);
        #endregion

        public bool SignIn(string login, string password);
    }
}
