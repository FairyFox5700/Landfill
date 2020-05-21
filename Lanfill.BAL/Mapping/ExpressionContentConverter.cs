using Landfill.Entities;
using Landfill.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Lanfill.BAL.Implementation.Mapping
{
    public class BaseExpressionConverter<TDto, TEntity> : ExpressionVisitor
    where TDto : class, new()
    where TEntity : BaseEntity<int>, new()
    {
        private readonly MappingContainer<TDto, TEntity> mappingContainer;
        private ParameterExpression originalParameter;
        private ParameterExpression replaceParameter;
        public BaseExpressionConverter(MappingContainer<TDto, TEntity> mappingContainer)
        {
            this.mappingContainer = mappingContainer;
        }
        public override Expression Visit(Expression node)
        {
            if (originalParameter == null)
            {
                if (node.NodeType != ExpressionType.Lambda)
                    throw new ArithmeticException("Expession must be a lambda expression");
                var lambda = (LambdaExpression)node;

                if (lambda.ReturnType != typeof(bool) || lambda.Parameters.Count != 1
                    || lambda.Parameters[0].Type != typeof(TDto))
                    throw new ArgumentException();
                originalParameter = lambda.Parameters[0];
                replaceParameter = Expression.Parameter(typeof(TEntity), originalParameter.Name);
            }
            return base.Visit(node);
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == this.originalParameter ? this.replaceParameter : base.VisitParameter(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression == originalParameter)
            {
                var member = mappingContainer.GetMappingFromMemberName<TDto>(node.Member.Name);
                return Expression.Property(replaceParameter, member);
            }
            return base.VisitMember(node);
        }
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            if (typeof(T) == typeof(Func<TDto, bool>))
            {
                return Expression.Lambda<Func<TEntity, bool>>(Visit(node.Body), new[] { replaceParameter });
            }
            return base.VisitLambda(node);
        }
    }

    public class MyExpressionVisitor : ExpressionVisitor
    {
        private ReadOnlyCollection<ParameterExpression> _parameters;

        public static Func<ContentDto, bool> Convert<T>(Expression<T> root)
        {
            var visitor = new MyExpressionVisitor();
            var expression = (Expression<Func<ContentDto, bool>>)visitor.Visit(root);
            return expression.Compile();
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            var param = _parameters?.FirstOrDefault(p => p.Name == node.Name);

            if (param != null)
            {
                return param;
            }

            if (node.Type == typeof(Content))
            {
                return Expression.Parameter(typeof(Content), node.Name);
            }

            if (node.Type == typeof(ContentTranslation))
            {
                return Expression.Parameter(typeof(TranslationDTO), node.Name);
            }

            return node;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            _parameters = VisitAndConvert<ParameterExpression>(node.Parameters, "VisitLambda");
            return Expression.Lambda(Visit(node.Body), _parameters);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var exp = Visit(node.Expression);

            if (node.Member.DeclaringType == typeof(Content))
            {
                if (node.Type == typeof(ICollection<ContentTranslation>))
                {
                    return Expression.MakeMemberAccess(exp, typeof(TranslationDTO).GetProperty("Translations"));
                }

                return Expression.MakeMemberAccess(exp, typeof(ContentDto).GetProperty(node.Member.Name));
            }

            if (node.Member.DeclaringType == typeof(FaqModel))
            {
                var nested = Expression.MakeMemberAccess(exp, typeof(FAQ).GetProperty("Person"));
                return Expression.MakeMemberAccess(nested, typeof(FaqModel).GetProperty(node.Member.Name));
            }

            return base.VisitMember(node);
        }
    }


}
