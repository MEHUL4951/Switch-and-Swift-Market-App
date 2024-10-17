using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Switch_and_Shift.Models
{
    public class UserReview
    {
        [Key]
        public int review_id { get; set; }

        [Required]
        public string reviewee_name { get; set; }

        [Required]
        public string reviewee_email { get; set; }
        [Required]
        public string reviewer_name { get; set; }

        [Required]
        public string reviewer_email { get; set; }


        [Required]
        public string review_description { get; set; }
        public string post_date { get; set; }

    }
}
