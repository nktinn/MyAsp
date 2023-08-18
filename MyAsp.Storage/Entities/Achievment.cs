namespace MyAsp.Storage.Entities
{
    public class Achievment
    {
        [Key]
        public int Id { get; set; }

        public string File { get; set; }
        public DateOnly Date { get; set; }
        public string Name { get; set; }
        [Required]
        public int UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; }
    }
}
