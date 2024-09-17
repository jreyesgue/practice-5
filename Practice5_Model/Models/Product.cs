using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice5_Model.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        [MaxLength(50)]
        public required string Category { get; set; }
        public DateTime DateAdded { get; set; }

        public Inventory Inventory { get; set; }
    }
}
