using Landfill.Entities;
using Landfill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.BAL.Abstract
{
    public interface IMappingModel
    {
        public ContentDto MapToContentDTO(Content contentEntity);
        public Content MapToContent(ContentDto contentDto, ContentType contentType);
        public IQueryable<ContentDto> MapToContentDTO(IQueryable<Content> contents);
        // public IQueryable<ContentDto> MapToContentDTO(IQueryable<Content> contents, QueryFilterSet filters);

    }
}
