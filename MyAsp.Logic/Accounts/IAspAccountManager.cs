namespace MyAsp.Logic.Accounts
{
    public interface IAspAccountManager
    {
        #region "Запрос аккаунтов"
        public User? GetByLogin(string login);
        public User? GetByMail(string email);
        public List<Admin> GetAdmins();
        #endregion

        #region "Запрос стипендий"
        public List<Result> GetResults(User account);
        public Grant? GetAcademic(User account);
        public Grant? GetSocial(User account);
        public Grant? GetPresident(User account);
        public Grant? GetGovernment(User account);
        public Grant? GetSupport(User account);
        #endregion

        #region "Запрос документов"
        public List<Document> GetDocuments(User account);
        #endregion

        #region "Запрос достижений"
        public Achievment? GetAchievmentByFile(string fileId);
        public List<Achievment> GetAchievments(User account);
        #endregion

        #region "Запрос расписаний"
        public List<Timetable> GetPairs(User account);
        public List<Timetable> GetExams(User account);
        public List<Timetable> GetCandidate();
        #endregion

        #region "Достижения - общее"
        public Task AddAchiev(Achievment achiev);
        public Task RemoveAchiev(Achievment achiev);
        #endregion

        #region "Новости"
        public List<New> GetNews();
        #endregion

        #region "Отправка писем на почту"
        public Task<bool> SendPassword(User account, string tmpPass);
        public Task SetTempPass(User account, string pass);
        public Task<bool> SendFeedBack(User account, string theme, string message);
        #endregion

        #region "Авторизация"
        public bool SignIn(string login, string password);
        public bool SignInTemp(string login, string password);
        #endregion

        #region "Менеджер фотографий"
        public bool OnModeration(int id);
        #endregion
    }
}
