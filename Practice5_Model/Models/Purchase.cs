using System.ComponentModel.DataAnnotations.Schema;

namespace Practice5_Model.Models
{
    public class Purchase
    {
        public int PurchaseID { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }

        public Product Product { get; set; }
    }
}
