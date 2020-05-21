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
    }
}
