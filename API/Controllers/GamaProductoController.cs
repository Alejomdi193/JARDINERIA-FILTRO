using API.Dtos;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;



public class GamaProductoController : BaseApiController
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GamaProductoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    [HttpGet]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<GamaProductoDto>>> Get()
    {
        var data = await unitOfWork.GamaProductos.GetAllAsync();
        return mapper.Map<List<GamaProductoDto>>(data);
    }

    [HttpGet("{gama}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GamaProductoDto>> Get(string gama)
    {
        var data = await unitOfWork.GamaProductos.GetByIdAsync(gama);
        if (data == null)
        {
            return NotFound();
        }
        return mapper.Map<GamaProductoDto>(data);
    }

    

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GamaProductoDto>> Post(GamaProductoDto dataDto)
    {
        var data = mapper.Map<GamaProducto>(dataDto);
        unitOfWork.GamaProductos.Add(data);
        await unitOfWork.SaveAsync();
        if (data == null)
        {
            return BadRequest();
        }
        dataDto.Gama = data.Gama;
        return CreatedAtAction(nameof(Post), new { gama = dataDto.Gama }, dataDto);
    }

    [HttpPut("{gama}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GamaProductoDto>> Put(string gama, [FromBody] GamaProductoDto dataDto)
    {
        if (dataDto == null)
        {
            return NotFound();
        }
        var data = mapper.Map<GamaProducto>(dataDto);
        unitOfWork.GamaProductos.Update(data);
        await unitOfWork.SaveAsync();
        return dataDto;
    }

    [HttpDelete("{gama}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string gama)
    {
        var data = await unitOfWork.GamaProductos.GetByIdAsync(gama);
        if (data == null)
        {
            return NotFound();
        }
        unitOfWork.GamaProductos.Remove(data);
        await unitOfWork.SaveAsync();
        return NoContent();
    }
}