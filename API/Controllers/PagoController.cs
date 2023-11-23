using API.Dtos;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;


public class PagoController : BaseApiController
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public PagoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    [HttpGet]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<PagoDto>>> Get()
    {
        var data = await unitOfWork.Pagos.GetAllAsync();
        return mapper.Map<List<PagoDto>>(data);
    }


    [HttpGet("{codigoCliente}/{idTransaccion}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DetallePedidoDto>> Get(int codigoCliente, string idTransaccion)
    {
        var data = await unitOfWork.Pagos.GetByIdx2Async(codigoCliente, idTransaccion);
        if (data == null)
        {
            return NotFound();
        }

        return mapper.Map<DetallePedidoDto>(data);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagoDto>> Post(PagoDto dataDto)
    {
        var data = mapper.Map<Pago>(dataDto);
        unitOfWork.Pagos.Add(data);
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

    [HttpPut("{codigoCliente}/{idTransaccion}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PagoDto>> Put(int codigoCliente, string idTransaccion, [FromBody] PagoDto dataDto)
    {
        if (dataDto == null)
        {
            return NotFound();
        }
        var existingData = await unitOfWork.Pagos.GetByIdx2Async(codigoCliente, idTransaccion);
        if (existingData == null)
        {
            return NotFound();
        }
        mapper.Map(dataDto, existingData);
        unitOfWork.Pagos.Update(existingData);
        await unitOfWork.SaveAsync();
        return dataDto;
    }

    [HttpDelete("{codigoCliente}/{idTransaccion}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int codigoCliente, string idTransaccion)
    {
        var data = await unitOfWork.Pagos.GetByIdx2Async(codigoCliente, idTransaccion);
        if (data == null)
        {
            return NotFound();
        }
        unitOfWork.Pagos.Remove(data);
        await unitOfWork.SaveAsync();
        return NoContent();
    }


}