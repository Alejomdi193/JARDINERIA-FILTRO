using API.Dtos;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;



public class DetallePedidoController : BaseApiController
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public DetallePedidoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<DetallePedidoDto>>> Get()
    {
        var data = await unitOfWork.DetallePedidos.GetAllAsync();
        return mapper.Map<List<DetallePedidoDto>>(data);
    }


    [HttpGet("{codigoPedido}/{codigoProducto}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DetallePedidoDto>> Get(int codigoPedido, string codigoProducto)
    {
        var data = await unitOfWork.DetallePedidos.GetByIdx1Async(codigoPedido, codigoProducto);
        if (data == null)
        {
            return NotFound();
        }

        return mapper.Map<DetallePedidoDto>(data);
    }


    [HttpGet("Consulta4")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<object>>> Get4()
    {
        var datos = await unitOfWork.DetallePedidos.Consulta4();
        return mapper.Map<List<object>>(datos);
    }

    [HttpGet("Consulta5")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<object>>> Get5()
    {
        var datos = await unitOfWork.DetallePedidos.Consulta4();
        return mapper.Map<List<object>>(datos);
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DetallePedidoDto>> Post(DetallePedidoDto dataDto)
    {
        var data = mapper.Map<DetallePedido>(dataDto);
        unitOfWork.DetallePedidos.Add(data);
        try
        {
            await unitOfWork.SaveAsync();
            return CreatedAtAction(nameof(Post), new { /* información para la respuesta */ }, dataDto);
        }
        catch (Exception ex)
        {
            return BadRequest("Error al crear el registro: " + ex.Message);
        }
    }

    [HttpPut("{codigoPedido}/{codigoProducto}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DetallePedidoDto>> Put(int codigoPedido, string codigoProducto, [FromBody] DetallePedidoDto dataDto)
    {
        if (dataDto == null)
        {
            return NotFound();
        }
        var existingData = await unitOfWork.DetallePedidos.GetByIdx1Async(codigoPedido, codigoProducto);
        if (existingData == null)
        {
            return NotFound();
        }
        mapper.Map(dataDto, existingData);
        unitOfWork.DetallePedidos.Update(existingData);
        await unitOfWork.SaveAsync();
        return dataDto;
    }

    [HttpDelete("{codigoPedido}/{codigoProducto}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int codigoPedido, string codigoProducto)
    {
        var existingData = await unitOfWork.DetallePedidos.GetByIdx1Async(codigoPedido, codigoProducto);
        if (existingData == null)
        {
            return NotFound();
        }
        unitOfWork.DetallePedidos.Remove(existingData);
        await unitOfWork.SaveAsync();
        return NoContent();
    }
}