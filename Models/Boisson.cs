using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
    [Table("Boisson")]
    public class Boisson
    {
        [Key]
        public long Id { get; set; }
        //[ForeignKey("ID")] //Ne fonctionne pas ? 
        //public Commande? Commande { get; set; }

        public long IdCommande { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
    }
}