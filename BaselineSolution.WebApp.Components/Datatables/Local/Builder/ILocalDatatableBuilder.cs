﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BaselineSolution.Framework.Infrastructure.Attributes;
using BaselineSolution.WebApp.Components.Datatables.Base;

namespace BaselineSolution.WebApp.Components.Datatables.Local.Builder
{
    /// <summary>
    ///     Builder for a <see cref="LocalDatatable{TEntity}" />
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ILocalDatatableBuilder<TEntity> : IDatatableBuilder where TEntity : class
    {
        // adds a display column to the datatable 
        ILocalDatatableColumnBuilder<TEntity> Column([NotNull] string header);

        // adds a property column to the datatable
        ILocalDatatableColumnBuilder<TEntity, bool?> Column([NotNull] string header, [NotNull] Expression<Func<TEntity, bool?>> propertyExpression);
        ILocalDatatableColumnBuilder<TEntity, int?> Column([NotNull] string header, [NotNull] Expression<Func<TEntity, int?>> propertyExpression);
        ILocalDatatableColumnBuilder<TEntity, double?> Column([NotNull] string header, [NotNull] Expression<Func<TEntity, double?>> propertyExpression);
        ILocalDatatableColumnBuilder<TEntity, decimal?> Column([NotNull] string header, [NotNull] Expression<Func<TEntity, decimal?>> propertyExpression);
        ILocalDatatableColumnBuilder<TEntity, long?> Column([NotNull] string header, [NotNull] Expression<Func<TEntity, long?>> propertyExpression);
        ILocalDatatableColumnBuilder<TEntity, short?> Column([NotNull] string header, [NotNull] Expression<Func<TEntity, short?>> propertyExpression);
        ILocalDatatableColumnBuilder<TEntity, string> Column([NotNull] string header, [NotNull] Expression<Func<TEntity, string>> propertyExpression);
        ILocalDatatableColumnBuilder<TEntity, DateTime?> Column([NotNull] string header, [NotNull] Expression<Func<TEntity, DateTime?>> propertyExpression);
        ILocalDatatableColumnBuilder<TEntity, TimeSpan?> Column([NotNull] string header, [NotNull] Expression<Func<TEntity, TimeSpan?>> propertyExpression);
        ILocalDatatableColumnBuilder<TEntity, ICollection<TProperty>> Column<TProperty>([NotNull] string header, [NotNull] Expression<Func<TEntity, ICollection<TProperty>>> propertyExpression);

        LocalDatatable<TEntity> Build();
        LocalDatatable<TEntity> Build(string htmlString);
        
        
    }
}