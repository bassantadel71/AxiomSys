using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomSysDL.Repository
{
	public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
	{
		private readonly TContext _ctx;

		public UnitOfWork(TContext ctx)
		{
			_ctx = ctx;
		}

		public IRepository<T> GetRepository<T>() where T : class
		{
			return new Repository<T>(_ctx);
		}

		public int Commit()
		{
			return _ctx.SaveChanges();
		}
	}
}
