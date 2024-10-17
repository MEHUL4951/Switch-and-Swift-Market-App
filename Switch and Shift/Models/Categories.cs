using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Switch_and_Shift.Models
{
    public class Categories
    {
        [Key]
        public int categories_id { get; set; }

        [Required]
        public string categories_name { get; set; }
    }
}
