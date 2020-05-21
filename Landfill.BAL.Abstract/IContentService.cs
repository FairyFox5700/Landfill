using Landfill.Entities;
using Landfill.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData.Query;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.BAL.Abstract
{
    public interface IContentService
    {
        IEnumerable<Content> GetAllConntent();
        IQueryable<ContentDto> GetAllContent();
        IQueryable<ContentDto> GetAllContent(ODataQueryOptions<ContentDto> options);
    }
   
}
