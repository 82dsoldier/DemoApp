using System.Collections.Generic;

namespace DemoApp.Data.Interfaces {
	public interface IServiceCore<TEntity>
		where TEntity : class, IModel {
		void Create(TEntity entity);
		void Create<TDto>(TDto dto) where TDto : IModel;
		TEntity Get(int id);
		TDto Get<TDto>(int id) where TDto : IModel;
		IEnumerable<TEntity> GetList();
		IEnumerable<TDto> GetList<TDto>();
		void Update(TEntity entity);
		void Update<TDto>(TDto dto) where TDto : IModel;
		void Delete(int id);

	}
}
