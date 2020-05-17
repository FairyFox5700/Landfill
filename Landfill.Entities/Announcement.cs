using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Landfill.Entities
{
    public class Announcement:BaseEntity<int>
    {
        public string Header { get; set; }
        public DateTime ValiUntil { get; set; }
        [ForeignKey("Content")]
        public  int ContentId { get; set; }
        public Content Content { get; set; }

    }
}
//[ForeignKey("FK_Announcement_content")]
//public override int Id { get; set; }
//public int ContentId { get; set; }
//public Content Content { get; set; }



