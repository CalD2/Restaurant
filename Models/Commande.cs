using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
    [Table("Commande")]
    public class Commande
    {
        /** Id de la commande */
        [Key]
        public long Id { get; set; }
        /** Statut de la commande : RECEIVED, PREPARATION, ENDED, DELIVERED */
        public string? Status { get; set; }

        public List<Plat>? Plats { get; set; }

        public List<Boisson>? Boissons { get; set; }

    }
}