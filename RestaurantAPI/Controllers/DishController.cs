using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _service;

        public DishController(IDishService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishDTO DTO)
        {
            var resId = _service.Create(restaurantId, DTO);

            return Created($"api/restaurant/{restaurantId}/dish/{resId}", null);
        }

        [HttpGet("{dishId}")]
        public ActionResult<DishDTO> GetDish([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dish = _service.GetById(restaurantId, dishId);

            return Ok(dish);
        }
        
        [HttpGet]
        public ActionResult<DishDTO> GetAllDishes([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dishes = _service.GetAll(restaurantId);

            return Ok(dishes);
        }
        
        [HttpDelete("{dishId}")]
        public ActionResult DeleteDish([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            _service.Delete(restaurantId, dishId);

            return NoContent();
        }
    }
}
