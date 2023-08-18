namespace MyAsp.Storage.Entities
{
    public class Result
    {
        [Key]
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string SubjName { get; set; }
        public string Grade { get; set; }
        //Уточнение типа экзамена
        public string Type { get; set; }
        [Required]
        public int UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; }
    }
}
