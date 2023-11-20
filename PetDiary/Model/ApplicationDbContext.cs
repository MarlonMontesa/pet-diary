using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetDiary.Model
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public virtual DbSet<ApplicationUser> users { get; set; }
        public virtual DbSet<UserPost> UserPost { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<UserPhoto> UserPhoto { get; set; }
        public virtual DbSet<UserVideo> UserVideo { get; set; }
        public virtual DbSet<UserAdoption> UserAdoption { get; set; }
        public virtual DbSet<AdoptComments> adoptComments { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public object ApplicationUser { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //name = "name of connection string"
                optionsBuilder.UseSqlServer("Name=psqlDb");
            }
        }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(e => e.Id)
                .HasColumnName("id");

            });
            modelBuilder.Entity<ApplicationUser>().HasIndex(u => u.UserName).IsUnique();
            modelBuilder.Entity<ApplicationUser>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<ApplicationRole>(entity => { entity.ToTable("Roles"); });
            modelBuilder.Entity<IdentityUserRole<Guid>>(entity => { entity.ToTable("UserRoles"); });
            modelBuilder.Entity<IdentityUserClaim<Guid>>(entity => { entity.ToTable("UserClaims"); });
            modelBuilder.Entity<IdentityUserLogin<Guid>>(entity => { entity.ToTable("UserLogins"); });
            modelBuilder.Entity<IdentityUserToken<Guid>>(entity => { entity.ToTable("UserTokens"); });
            modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity => { entity.ToTable("RoleClaims"); });
            modelBuilder.Entity<UserPost>(entity => 
            { 
                entity.ToTable("UserPost");
                entity.HasOne(x => x.user).WithMany(y => y.userPosts);
            });
            modelBuilder.Entity<Comments>(entity =>
            {
                entity.ToTable("Comments");
                entity.HasOne(x => x.user).WithMany(y => y.comments).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.userPost).WithMany(y => y.comments).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<UserPhoto>(entity =>
            {
                entity.ToTable("UserPhoto");
                entity.HasOne(x => x.user).WithMany(y => y.userPhotos);
            });
            modelBuilder.Entity<UserVideo>(entity =>
            {
                entity.ToTable("UserVideo");
                entity.HasOne(x => x.user).WithMany(y => y.userVideos);
            });
            modelBuilder.Entity<UserAdoption>(entity =>
            {
                entity.ToTable("UserAdoption");
                entity.HasOne(x => x.user).WithMany(y => y.userAdoptions);
            });
            modelBuilder.Entity<AdoptComments>(entity =>
            {
                entity.ToTable("AdoptComments");
                entity.HasOne(x => x.user).WithMany(y => y.adoptComments).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(x => x.userAdoption).WithMany(y => y.adoptComments).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");
                entity.HasOne(x => x.user).WithMany(y => y.messages).HasForeignKey(y => y.UserId);
            });


        }
    }
}
