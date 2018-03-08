﻿using System.Linq;
using BaselineSolution.Bo.Internal;
using BaselineSolution.DAL.Infrastructure.Bases;
using BaselineSolution.DAL.Repositories;
using BaselineSolution.Facade.Internal;
using BaselineSolution.Framework.Extensions;
using BaselineSolution.Framework.Infrastructure.Attributes;
using BaselineSolution.Framework.Infrastructure.Contracts;
using BaselineSolution.Framework.Infrastructure.Filtering;
using BaselineSolution.Framework.Infrastructure.Sorting;
using BaselineSolution.Framework.Response;
using BaselineSolution.Service.Infrastructure.Extentions;
using BaselineSolution.Service.Translators.Internal;

namespace BaselineSolution.Service.Infrastructure.Internal
{
    public class GenericService<TBo, TEntity> : IGenericService<TBo>
        where TBo : BaseBo, new()
        where TEntity : Entity, new()
    {

        private readonly IGenericRepository<TEntity> _repository;
        private readonly ITranslator<TBo, TEntity> _translator;

        public GenericService(IGenericRepository<TEntity> repository, ITranslator<TBo, TEntity> translator)
        {
            ;
            _repository = repository;
            _translator = translator;
        }

        Response<TBo> IGenericService<TBo>.GetById(int id)
        {
            TEntity item = _repository.FindById(id);
            if (item == null)
                return new Response<TBo>().AddItemNotFound(id);

            return new Response<TBo>(item.ToBo(_translator));
        }

        Response<int> IGenericService<TBo>.AddOrUpdate(TBo bo, int userId)
        {
            if (!bo.IsValid())
                return new Response<int>().AddValidationMessage(bo.ValidationMessages);

            var item = bo.IsNew ? new TEntity() : _repository.FindById(bo.Id);

            if (item == null)
                return new Response<int>().AddItemNotFound(bo.Id);

            item = bo.UpdateModel(item, _translator);
            _repository.AddOrUpdate(item);
            _repository.Commit(userId);

            return new Response<int>(item.Id);
        }

        Response<bool> IGenericService<TBo>.Delete(int id, int userId)
        {
            var user = _repository.FindById(id);
            if (user == null)
                return new Response<bool>().AddItemNotFound(id);

            _repository.Delete(id);
            _repository.Commit(userId);


            return new Response<bool>(true);
        }

        Response<int> IGenericService<TBo>.Count(IEntityFilter<TBo> filter)
        {
            var list = _repository.List();
            list = list.Filter(filter);
            return new Response<int>(list.Count());
        }

        Response<TBo> IGenericService<TBo>.List(IEntityFilter<TBo> filter)
        {
            var list = _repository.List();
            list = list.Filter(filter);

            var translated = list.ToList().Select(x => x.ToBo(_translator)).ToList();
            return new Response<TBo>(translated);
        }

        Response<TBo> IGenericService<TBo>.List(IEntityFilter<TBo> filter, [CanBeNull] IEntitySorter<TBo> sorter, [CanBeNull] int? page, [CanBeNull] int? pageSize)
        {
            var list = _repository.List();
            list = list.Filter(filter);


            if (sorter != null && page.HasValue && pageSize.HasValue)
            {
                list = list.Sort(sorter);
                list = list.Skip((page.Value) * pageSize.Value);
                list = list.Take(pageSize.Value);
            }


            var translated = list.ToList().Select(x => x.ToBo(_translator)).ToList();
            return new Response<TBo>(translated);

        }
    }
}