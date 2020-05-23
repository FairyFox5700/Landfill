using Landfill.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LandFill.DAL.Abstract
{
    public interface IContentRepository
    {
        IEnumerable<Content> GetContent(Expression<Func<Content, bool>> expression);
        IEnumerable<Content> GetContent<TKey>(Expression<Func<Content, TKey>> expression);
        IEnumerable<Content> GetContent(int pageSize, int pageIndex);
    }
}
