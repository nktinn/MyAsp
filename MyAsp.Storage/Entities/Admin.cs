namespace MyAsp.Storage.Entities
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? Photo { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        //Должность
        public string? Function { get; set; }
        public string? Cabinet { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
