using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DreamShop.Services.CouponAPI.Models.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
    }
}
