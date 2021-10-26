using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebNews.Models
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Category Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter Category Discription")]
        public string Discription { get; set; }

        public List<News> Newss { get; set; }
    }
}
