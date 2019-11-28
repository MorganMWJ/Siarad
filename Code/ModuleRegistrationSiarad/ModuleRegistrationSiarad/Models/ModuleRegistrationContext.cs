using Microsoft.EntityFrameworkCore;

namespace ModuleRegistrationSiarad.Models
{
    public class ModuleRegistrationContext : DbContext
    {
        public ModuleRegistrationContext(DbContextOptions<ModuleRegistrationContext> options) : base(options) { }

        public DbSet<Module> module {get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Module>()
                .HasKey(c => new { c.module_id, c.year });
            modelBuilder.Entity<StudentsRegistreredForSpecificModules>().Property(p => p.auto_id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Staff>().Property(p => p.auto_id).ValueGeneratedOnAdd();
        }
        public DbSet<Student> students {get; set;}
        public DbSet<Staff> staff { get; set; }
        public DbSet<StudentsRegistreredForSpecificModules> registered_students { get; set; }
    }
}
