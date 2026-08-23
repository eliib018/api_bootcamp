using Application.Cuentas.Comandos.ActualizarCuenta;
using Application.Cuentas.Comandos.CrearCuenta;
using Application.Cuentas.Comandos.EliminarCuenta;
using Application.Cuentas.Consultas.ObtenerCuentaPorId;
using Application.Cuentas.Consultas.ObtenerCuentas;
using Application.DTOs;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiBootcamp.Controllers;

[ApiController]
[Route("api/v1/cuentas")]
public class CuentasController : ControllerBase
{
    private readonly ISender _sender;

    public CuentasController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<CuentaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerTodas(
        CancellationToken cancellationToken)
    {
        var resultado = await _sender.Send(
            new ObtenerCuentasQuery(),
            cancellationToken);

        return Ok(resultado);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CuentaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerPorId(
        Guid id,
        CancellationToken cancellationToken)
    {
        var resultado = await _sender.Send(
            new ObtenerCuentaPorIdQuery(id),
            cancellationToken);

        if (resultado is null)
        {
            return NotFound();
        }

        return Ok(resultado);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CuentaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Crear(
        [FromBody] CrearCuentaRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CrearCuentaCommand(
            request.NumeroCuenta,
            request.Titular,
            request.Saldo,
            request.Estado);

        var resultado = await _sender.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = resultado.Id },
            resultado);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CuentaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarCuentaRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ActualizarCuentaCommand(
            id,
            request.Titular,
            request.Saldo,
            request.Estado);

        var resultado = await _sender.Send(
            command,
            cancellationToken);

        if (resultado is null)
        {
            return NotFound();
        }

        return Ok(resultado);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Eliminar(
        Guid id,
        CancellationToken cancellationToken)
    {
        var eliminado = await _sender.Send(
            new EliminarCuentaCommand(id),
            cancellationToken);

        if (!eliminado)
        {
            return NotFound();
        }

        return NoContent();
    }
}

public record CrearCuentaRequest(
    string NumeroCuenta,
    string Titular,
    decimal Saldo,
    EstadoCuenta Estado);

public record ActualizarCuentaRequest(
    string Titular,
    decimal Saldo,
    EstadoCuenta Estado);