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
        public UserAuthDbContext(DbContextOptions<UserAuthDbContext> options) : base(options)
        {
            
        }

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
            builder.Entity<UserCreds>().HasData(new UserCreds { Id=1,Username = "simon", Password = CryptoService.HashSHA256("12345donteattheyellowsnow123493")});
            builder.Entity<UserCreds>().HasData(new UserCreds { Id=2,Username = "lake", Password = CryptoService.HashSHA256("incaroad$guacomoligreen12345") });
            builder.Entity<UserCreds>().HasData(new UserCreds { Id=3,Username = "palmer", Password = CryptoService.HashSHA256("imightbegoingtom0nta@asoon321432543") });
            builder.Entity<UserCreds>().HasData(new UserCreds { Id=4,Username = "george", Password = CryptoService.HashSHA256("mrtem0rinem@nplayas0n&tomeeee0192333") });
            builder.Entity<UserCreds>().HasData(new UserCreds { Id=5, Username = "barak", Password = CryptoService.HashSHA256("evry1234bodyrockoor098432buddddy") });
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
