namespace MyAsp.Storage.Entities
{
    public class PhotoManagement
    {
        [Key]
        public int Id { get; set; }

        public string File { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public int UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; }
    }
}
