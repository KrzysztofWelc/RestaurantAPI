using RestaurantAPI.Models;
using System.Collections.Generic;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDTO DTO);
        IEnumerable<RestaurantDTO> GetAll();
        RestaurantDTO GetById(int id);
        public void Delete(int id);
        public void Update(int id, UpdateRestaurantDTO DTO);
    }
}