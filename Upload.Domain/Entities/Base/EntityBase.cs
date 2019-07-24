using prmToolkit.NotificationPattern;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Upload.Domain.Entities.Base
{
    public abstract class EntityBase : Notifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public abstract void Validate();
    }
}
