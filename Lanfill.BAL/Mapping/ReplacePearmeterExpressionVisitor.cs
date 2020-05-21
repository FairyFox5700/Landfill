using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Text;

namespace Lanfill.BAL.Implementation.Mapping
{
    public class ReplacePearmeterExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression originalParameter;
        private readonly Expression replaceParameter;
        public ReplacePearmeterExpressionVisitor(Expression originalParameter, Expression replaceParameter)
        {
            this.originalParameter = originalParameter ?? throw new ArgumentNullException(nameof(originalParameter));
            this.replaceParameter = replaceParameter ?? throw new ArgumentNullException(nameof(replaceParameter));
        }
        public override Expression Visit(Expression node)
        {
            return  node == this.originalParameter ? this.replaceParameter : base.Visit(node);
        }
    }



}
