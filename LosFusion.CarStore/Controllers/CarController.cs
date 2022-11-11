using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LosFusion.CarStore.BusinessLogicLayer.Entities;
using LosFusion.CarStore.BusinessLogicLayer.Interfaces;

namespace LosFusion.CarStore.ServiceLayer.Controller;

[Route("api/[controller]")]
[ApiController]
public class CarController : ControllerBase
{
    readonly ICarRepository _repo;

    public CarController(ICarRepository repo)
    {
        _repo = repo;
    }

    //[Authorize]
    // GET: api/<CarController>
    [HttpGet]
    public async Task<IEnumerable<CarEntity>> Get()
    {
        return await _repo.GetAsync();
    }

    // GET api/<CarController>/5
    [HttpGet("{year}")]
    public async Task<IEnumerable<CarEntity>> Get(int year)
    {
        return await _repo.GetByYearAsync(year);
    }

    // POST api/<CarController>
    [HttpPost]
    public async Task<CarEntity> Post([FromBody] CarEntity model)
    {
        return await _repo.AddAsync(model);
    }

    // PUT api/<CarController>/5
    [HttpPut("{id}")]
    public async Task<CarEntity?> Put(int id, [FromBody] CarEntity model)
    {
        return await _repo.UpdateAsync(id, model);
    }

    // PATCH api/<CarController>/5
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<CarEntity> patchEntity)
    {
        var entity = await _repo.GetAsync(id);

        if (entity == null)
        {
            return NotFound();
        }

        patchEntity.ApplyTo(entity, ModelState);
        await _repo.UpdateAsync(id, entity);

        return Ok(entity);
    }

    // DELETE api/<CarController>/5
    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _repo.DeleteAsync(id);
    }
}
