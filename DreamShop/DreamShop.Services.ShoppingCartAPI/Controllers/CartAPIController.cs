using AutoMapper;
using DreamShop.Services.ShoppingCartAPI.Data;
using DreamShop.Services.ShoppingCartAPI.IService;
using DreamShop.Services.ShoppingCartAPI.Models;
using DreamShop.Services.ShoppingCartAPI.Models.Dto;
using DreamShop.Services.ShoppingCartAPI.Models.DTOs;
using DreamShop.Services.ShoppingCartAPI.Services.Interface;
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
        private readonly ICouponService _couponService;
        private readonly IProductService _productService;
        private ResponseDto _responseDto;

        public CartAPIController(ShoppingCartAPIDbContext db, IMapper mapper, ICouponService couponService, IProductService productService)
        {
            _db = db;
            _mapper = mapper;
            _couponService = couponService;
            _productService = productService;
            _responseDto = new ResponseDto();
        }

        [HttpGet("{userId}/carts")]
        public async Task<ResponseDto> GetUserCarts(string userId)
        {
            try
            {
                if (userId is null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = $"User id is not provided! Please provide valid user id.";
                    return _responseDto;
                }

                var cart = await _db.CartHeaders.Include(c=> c.CartDetails).FirstOrDefaultAsync(c => c.UserId == userId);
                if (cart is null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = $"No shopping cart is found for the user id: {userId}";
                    return _responseDto;
                }

                var dataToReturn = _mapper.Map<CartHeaderDto>(cart);
                var products = await _productService.GetProductsAsync();
                foreach (var details in dataToReturn.CartDetails)
                {
                    details.Product = products.FirstOrDefault(c => c.Id == details.ProductId);
                    dataToReturn.CartTotal += details.Product.Price * details.Count;
                }

                if (!string.IsNullOrEmpty(cart.CouponCode))
                {
                    var existingCoupon = await _couponService.GetCouponByCode(cart.CouponCode);
                    if (existingCoupon != null && dataToReturn.CartTotal > existingCoupon.MinAmount)
                    {
                        dataToReturn.CartTotal -= existingCoupon.DiscountAmount;
                        dataToReturn.Discount = existingCoupon.DiscountAmount;
                    }
                }

                _responseDto.Result = dataToReturn;
            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = e.Message;
            }

            return _responseDto;
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

        [HttpPost("removeCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] long detailsId)
        {
            try
            {
                var detailsToRemove = await _db.CartDetails.FirstOrDefaultAsync(c => c.Id == detailsId);
                if (detailsToRemove is null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = $"No cart details found by this id: {detailsId}";
                    return _responseDto;
                }

                var totalCartCount = await _db.CartDetails.CountAsync(c => c.CartHeaderId == detailsToRemove.CartHeaderId);
                _db.Remove(detailsToRemove);
                if (totalCartCount == 1)
                {
                    var headerToRemove =
                        await _db.CartHeaders.FirstOrDefaultAsync(c => c.Id == detailsToRemove.CartHeaderId);
                    _db.Remove(headerToRemove);
                }

                await _db.SaveChangesAsync();
                _responseDto.Message = "Delete operation successfully completed.";
                _responseDto.IsSuccess = true;
            }
            catch (Exception e)
            {
                _responseDto.Message = e.Message;
                _responseDto.IsSuccess = false;
            }

            return _responseDto;
        }

        [HttpPost("addCoupon")]
        public async Task<ResponseDto> AddCoupon([FromBody] CartHeaderDto cart)
        {
            try
            {
                var existingCart = await _db.CartHeaders.FirstOrDefaultAsync(c => c.UserId == cart.UserId);
                if (existingCart is null)
                {
                    _responseDto.Message = $"No cart information is found for the user id: {cart.UserId}";
                    _responseDto.IsSuccess = false;
                    return _responseDto;
                }

                existingCart.CouponCode = cart.CouponCode;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _responseDto.Message = e.Message;
                _responseDto.IsSuccess = false;
            }

            return _responseDto;
        }
    }
}
