using Landfill.Entities;
using System.Collections.Generic;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.Models
{
    public class ContentConatinerDto
    {
        public ContentType ContentType { get; set; }
        public List<ContentTranslation> Translations { get; set; }//Dict
        public State State { get; set; }
        public FaqModel FaqModel { get; set; }
        public AnnouncementModel AnnouncementModel { get; set; }
    }


}
