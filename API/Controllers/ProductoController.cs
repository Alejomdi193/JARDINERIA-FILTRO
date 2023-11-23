using API.Dtos;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;

using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;



public class ProductoController : BaseApiController
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ProductoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> Get()
    {
        var data = await unitOfWork.Productos.GetAllAsync();
        return mapper.Map<List<ProductoDto>>(data);
    }

    [HttpGet("{codigoProducto}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductoDto>> Get(string codigoProducto)
    {
        var data = await unitOfWork.Productos.GetByIdAsync(codigoProducto);
        if (data == null)
        {
            return NotFound();
        }
        return mapper.Map<ProductoDto>(data);
    }
       [HttpGet("Consulta6")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<string> GetConsulta6()
    {

        var data = await unitOfWork.Productos.Consulta6();
        return data;
    }
    [HttpGet("Consulta10")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<object>>> Get10()
    {
        var datos = await unitOfWork.Productos.Consulta10();
        return mapper.Map<List<object>>(datos);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductoDto>> Post(ProductoDto dataDto)
    {
        var data = mapper.Map<Producto>(dataDto);
        unitOfWork.Productos.Add(data);
        await unitOfWork.SaveAsync();
        if (data == null)
        {
            return BadRequest();
        }
        dataDto.CodigoProducto = data.CodigoProducto;
        return CreatedAtAction(nameof(Post), new { codigoProducto = dataDto.CodigoProducto }, dataDto);
    }

    [HttpPut("{codigoProducto}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoDto>> Put(string codigoProducto, [FromBody] ProductoDto dataDto)
    {
        if (dataDto == null)
        {
            return NotFound();
        }
        var data = mapper.Map<Producto>(dataDto);
        unitOfWork.Productos.Update(data);
        await unitOfWork.SaveAsync();
        return dataDto;
    }

    [HttpDelete("{codigoProducto}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string codigoProducto)
    {
        var data = await unitOfWork.Productos.GetByIdAsync(codigoProducto);
        if (data == null)
        {
            return NotFound();
        }
        unitOfWork.Productos.Remove(data);
        await unitOfWork.SaveAsync();
        return NoContent();
    }
}