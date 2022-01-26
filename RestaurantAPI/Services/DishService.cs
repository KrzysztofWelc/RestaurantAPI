using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDTO DTO);
        public DishDTO GetById(int restaurantId, int dishId);
        public List<DishDTO> GetAll(int restaurantId);
        public void Delete(int restaurantId, int dishId);
    }

    public class DishService : IDishService
    {
        private readonly RestaurantDBContext _context;
        private readonly IMapper _mapper;

        public DishService(RestaurantDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public int Create(int restaurantId, CreateDishDTO DTO)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(r => r.Id == restaurantId);
            if (restaurant is null) throw new NotFoundException("Restaurant not found.");

            var dishEntity = _mapper.Map<Dish>(DTO);
            dishEntity.RestaurantId = restaurantId;
            _context.Dishes.Add(dishEntity);
            _context.SaveChanges();

            return dishEntity.Id;
        }

        public DishDTO GetById(int restaurantId, int dishId)
        {
            var r = _context.Restaurants.FirstOrDefault(r => r.Id == restaurantId);
            if (r is null) throw new NotFoundException("Restaurant not found");

            var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);

            if(dish is null || dish.RestaurantId != restaurantId)
            {
                throw new NotFoundException("Dish not found");
            }

            var dishDTO = _mapper.Map<DishDTO>(dish);
            return dishDTO;
        }

        public List<DishDTO> GetAll(int restaurantId)
        {
            var r = _context.Restaurants.Include(r => r.Dishes).FirstOrDefault(r => r.Id == restaurantId);
            
            if (r is null) throw new NotFoundException("Restaurant not found");

            var dishDTOs = _mapper.Map<List<DishDTO>>(r.Dishes);
            return dishDTOs;
        }
        
        public void Delete(int restaurantId, int dishId)
        {
            var r = _context.Restaurants.Include(r => r.Dishes).FirstOrDefault(r => r.Id == restaurantId);
            
            if (r is null) throw new NotFoundException("Restaurant not found");

            var d = _context.Dishes.FirstOrDefault(d => d.Id == dishId);
            if (d is null) throw new NotFoundException("Dish not found");
            _context.Remove(d);
            _context.SaveChanges();
        }
    }
}
