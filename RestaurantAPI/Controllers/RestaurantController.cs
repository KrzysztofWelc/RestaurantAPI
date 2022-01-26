using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantService _service;
        
        public RestaurantController(IRestaurantService service, IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }
        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDTO>> GetAllRestaurants()
        {
            var restaurantsDTOs = _service.GetAll();
            return Ok(restaurantsDTOs);
        }

        [HttpGet("{id}")]
        public ActionResult<RestaurantDTO> GetRestaurantByID([FromRoute] int id)
        {
            var restaurantDTO = _service.GetById(id);

            return Ok(restaurantDTO);
            
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDTO DTO)
        {
            var id = _service.Create(DTO);

            return Created($"/api/restaurants/{id}", null);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteRestaurant([FromRoute] int id)
        {
            _service.Delete(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult UpdateRestaurant([FromBody] UpdateRestaurantDTO DTO, [FromRoute] int id)
        {
             _service.Update(id, DTO);
            
            return Ok();
        }
    }
}
