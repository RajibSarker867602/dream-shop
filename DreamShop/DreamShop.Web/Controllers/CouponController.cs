using DreamShop.Web.Models.DTOs;
using DreamShop.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;

namespace DreamShop.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> Index()
        {
            List<CouponDto> coupons = new List<CouponDto>();
            var data = await _couponService.GetAllCoupons();

            if (data != null && data.IsSuccess)
            {
                coupons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(data.Result));
            }

            return View(coupons);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CouponDto coupon)
        {
            if (ModelState.IsValid)
            {
                var response = await _couponService.CreateCoupon(coupon);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(coupon);
        }
        
        public async Task<IActionResult> Delete(long couponId)
        {
            var existingCoupon = await _couponService.GetCouponById(couponId);
            if (existingCoupon != null && existingCoupon.IsSuccess)
            {
                var couponToDelete = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(existingCoupon.Result));
                return View(couponToDelete);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CouponDto coupon)
        {
            var existingCoupon = await _couponService.DeleteCoupon(coupon.Id);
            if (existingCoupon != null && existingCoupon.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }
    }
}
