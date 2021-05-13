using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Models.ViewModels
{
    public class HomeVm
    {
        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Catagory> Catagories { get; set; }
    }
}
