using DemoApp.Data.Interfaces;
using ExpressMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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
		protected ILogger _logger;
		public ServiceCore(DemoAppContext context, ILogger logger) {
			_context = context;
			_logger = logger;
		}
		public virtual void Create(TEntity entity) {
			try {
				_context.Set<TEntity>().Add(entity);
				_context.SaveChanges();
			} catch (Exception e) {
				//When an exception occurs, catch it here to log whatever went wrong, but throw it back out to the controller
				//so it can report the proper status back to the client.  This can avoid potentially incorrect statuses at the client
				//such as possibly getting a 404 from a Get request when it should have been a 500
				_logger.LogError(e, $"An error occurred creating an object of type {nameof(TEntity)}");
				throw;
			}
		}

		public virtual void Create<TDto>(TDto dto) where TDto : IModel {
			try {
				var entity = Mapper.Map<TDto, TEntity>(dto);
				_context.Set<TEntity>().Add(entity);
				_context.SaveChanges();
			} catch (Exception e) {
				_logger.LogError(e, $"An error occurred creating an object of type {nameof(TEntity)}");
				throw;
			}
		}

		public abstract void Delete(int id);

		public virtual TEntity Get(int id) {
			try {
				return _context.Set<TEntity>().FirstOrDefault(e => e.Id == id);
			} catch (Exception e) {
				_logger.LogError(e, $"An error occurred fetching an object of type {nameof(TEntity)}");
				throw;
			}
		}

		public virtual TDto Get<TDto>(int id) where TDto : IModel {
			try {
				return Mapper.Map<TEntity, TDto>(Get(id));
			} catch (Exception e) {
				_logger.LogError(e, $"An error occurred fetching an object of type {nameof(TEntity)}");
				throw;
			}
		}

		public virtual IEnumerable<TEntity> GetList() {
			try {
				return _context.Set<TEntity>();
			} catch (Exception e) {
				_logger.LogError(e, $"An error occurred fetching a list of type {nameof(TEntity)}");
				throw;
			}
		}

		public virtual IEnumerable<TDto> GetList<TDto>() {
			try {
				return Mapper.Map<IEnumerable<TEntity>, IEnumerable<TDto>>(GetList());
			} catch (Exception e) {
				_logger.LogError(e, $"An error occurred fetching a list of type {nameof(TEntity)}");
				throw;
			}
		}

		public virtual void Update(TEntity entity) {
			try {
				_context.Set<TEntity>().Attach(entity);
				_context.Entry(entity).State = EntityState.Modified;
				_context.SaveChanges();
			} catch (Exception e) {
				_logger.LogError(e, $"An error occurred updating an object of type {nameof(TEntity)}");
				throw;
			}
		}

		public virtual void Update<TDto>(TDto dto) where TDto : IModel {
			try {
				var entity = Mapper.Map<TDto, TEntity>(dto);
				Update(entity);
			} catch (Exception e) {
				_logger.LogError(e, $"An error occurred updating an object of type {nameof(TEntity)}");
				throw;
			}
		}
	}
}
