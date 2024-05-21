using ecommerce.DTO;
using ecommerce.HelperClasses;
using ecommerce.Models;
using ecommerce.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ecommerce.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private UnitOfWork unit;
        Mapper mapper { get; }

        public CartController(UnitOfWork unit, IConfiguration configuration, Mapper mapper)
        {
            _configuration = configuration;
            this.unit = unit;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "user")]
        public IActionResult get()
        {
            var userIdClaim = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var cartProdList = unit.UsersCartsRepository.GetAllByUserId(userIdClaim);
            var CartProdDTOList = mapper.CartToCartGetDTO(cartProdList);
            return Ok(CartProdDTOList);
        }
        [HttpGet("count")]
        [Authorize(Roles = "user")]
        public IActionResult getCount()
        {
            var userIdClaim = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var cartcount = unit.UsersCartsRepository.GetAllByUserId(userIdClaim).Count();
            return Ok(cartcount);
        }
        [HttpPut]
        [Authorize(Roles = "user")]
        public IActionResult updateQuantity(QtyUpdateDTO qtyupdate)
        {
            var userIdClaim = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var cartProd = unit.UsersCartsRepository.GetFirstByFilter(c=>c.UserId== userIdClaim&&c.ProductId== qtyupdate.productId);
            if (cartProd != null)
            {
                cartProd.Quantity = qtyupdate.qty;
                unit.UsersCartsRepository.Update(cartProd);
                unit.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        [Authorize(Roles = "user")]
        public IActionResult addProduct([FromBody]int id)
        {
            var userIdClaim = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var product = unit.ProductRepository.GetById(id);
            var user = unit.UserRepository.GetById(userIdClaim);
            if(product != null && user!=null)
            {
                var cartProd = new UserCart()
                {
                    ProductId = id,
                    UserId = userIdClaim,
                    Quantity = 1,
                };
                if (unit.UsersCartsRepository.GetFirstByFilter(p => p.ProductId == id && p.UserId==userIdClaim) == null)
                {
                    unit.UsersCartsRepository.Insert(cartProd);
                    unit.SaveChanges();
                }
     
                return Ok();
            }
            return BadRequest();

        }
        [HttpDelete]
        [Authorize(Roles = "user")]
        public IActionResult deleteProduct([FromQuery] int id)
        {
            var userIdClaim = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var product = unit.UsersCartsRepository.GetById(id);
            var user = unit.UserRepository.GetById(userIdClaim);
            if (product != null && user != null)
            {
                unit.UsersCartsRepository.Delete(product);
                unit.SaveChanges();
                return Ok();
            }
            return BadRequest();

        }

    }
}
