using System.IO.Compression;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>{
    public void Configure(EntityTypeBuilder<User> builder){
        builder.ToTable("user");
        builder.HasKey(x => x.Id);

        //--Properties
        builder.Property(x => x.Id)
            .IsRequired()
            .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            .HasColumnName("idPk");
        
        builder.Property(x => x.Username)
            .IsRequired()
            .HasColumnName("usernameUser")
            .HasMaxLength(50);
        
        builder.Property(x => x.Email)            
            .HasColumnName("emailUser")
            .HasMaxLength(100);
        
        builder.Property(x => x.Password)
            .IsRequired()
            .HasColumnName("passwordUser")
            .HasMaxLength(200);

        builder.Property(x => x.AccessToken)
            .HasColumnName("accessTokenUser")
            .HasMaxLength(500);
        
        builder.Property(x => x.RefreshToken)
            .HasColumnName("refreshTokenUser")
            .HasMaxLength(500);
        
        builder.HasIndex(p => new{
            p.Username,p.Email
        })
        .HasDatabaseName("IX_Username_Email")
        .IsUnique();
        
        //-Relations
        builder.HasOne(x => x.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleId);   
    }
}