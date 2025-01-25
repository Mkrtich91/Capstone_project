// <copyright file="DataContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.DataAccessLayer.Database
{
    using GameStore.DataAccessLayer.Data;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
   

    public class DataContext : IdentityDbContext<IdentityUser, UserRole, string>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Permission> Permissions { get; set; }

        public virtual DbSet<RolePermission> RolePermissions { get; set; }

        public virtual DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Platform> Platforms { get; set; }

        public DbSet<GameGenre> GameGenres { get; set; }

        public DbSet<GamePlatform> GamePlatforms { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderGame> OrderGames { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.HasIndex(e => e.Key).IsUnique();
                entity.Property(e => e.Key).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.UnitInStock).IsRequired();
                entity.Property(e => e.Discount).IsRequired();

                entity.HasOne(e => e.Publisher)
                    .WithMany(p => p.Games)
                    .HasForeignKey(e => e.PublisherId)
                    .IsRequired();

                entity.HasMany(g => g.Comments)
              .WithOne(c => c.Game)
              .HasForeignKey(c => c.GameId)
              .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.PublishedDate)
           .IsRequired().HasColumnType("datetime");
                entity.Property(e => e.ViewCount)
                    .IsRequired()
                    .HasDefaultValue(0);

            });
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.ParentGenreId);
            });

            modelBuilder.Entity<Platform>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Type).IsRequired();
                entity.HasIndex(e => e.Type).IsUnique();
            });

            modelBuilder.Entity<GameGenre>()
                .HasKey(pc => new { pc.GameId, pc.GenreId });
            modelBuilder.Entity<GameGenre>()
                .HasOne(p => p.Game)
                .WithMany(pc => pc.GameGenres)
                .HasForeignKey(p => p.GameId);
            modelBuilder.Entity<GameGenre>()
               .HasOne(p => p.Genre)
               .WithMany(pc => pc.GameGenres)
               .HasForeignKey(p => p.GenreId);
            modelBuilder.Entity<GamePlatform>()
          .HasKey(gp => new { gp.GameId, gp.PlatformId });

            modelBuilder.Entity<GamePlatform>()
                .HasOne(gp => gp.Game)
                .WithMany(game => game.GamePlatforms)
                .HasForeignKey(gp => gp.GameId);

            modelBuilder.Entity<GamePlatform>()
                .HasOne(gp => gp.Platform)
                .WithMany(platform => platform.GamePlatforms)
                .HasForeignKey(gp => gp.PlatformId);

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CompanyName).IsRequired();
                entity.HasIndex(e => e.CompanyName).IsUnique();
                entity.Property(e => e.HomePage);
                entity.Property(e => e.Description);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Date);
                entity.Property(e => e.CustomerId).IsRequired();
                entity.Property(e => e.Status)
              .HasConversion<int>()
              .IsRequired();
                entity.HasMany(e => e.OrderGames)
                    .WithOne(og => og.Order)
                    .HasForeignKey(og => og.OrderId);
            });

            modelBuilder.Entity<OrderGame>(entity =>
            {
                entity.HasKey(og => og.OrderGameId);
                entity.Property(og => og.Price).IsRequired();
                entity.Property(og => og.Quantity).IsRequired();
                entity.Property(og => og.Discount);

                entity.HasOne(og => og.Order)
                    .WithMany(o => o.OrderGames)
                    .HasForeignKey(og => og.OrderId);

                entity.HasOne(og => og.Game)
                    .WithMany(g => g.OrderGames)
                    .HasForeignKey(og => og.GameId);

                entity.HasIndex(og => new { og.OrderId, og.GameId }).IsUnique();
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired();

                entity.Property(c => c.Body)
                    .IsRequired();

                entity.HasOne(c => c.ParentComment)
                    .WithMany(c => c.Replies)
                    .HasForeignKey(c => c.ParentCommentId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                entity.HasOne(c => c.Game)
                    .WithMany(g => g.Comments)
                    .HasForeignKey(c => c.GameId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasMany(p => p.RolePermissions)
                    .WithOne(rp => rp.Permission)
                    .HasForeignKey(rp => rp.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasMany(r => r.RolePermissions)
                    .WithOne(rp => rp.Role)
                    .HasForeignKey(rp => rp.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

                entity.HasOne(rp => rp.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(rp => rp.RoleId)
                    .IsRequired();

                entity.HasOne(rp => rp.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(rp => rp.PermissionId)
                    .IsRequired();
            });

            DataSeeder.SeedGenres(modelBuilder);
            DataSeeder.SeedPlatforms(modelBuilder);
            DataSeeder.SeedPublishers(modelBuilder);
        }
       
    }
}
