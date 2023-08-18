namespace MyAsp.Storage.Entities
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string Name { get; set; }
        [Required]
        public int UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; }
    }
}
