using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Switch_and_Shift.Models
{
	public class Products
	{
		[Key]
		public int Product_ID { get; set; }


		[Required]
		[DisplayName("Category")]
		public string product_category { get; set; }

		[Required]
		[DisplayName("Price")]
		public int product_price { get; set; }

		[Required]
		[DisplayName("Brand")]
		public string product_brand { get; set; }

		[Required]
		[DisplayName("Model")]
		public string product_model { get; set; }


		[DisplayName("Details")]
		public string product_details { get; set; }
		[DisplayName("Warranty")]
		public string product_warranty { get; set; }
		[DisplayName("Usage")]
		public string product_usage { get; set; }

		[DisplayName("Condition")]
		public string product_condition { get; set; }


		public string user_location { get; set; }
		public int UserId { get; set; }


		[DisplayName("Image Name")]
		public string image_name { get; set; }

		[NotMapped]
		[Required]
		[DisplayName("Upload File")]
		public IFormFile ImageFile { get; set; }


		public string post_date { get; set; }


	}

}
