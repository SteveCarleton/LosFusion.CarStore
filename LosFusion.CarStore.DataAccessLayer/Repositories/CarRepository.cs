using Microsoft.EntityFrameworkCore;
using LosFusion.CarStore.BusinessLogicLayer.Entities;
using LosFusion.CarStore.BusinessLogicLayer.Interfaces;

namespace LosFusion.CarStore.DataAccessLayer.Repositories
{
    public class CarRepository : ICarRepository
    {
        readonly CarDbContext _context;

        public CarRepository(CarDbContext context)
        {
            _context = context;
        }

        public async Task<List<CarEntity>> GetAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<CarEntity?> GetAsync(int id)
        {
            try
            {
                return await _context.Cars.FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception exc)
            {
                throw new ApplicationException($"Error getting Cars - Id: {id}", exc);
            }
        }

        public async Task<List<CarEntity>> GetByYearAsync(int year)
        {
            try
            {
                var query = from e in _context.Cars
                             where e.Year == year
                             select e;

                return await query.ToListAsync();
            }
            catch (Exception exc)
            {
                throw new ApplicationException($"Error getting Cars - Year: {year}", exc);
            }
        }

        public async Task<CarEntity> AddAsync(CarEntity entity)
        {
            try
            {
                _context.Cars.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception exc)
            {
                throw new ApplicationException($"Error adding Car - Model: {entity.Model}", exc);
            }
        }

        //public async Task<CarEntity> UpdateAsync(CarEntity entity)
        //{
        //    try
        //    {
        //        _context.Entry(entity).State = EntityState.Modified;
        //        await _context.SaveChangesAsync();
        //        return entity;
        //    }
        //    catch (Exception exc)
        //    {
        //        throw new ApplicationException($"Error updating Car - Model: {entity.Model}", exc);
        //    }
        //}
        public async Task<CarEntity> UpdateAsync(int id, CarEntity entity)
        {
            try
            {
                var orig = await _context.Cars.FirstOrDefaultAsync(e => e.Id == id);
                _context.Entry(orig).CurrentValues.SetValues(entity);
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception exc)
            {
                throw new ApplicationException($"Error updating Car - Model: {entity.Model}", exc);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = new CarEntity { Id = id };
            _context.Cars.Attach(entity);
            _context.Entry(entity).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}