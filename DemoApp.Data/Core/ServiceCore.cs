using DemoApp.Data.Interfaces;
using ExpressMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DemoApp.Data.Core {
	/// <summary>
	/// Create data services quickly with this base class
	/// </summary>
	/// <typeparam name="TEntity">The type of entity this service will be returning.</typeparam>
	/// <seealso cref="DemoApp.Data.Interfaces.IServiceCore{TEntity}" />
	/// <remarks>
	/// The ServiceCore class encompasses basic CRUD operations and can be sub-classed to quickly provide this functionality 
	/// to other data services.  In a normal application, there would be no need for this class to be abstract as all of the 
	/// functions would be virtual.  However, this will hopefully meet the requirement to provide one abstract class.
	/// </remarks>
	public abstract class ServiceCore<TEntity> : IServiceCore<TEntity>
		where TEntity : class, IModel {
		protected DemoAppContext _context;

		public ServiceCore(DemoAppContext context) {
			_context = context;
		}
		public virtual void Create(TEntity entity) {
			_context.Set<TEntity>().Add(entity);
			_context.SaveChanges();
		}

		public virtual void Create<TDto>(TDto dto) where TDto : IModel {
			var entity = Mapper.Map<TDto, TEntity>(dto);
			_context.Set<TEntity>().Add(entity);
			_context.SaveChanges();
		}

		public abstract void Delete(int id);

		public virtual TEntity Get(int id)
			=> _context.Set<TEntity>().FirstOrDefault(e => e.Id == id);

		public virtual TDto Get<TDto>(int id) where TDto : IModel
			=> Mapper.Map<TEntity, TDto>(Get(id));

		public virtual IEnumerable<TEntity> GetList()
			=> _context.Set<TEntity>();

		public virtual IEnumerable<TDto> GetList<TDto>()
			=> Mapper.Map<IEnumerable<TEntity>, IEnumerable<TDto>>(GetList());
		public virtual void Update(TEntity entity) {
			_context.Set<TEntity>().Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
			_context.SaveChanges();
		}

		public virtual void Update<TDto>(TDto dto) where TDto : IModel {
			var entity = Mapper.Map<TDto, TEntity>(dto);
			Update(entity);
		}
	}
}
