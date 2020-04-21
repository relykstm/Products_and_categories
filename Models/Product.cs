using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Products_and_Catagories.Models
{
    public class Product
    {
        [Key]
        public int ProductId {get;set;}

        [Required]
        public string Name {get;set;}
        [Required]
        public string Description {get;set;}
        [Required]
        public decimal Price {get;set;}

        public List<Association> Categories {get;set;}
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } =  DateTime.Now;

    }
}