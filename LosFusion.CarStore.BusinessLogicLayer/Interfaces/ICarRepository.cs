using LosFusion.CarStore.BusinessLogicLayer.Entities;

namespace LosFusion.CarStore.BusinessLogicLayer.Interfaces;

public interface ICarRepository
{
    Task<CarEntity> AddAsync(CarEntity entity);
    Task DeleteAsync(int id);
    Task<List<CarEntity>> GetAsync();
    Task<List<CarEntity>> GetByYearAsync(int year);
    Task<CarEntity> UpdateAsync(int id, CarEntity entity);
}