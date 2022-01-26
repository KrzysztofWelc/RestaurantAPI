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
    [Route("api/account")]
    [ApiController]
    public class AccontController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccontController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public ActionResult CreateUser([FromBody] RegisterUserDTO DTO)
        {
            _service.RegisterUser(DTO);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult LoginUser([FromBody] LoginDTO DTO)
        {
            string token = _service.GenerateJWT(DTO);
            return Ok(token);
        }
    }
}
