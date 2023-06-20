using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static Humanizer.On;
using System.Text.RegularExpressions;
using System;
using Blog.Models;

namespace Blog.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTag { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //dodajemy parę kluczy do tabeli pośredniczącej do relacji many to many
    builder.Entity<ArticleTag>()
    .HasKey(pg => new { pg.ArticleId, pg.TagId });
            //w tabeli posredniczacej ArticleTag
            builder.Entity<ArticleTag>()
            .HasOne<Article>(pg => pg.Article) // dla jednego artykulu
            .WithMany(p => p.ArticleTagConnection) // jest wiele ArticleTag
            .HasForeignKey(p => p.ArticleId); // a powizanie jest realizowane przez klucz obcy ArticleId
    //w tabeli posredniczacej PersonGroup
    builder.Entity<ArticleTag>()
    .HasOne<Tag>(pg => pg.Tag) // dla jednego tagu
    .WithMany(g => g.ArticleTagConnection) // jest wiele ArticleTag
    .HasForeignKey(g => g.TagId); // a powizanie jest realizowane przez klucz obcy TagId
    }
    }
}