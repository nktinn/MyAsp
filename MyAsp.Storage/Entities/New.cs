namespace MyAsp.Storage.Entities
{
    public class New
    {
        [Key]
        public int Id { get; set; }
        public string Info { get; set; }
        public string Link { get; set; }
        public DateOnly Date{ get; set; }
    }
}
