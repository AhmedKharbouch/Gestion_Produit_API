using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todoItemProject.Models
{
    
    public class Product
    {
        public int Id { get; set; }

        public string label { get; set; } = string.Empty;

        public int quantite { get; set; }
        public double prixHT { get; set; }

        public double prixTT { get; set; }
        

        //categoryId attribute is a foreign key
        
        public int categoryId { get; set; }

        //category attribute is a navigation property
        public Category? category { get; set; }



    }
}
