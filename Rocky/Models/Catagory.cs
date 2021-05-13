﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Models
{
    public class Catagory
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [DisplayName("Display Order")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage ="Greater than zero")]
        public int DisplayOrder { get; set; }


    }
}