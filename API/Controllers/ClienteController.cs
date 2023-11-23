using API.Dtos;
using AutoMapper;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;


public class ClienteController : BaseApiController
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ClienteController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> Get()
    {
        var data = await unitOfWork.Clientes.GetAllAsync();
        return mapper.Map<List<ClienteDto>>(data);
    }

    [HttpGet("{CodigoCliente}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClienteDto>> Get(int codigoCliente)
    {
        var data = await unitOfWork.Clientes.GetByIdAsync(codigoCliente);
        if (data == null)
        {
            return NotFound();
        }
        return mapper.Map<ClienteDto>(data) ;
    }

    
    [HttpGet("Consulta2")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<object>>> Get2()
    {
        var datos = await unitOfWork.Clientes.Consulta2();
        return mapper.Map<List<object>>(datos);
    }

 

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClienteDto>> Post(ClienteDto dataDto)
    {
        var data = mapper.Map<Cliente>(dataDto);
        unitOfWork.Clientes.Add(data);
        await unitOfWork.SaveAsync();
        if (data == null)
        {
            return BadRequest();
        }
        dataDto.CodigoCliente = data.CodigoCliente;
        return CreatedAtAction(nameof(Post), new { codigoCliente = dataDto.CodigoCliente }, dataDto);
    }

    [HttpPut("{codigoCliente}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteDto>> Put(int codigoCliente, [FromBody] ClienteDto dataDto)
    {
        if (dataDto == null)
        {
            return NotFound();
        }
        var data = mapper.Map<Cliente>(dataDto);
        unitOfWork.Clientes.Update(data);
        await unitOfWork.SaveAsync();
        return dataDto;
    }

    [HttpDelete("{codigoCliente}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int codigoCliente)
    {
        var data = await unitOfWork.Clientes.GetByIdAsync(codigoCliente);
        if (data == null)
        {
            return NotFound();
        }
        unitOfWork.Clientes.Remove(data);
        await unitOfWork.SaveAsync();
        return NoContent();
    }




}