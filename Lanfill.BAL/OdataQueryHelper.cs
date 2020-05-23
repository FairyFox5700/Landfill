using Landfill.Entities;
using Landfill.Models;
using Lanfill.BAL.Implementation.Mapping;
using Microsoft.AspNet.OData.Query;
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;


namespace Lanfill.BAL.Implementation
{
    public static class OdataQueryHelper
    {
        public static Expression ToExpression<TElement>(this OrderByQueryOption orderBy)
        {
            IQueryable queryable = Enumerable.Empty<TElement>().AsQueryable();
            queryable = orderBy.ApplyTo(queryable, new ODataQuerySettings());
            var methodCallExp = queryable.Expression as MethodCallExpression;
            if (methodCallExp == null)
            {
                // return a default generic expression that validates to true
                return Expression.Lambda<Func<TElement, string>>(Expression.Constant(true),
                    Expression.Parameter(typeof(TElement)));
            }

            return methodCallExp;
        }
        //filtered expression is not serialized why????????????????
        public static Expression<Func<T, bool>> GetFilter<T>(this ODataQueryOptions<T> options)
        {
            IQueryable query = Enumerable.Empty<T>().AsQueryable();
            query = options.Filter.ApplyTo(query, new ODataQuerySettings());
            var call = query.Expression as MethodCallExpression;
            if (call != null && call.Method.Name == nameof(Queryable.Where) && call.Method.DeclaringType == typeof(Queryable))
            {
                var predicate = ((UnaryExpression)call.Arguments[1]).Operand;
                return (Expression<Func<T, bool>>)predicate;
            }
            return null;
        }

      

    }
}
