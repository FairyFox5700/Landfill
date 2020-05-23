using Landfill.DAL.Implementation.Core;
using Landfill.Entities;
using LandFill.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Landfill.DAL.Implementation.Repositories
{
    public class ContentRepository : IContentRepository
    {
        private readonly LandfillContext landfillContext;

        public ContentRepository(LandfillContext landfillContext)
        {
            this.landfillContext = landfillContext;
        }

        public IEnumerable<Content> GetContent(Expression<Func<Content, bool>> expression)
        {
            return landfillContext.Contents.Where(expression).AsEnumerable<Content>();


        }
        public IEnumerable<Content> GetContent<TKey>(Expression<Func<Content, TKey>> expression)
        {
            return landfillContext.Contents.OrderBy(expression).AsEnumerable<Content>();
        }

        public IEnumerable<Content> GetContent(int pageSize, int pageIndex)
        {
            return landfillContext.Contents.Skip(pageSize *(pageIndex- 1)).Take(pageSize);
        }



    }
}
