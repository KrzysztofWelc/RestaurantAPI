using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDBContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RestaurantService(RestaurantDBContext context, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public RestaurantDTO GetById(int id)
        {
            var restaurant = _context
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null) throw new NotFoundException("Restaurant not found.");

            var result = _mapper.Map<RestaurantDTO>(restaurant);
            return result;
        }

        public IEnumerable<RestaurantDTO> GetAll()
        {
            var restaurants = _context
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .ToList();

            var restaurantDTOs = _mapper.Map<List<RestaurantDTO>>(restaurants);
            return restaurantDTOs;
        }

        public int Create(CreateRestaurantDTO DTO)
        {
            var restaurant = _mapper.Map<Restaurant>(DTO);
            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();

            return restaurant.Id;
        }

        public void Delete(int id)
        {
            _logger.LogWarning($"Restaurant {id} DELETE action invoked");
            var restaurant = _context.Restaurants.FirstOrDefault(r => r.Id == id);

            if (restaurant is null) throw new NotFoundException("Restaurant not found.");

            _context.Remove(restaurant);
            _context.SaveChanges();

        }

        public void Update(int id, UpdateRestaurantDTO DTO)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(r => r.Id == id);

            if(restaurant is null)
            {
                throw new NotFoundException("Restaurant not found.");
            }

            restaurant.Name = DTO.Name;
            restaurant.Description = DTO.Description;
            restaurant.HasDelivery = DTO.HasDelivery;

            _context.SaveChanges();
        }
    }
}
