using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebNews.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter news title")]
        public string title { get; set; }
        [Required(ErrorMessage = "Enter news Date")]
        public DateTime NewsDate { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "Enter news Topic")]
        public string Topic { get; set; }
        [ForeignKey("category")]
        public int categoryId { get; set; }
        public Categories category { get; set; }
    }
}
