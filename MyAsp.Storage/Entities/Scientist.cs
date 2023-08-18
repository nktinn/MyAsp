namespace MyAsp.Storage.Entities
{
    public class Scientist
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        //Ученая степень
        public string Degree { get; set; }
        //Кафедра
        public string Department { get; set; }
    }
}
