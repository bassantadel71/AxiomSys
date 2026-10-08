using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomSysDL.Repository
{
	public interface IUnitOfWork<TContext> where TContext : DbContext
	{
		IRepository<T> GetRepository<T>() where T : class;
		int Commit();
	}
}
