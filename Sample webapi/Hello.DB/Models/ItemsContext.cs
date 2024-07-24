using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Hello.DB.Models;

public partial class ItemsContext : DbContext
{
    public ItemsContext()
    {
    }

    public ItemsContext(DbContextOptions<ItemsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Itemcost> Itemcosts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.0.0.27;Database=Items;Integrated Security=true;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Items__3214EC074D38CE45");

            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.CostNavigation).WithMany(p => p.Items)
                .HasForeignKey(d => d.Cost)
                .HasConstraintName("FK__Items__Cost__403A8C7D");
        });

        modelBuilder.Entity<Itemcost>(entity =>
        {
            entity.HasKey(e => e.Icost).HasName("PK__Itemcost__2CFB47CD09624564");

            entity.ToTable("Itemcost");

            entity.Property(e => e.Icost)
                .ValueGeneratedNever()
                .HasColumnName("icost");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
