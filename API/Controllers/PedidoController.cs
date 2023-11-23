using API.Dtos;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;



public class PedidoController : BaseApiController
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public PedidoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PedidoDto>>> Get()
    {
        var data = await unitOfWork.Pedidos.GetAllAsync();
        return mapper.Map<List<PedidoDto>>(data);
    }

    [HttpGet("{codigoPedido}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(int codigoPedido)
    {
        var data = await unitOfWork.Pedidos.GetByIdAsync(codigoPedido);
        return Ok(data);
    }

    [HttpGet("Consulta1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<object>>> Get1()
    {
        var datos = await unitOfWork.Pedidos.Consulta1();
        return mapper.Map<List<object>>(datos);
    }

 

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PedidoDto>> Post(PedidoDto dataDto)
    {
        var data = mapper.Map<Pedido>(dataDto);
        unitOfWork.Pedidos.Add(data);
        await unitOfWork.SaveAsync();
        if (data == null)
        {
            return BadRequest();
        }
        dataDto.CodigoPedido = data.CodigoPedido;
        return CreatedAtAction(nameof(Post), new { codigoPedido = dataDto.CodigoPedido }, dataDto);
    }

    [HttpPut("{codigoPedido}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PedidoDto>> Put(int codigoPedido, [FromBody] PedidoDto dataDto)
    {
        if (dataDto == null)
        {
            return NotFound();
        }
        var data = mapper.Map<Pedido>(dataDto);
        unitOfWork.Pedidos.Update(data);
        await unitOfWork.SaveAsync();
        return dataDto;
    }

    [HttpDelete("{codigoPedido}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int codigoPedido)
    {
        var data = await unitOfWork.Pedidos.GetByIdAsync(codigoPedido);
        if (data == null)
        {
            return NotFound();
        }
        unitOfWork.Pedidos.Remove(data);
        await unitOfWork.SaveAsync();
        return NoContent();
    }


}