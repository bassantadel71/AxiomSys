using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomSysDL.Repository
{
	public interface IRepository<T> where T : class
	{
		IQueryable<T> GetAll();
		void Add(T entity);
		void Remove(T entity);
	}
}
