using System.ComponentModel.DataAnnotations.Schema;

namespace Landfill.Entities
{
    public class FAQ :BaseEntity<int>
    {
        public string Tag { get; set; }
        public int ContentId { get; set; }
        public Content Content { get; set; }
    }
}
//[ForeignKey("FK_FAQ_Content")]
//public override int Id { get; set; }
// public Content Content { get; set; }



