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
		public DbSet<SaTaxis> SaTaxes => Set<SaTaxis>();

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
		}
	}
}
