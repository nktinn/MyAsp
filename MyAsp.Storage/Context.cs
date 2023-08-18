using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyAsp.Storage.Entities;

namespace MyAsp.Storage
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");
        }

        public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        {
            public DateOnlyConverter() : base(
                    d => d.ToDateTime(TimeOnly.MinValue),
                    d => DateOnly.FromDateTime(d))
            { }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Scientist> Scientists { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Achievment> Achievments { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Grant> Grants { get; set; }
        public DbSet<PhotoManagement> PM { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<New> News { get; set; }
    }
}
