using System.ComponentModel.DataAnnotations.Schema;

namespace Landfill.Entities
{
    public abstract class BaseEntity<TKey>
    {
        public virtual TKey Id { get; set; }
    }
}

       


       