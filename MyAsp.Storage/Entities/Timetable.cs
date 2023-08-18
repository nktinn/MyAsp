namespace MyAsp.Storage.Entities
{
    public class Timetable
    {
        [Key]
        public int Id { get; set; }

        public string File { get; set; }
        public DateOnly Date { get; set; }
        public string Name { get; set; }
        //Вид расписания (для профиля подг(ук. название), направления(ук. название), группы(указать номер), кандидатские)
        public string Type { get; set; }
    }
}
