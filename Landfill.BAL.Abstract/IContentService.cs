using Landfill.Entities;
using Landfill.Models;
using System.Collections.Generic;
using System.Linq;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.BAL.Abstract
{
    public interface IContentService
    {
        IEnumerable<Content> GetAllConntent();
        IQueryable<ContentDto> GetAllContent();
    }
   
}
