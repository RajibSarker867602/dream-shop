using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;
using DreamShop.Services.ShoppingCartAPI.Models.Dtos;

namespace DreamShop.Services.ShoppingCartAPI.Models
{
    public class CartDetails
    {
        [Key]
        public long Id { get; set; }

        public long CartHeaderId { get; set; }
        public CartHeader CartHeader { get; set; }
        public long ProductId { get; set; }
        [NotMapped] public ProductDto Product { get; set; }
        public int Count { get; set; }
    }
}
