namespace MyAsp.Storage.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? Photo { get; set; }
        //Номер удостоверения
        public string CNumber { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        //Номер группы
        public string GNumber { get; set; }
        //направление подготовки
        public string? StDirection { get; set; }
        //профиль подготовки
        public string? ProfDirection { get; set; }
        public string? WTheme { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        //Временный пароль
        public string? TmpPassword { get; set; }
        //Существует ли запрос сброса пароля
        public DateTime? RequestTime { get; set; }
        public int? ScientistID { get; set; }
        [ForeignKey(nameof(ScientistID))]
        public virtual Scientist? Scientist { get; set; }
    }
}
