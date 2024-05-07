
using AutoMapper;
using DreamShop.Services.CouponAPI.Data;
using DreamShop.Services.CouponAPI.Models.DTOs;
using DreamShop.Services.CouponAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DreamShop.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly CouponAPIDbContext _db;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;

        public CouponAPIController(CouponAPIDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = _mapper.Map<List<CouponDto>>(await _db.Coupons.AsNoTracking().ToListAsync());
                _response.Result = data;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }
            return Ok(_response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            if (id == 0) return BadRequest("Invalid input request.");

            try
            {
                _response.Result = _mapper.Map<CouponDto>(await _db.Coupons.FirstOrDefaultAsync(c => c.Id == id));
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }

            return Ok(_response);
        }

        [HttpGet("{code}/code")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return BadRequest("Invalid input request.");

            try
            {
                _response.Result = _mapper.Map<CouponDto>(await _db.Coupons.FirstAsync(c => c.CouponCode == code));
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }

            return Ok(_response);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Post([FromBody] CouponDto coupon)
        {
            if (coupon == null) return BadRequest("Invalid input request.");

            try
            {
                var couponToSave = _mapper.Map<Coupon>(coupon);
                await _db.Coupons.AddAsync(couponToSave);
                await _db.SaveChangesAsync();
                _response.Result = _mapper.Map<CouponDto>(couponToSave);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }

            return Ok(_response);
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Put([FromBody] CouponDto coupon)
        {
            if (coupon == null) return BadRequest("Invalid input request.");

            try
            {
                var couponToUpdate = _mapper.Map<Coupon>(coupon);
                _db.Coupons.Update(couponToUpdate);
                await _db.SaveChangesAsync();
                _response.Result = _mapper.Map<CouponDto>(couponToUpdate);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }

            return Ok(_response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (id == 0) return BadRequest("Invalid input request!");

            try
            {
                var dataToDelete = await _db.Coupons.FirstAsync(c => c.Id == id);
                _db.Coupons.Remove(dataToDelete);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }

            return Ok(_response);
        }
    }
}
