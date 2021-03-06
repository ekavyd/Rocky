using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        public string ShortDesc{ get; set; }

        public string Description { get; set; }
        
        [Range(1, int.MaxValue)]
        public double Price { get; set; }
        
        public string Image { get; set; }
        
        [Display(Name="Catagory Type")]
        public int CatagoryId { get; set; }

        [Display(Name = "Application Type")]
        public int ApplicationId { get; set; }

        [ForeignKey("CatagoryId")]
        public virtual Catagory Catagory { get; set; }

        [ForeignKey("ApplicationId")]
        public virtual Application Application { get; set; }


    }
}
