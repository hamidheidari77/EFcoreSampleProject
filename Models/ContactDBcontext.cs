using Microsoft.EntityFrameworkCore;

namespace ContactManager.Models
{
    public class ContactDBcontext:DbContext
    {
        public DbSet<Person> People { get; set; }   
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=192.168.103.1;Initial Catalog=Contact;User ID=sa;Password=3860449941");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
