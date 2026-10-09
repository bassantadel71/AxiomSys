using AxiomSysDL.Models.SupplyChain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomSysDL.Context
{
	public class SupplyChainContext : DbContext
	{
		public SupplyChainContext(DbContextOptions<SupplyChainContext> options) : base(options) 
		{
		
		}
		public DbSet<SaTaxis> SaTaxes { get; set; } = null!;
		public DbSet<SaDeliveryTerm> SaDeliveryTerms { get; set; } = null!;


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<SaTaxis>(e =>
			{
				e.ToTable("SA_TAXES");
				e.HasKey(x => x.Code);
				e.Property(x => x.Code).HasColumnName("CODE");
				e.Property(x => x.BName).HasColumnName("B_NAME");
				e.Property(x => x.SName).HasColumnName("S_NAME");
			});

			modelBuilder.Entity<SaDeliveryTerm>(entity =>
			{
				entity.ToTable("SA_DELIVERY_TERMS");
				entity.HasKey(x => x.Code);
				entity.Property(x => x.Code).HasColumnName("CODE").ValueGeneratedNever();
				entity.Property(x => x.SName).HasColumnName("S_NAME").HasMaxLength(60);
				entity.Property(x => x.BName).HasColumnName("B_NAME").HasMaxLength(60);
				entity.Property(x => x.Days).HasColumnName("DAYS");
				entity.Property(x => x.ActiveFlag).HasColumnName("ACTIVE_FLAG");
				entity.Property(x => x.EntryUser).HasColumnName("ENTRY_USER");
				entity.Property(x => x.EntryDate).HasColumnName("ENTRY_DATE");
				entity.Property(x => x.ChangeUser).HasColumnName("CHANGE_USER");
				entity.Property(x => x.ChangeDate).HasColumnName("CHANGE_DATE");
			});
		}
	}
}
