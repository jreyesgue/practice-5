using System.ComponentModel.DataAnnotations.Schema;

namespace Practice5_Model.Models
{
    public class Sale
    {
        public int SaleID { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal SalePrice { get; set; }
        public DateTime SaleDate { get; set; }

        public Product? Product { get; set; }
    }
}
