using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomSysDL.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly DbContext _ctx;

		public Repository(DbContext ctx)
		{
			_ctx = ctx;
		}

		public IQueryable<T> GetAll()
		{
			return _ctx.Set<T>();
		}

		public void Add(T entity)
		{
			_ctx.Set<T>().Add(entity);
		}

		public void Remove(T entity)
		{
			_ctx.Set<T>().Remove(entity);
		}
	}
}
