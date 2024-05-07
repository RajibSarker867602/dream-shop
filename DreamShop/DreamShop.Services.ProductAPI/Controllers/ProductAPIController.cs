using AutoMapper;
using DreamShop.Services.ProductAPI.Data;
using DreamShop.Services.ProductAPI.Models;
using DreamShop.Services.ProductAPI.Models.Dtos;
using DreamShop.Services.ProductAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DreamShop.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductAPIController : ControllerBase
    {
        private readonly ProductAPIDbContext _db;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;

        public ProductAPIController(ProductAPIDbContext db, IMapper mapper)
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
                var data = _mapper.Map<List<ProductDto>>(await _db.Products.AsNoTracking().ToListAsync());
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
                _response.Result = _mapper.Map<ProductDto>(await _db.Products.FirstOrDefaultAsync(c => c.Id == id));
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
        public async Task<IActionResult> Post([FromBody] ProductDto product)
        {
            if (product == null) return BadRequest("Invalid input request.");

            try
            {
                var productToSave = _mapper.Map<Product>(product);
                await _db.Products.AddAsync(productToSave);
                await _db.SaveChangesAsync();
                _response.Result = _mapper.Map<ProductDto>(productToSave);
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
        public async Task<IActionResult> Put([FromBody] ProductDto product)
        {
            if (product == null) return BadRequest("Invalid input request.");

            try
            {
                var productToSave = _mapper.Map<Product>(product);
                _db.Products.Update(productToSave);
                await _db.SaveChangesAsync();
                _response.Result = _mapper.Map<ProductDto>(productToSave);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
            }

            return Ok(_response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(long id)
        {
            if (id == 0) return BadRequest("Invalid input request!");

            try
            {
                var dataToDelete = await _db.Products.FirstAsync(c => c.Id == id);
                _db.Products.Remove(dataToDelete);
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
