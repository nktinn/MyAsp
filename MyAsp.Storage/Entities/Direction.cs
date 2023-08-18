namespace MyAsp.Storage.Entities
{
    public class Direction
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        //Кафедра
        public string? Department { get; set; }
    }
}
