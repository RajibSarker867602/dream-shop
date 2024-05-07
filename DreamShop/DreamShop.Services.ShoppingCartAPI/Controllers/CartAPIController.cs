using AutoMapper;
using DreamShop.Services.ShoppingCartAPI.Data;
using DreamShop.Services.ShoppingCartAPI.Models;
using DreamShop.Services.ShoppingCartAPI.Models.Dto;
using DreamShop.Services.ShoppingCartAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DreamShop.Services.ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly ShoppingCartAPIDbContext _db;
        private readonly IMapper _mapper;
        private ResponseDto _responseDto;

        public CartAPIController(ShoppingCartAPIDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpPost("upsert")]
        public async Task<ResponseDto> Upsert([FromBody] CartHeaderDto cart)
        {
            try
            {
                var existingCart = await _db.CartHeaders.Include(c => c.CartDetails)
                    .FirstOrDefaultAsync(c => c.UserId == cart.UserId);
                if (existingCart is null)
                {
                    // create cart with details
                    var cartToSave = _mapper.Map<CartHeader>(cart);
                    await _db.CartHeaders.AddAsync(cartToSave);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    var existingProduct =
                        existingCart.CartDetails.FirstOrDefault(c => c.ProductId == cart.CartDetails.First().ProductId);
                    if (existingProduct is null)
                    {
                        // add details
                        var details = _mapper.Map<ICollection<CartDetails>>(cart.CartDetails);
                        existingCart.CartDetails.Add(details.First());
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        // update quantity
                        existingProduct.Count += cart.CartDetails.First().Count;
                        await _db.SaveChangesAsync();
                    }
                }
                _responseDto.Result = cart;
            }
            catch (Exception e)
            {
                _responseDto.Message = e.Message.ToString();
                _responseDto.IsSuccess = false;
                return _responseDto;
            }

            return _responseDto;
        }
    }
}
