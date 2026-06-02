using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Simulation2.Models
{
    public class Worker : BaseEntity
    {
        [Required]
        [MinLength(3, ErrorMessage =" 3 herfden azdi")]
        [MaxLength(100, ErrorMessage = " cox boyukdu")]
        public string Name { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = " 3 herfden azdi")]
        [MaxLength(100, ErrorMessage = " cox boyukdu")]
        public string Job {  get; set; }
        public string Image { get; set; }
    }
}
