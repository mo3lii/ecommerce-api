using ecommerce.DTO;
using ecommerce.HelperClasses;
using ecommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        OrderService orderService;
        Mapper mapper;
        public OrderController(OrderService orderService,Mapper mapper)
        {
            this.orderService=orderService;
            this.mapper=mapper;
        }
        [HttpPost]
        [Authorize(Roles ="user")]
        public IActionResult Add(OrderPostDTO orderDTO)
        {
            var userIdClaim = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            //var userIdClaim = Guid.Parse("5d62baf8-112c-4fa4-83b5-138f15aa2122");
            orderService.AddOrder(orderDTO, userIdClaim);
            return Ok();
        }
        [HttpGet("products")]
        public IActionResult Products()
        {
            var userIdClaim = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            //var userIdClaim = Guid.Parse("5d62baf8-112c-4fa4-83b5-138f15aa2122");
            var products = mapper.CartToCartGetDTO(orderService.GetOrderCartProducts(userIdClaim).ToList());
            return Ok(products);
        }
    }
    
}
