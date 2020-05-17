using System.ComponentModel.DataAnnotations.Schema;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.Entities
{
    public class ContentTranslation : BaseEntity<int>
    {
        public int ContentId { get; set; }
        public Content Content { get; set; }
        public string Text { get; set; }
        public Language Language { get; set; }
    }
}

       


       