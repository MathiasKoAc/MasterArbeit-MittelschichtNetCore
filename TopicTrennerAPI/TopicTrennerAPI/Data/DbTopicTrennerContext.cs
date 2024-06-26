﻿using Microsoft.EntityFrameworkCore;
using System;
using TopicTrennerAPI.Models;


namespace TopicTrennerAPI.Data
{
    public class DbTopicTrennerContext : DbContext
    {
        private static string _dbConfigStr;
        public static string DbConfigString {
            set
            {
                _dbConfigStr = value;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(_dbConfigStr == null)
            {
                throw new Exception("DB Config not set correct please setup DbConfigString");
            }
            optionsBuilder.UseMySql(_dbConfigStr);//UseInMemoryDatabase("SmileTEST"); //"Server=127.0.0.1; Port=3306; Database=smile; user=root; Pwd=lfasdFl3sa6hRdlk5hVd3lkHjsa; charset=utf8;");
        }

        public DbSet<SimpleRule> Rules { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SimpleRuleSubject> SimpleRuleSubjects { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SessionRun> SessionRuns { get; set; }
        public DbSet<TimeDiff> TimeDiffs { get; set; }
        public DbSet<PurposeMessage> PurposeMessages { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<EventMessage> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SimpleRule>().ToTable("Rule");
            modelBuilder.Entity<Session>().ToTable("Session");
            modelBuilder.Entity<SimpleRuleSubject>().ToTable("SimpleRuleSubject");
            modelBuilder.Entity<Subject>().ToTable("Subject");
            modelBuilder.Entity<SessionRun>().ToTable("SessionRun");
            modelBuilder.Entity<TimeDiff>().ToTable("TimeDiff");
            modelBuilder.Entity<PurposeMessage>().ToTable("PurposeMessage");
            modelBuilder.Entity<Log>().ToTable("Log");
            modelBuilder.Entity<EventMessage>().ToTable("Event");
        }
    }
}
