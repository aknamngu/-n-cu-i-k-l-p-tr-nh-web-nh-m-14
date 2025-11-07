using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SweetAndSavoryBakery.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
