using Landfill.DAL.Implementation.Core;
using Landfill.Entities;
using LandFill.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Landfill.DAL.Implementation.Repositories
{
    public class ContentRepository:IContentRepository
    {
        private readonly LandfillContext landfillContext;

        public ContentRepository(LandfillContext landfillContext)
        {
            this.landfillContext = landfillContext;
        }
  
        public IEnumerable<Content> GetContent(Expression<Func<Content, bool>> expression)
        {
           // return Colours.Where(mappedPredicate.Compile()).Select(c => new DomainColour(c.Name)).ToList();
            return landfillContext.Contents.Where(expression).AsEnumerable<Content>();
          
        }


    }
}
