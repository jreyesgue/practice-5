using System.ComponentModel.DataAnnotations.Schema;

namespace Practice5_Model.Models
{
    public class Inventory
    {
        public int InventoryID { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public int Stock { get; set; }
        public DateTime DateModified { get; set; }

        public Product Product { get; set; }
    }
}
