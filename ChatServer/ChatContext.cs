﻿using ChatServer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Nancy;
using Npgsql;

namespace ChatServer
{
    public class ChatContext : DbContext
    {
        private readonly GlobalConfig config;
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTeam> UserTeams{ get; set; }

        public ChatContext(GlobalConfig config)
        {
            this.config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            switch (config.DbType.ToLower())
            {
                case "inmemory":
                    optionsBuilder.UseInMemoryDatabase(config.DbName);
                    break;
                case "localdb":
                    optionsBuilder.UseSqlServer($"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={config.DbName}");
                    break;
                case "pgsql":
                    var connectionString = new NpgsqlConnectionStringBuilder
                    {
                        Database = config.DbName,
                        Username = config.Username,
                        Password = config.Password,
                        Host = config.DbHost,
                        Port = 5432
                    }.ConnectionString;
                    optionsBuilder.UseNpgsql(connectionString);
                    break;
                default:
                    throw new ConfigurationException("Invalid database type");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Channel>()
                .HasIndex(c => c.ChannelName).IsUnique();
            modelBuilder.Entity<Channel>()
                .HasOne(c => c.Team)
                .WithMany(t => t.Channels)
                .HasForeignKey(c => c.TeamId);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Channel)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChannelId);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Target)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Team>()
                .HasMany(t => t.UserTeams)
                .WithOne(tu => tu.Team)
                .HasForeignKey(tu => tu.TeamId);
            modelBuilder.Entity<Team>()
                .HasIndex(t => t.Name).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserTeams)
                .WithOne(ut => ut.User)
                .HasForeignKey(ut => ut.UserId);
            modelBuilder.Entity<UserTeam>()
                .HasOne(ut => ut.Role)
                .WithMany()
                .HasForeignKey(ut => ut.RoleId);
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name).IsUnique();
        }
    }
}