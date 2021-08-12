using EntityServer.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntityServer
{
    public class UserAuthDbContext:DbContext
    {
        public DbSet<UserCreds> UsersCreds{ get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // userName
            builder.Entity<UserCreds>()
                   .HasIndex(uc => uc.Username)
                   .IsUnique();

            // seed
            builder.Entity<UserCreds>().HasData(new UserCreds { Username = "simon", Password = CryptoService.HashSHA256("12345donteattheyellowsnow123493")});
        }
    }

    public class UserCreds
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }


}
