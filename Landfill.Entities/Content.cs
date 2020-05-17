using System.Collections.Generic;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.Entities
{
    public  class Content : BaseEntity<int>
    {
        public ContentType ContentType { get; set; }
        public ICollection<ContentTranslation> Translations { get; set; }
        public State State { get; set; }
        public FAQ Faq { get; set; }
        public Announcement Announcement { get; set; }

    }
}

       


       