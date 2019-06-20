using Microsoft.EntityFrameworkCore;
using TopicTrennerAPI.Models;


namespace TopicTrennerAPI.Data
{
    public class DbTopicTrennerContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("Server=127.0.0.1; Port=3306; Database=smile; user=root; Pwd=lfasdFl3sa6hRdlk5hVd3lkHjsa; charset=utf8;");
        }

        public DbSet<SimpleRule> Rules { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionSimpleRule> SessionSimpleRules { get; set; }
        public DbSet<SimpleRuleSubject> SimpleRuleSubjects { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SimpleRule>().ToTable("Rule");
            modelBuilder.Entity<Session>().ToTable("Session");
            modelBuilder.Entity<SessionSimpleRule>().ToTable("SessionSimpleRule");
            modelBuilder.Entity<SimpleRuleSubject>().ToTable("SimpleRuleSubject");
            modelBuilder.Entity<Subject>().ToTable("Subject");
        }
    }
}
