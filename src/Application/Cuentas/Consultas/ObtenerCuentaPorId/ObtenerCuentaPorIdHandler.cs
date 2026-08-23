using System;
using System.Collections.Generic;
using System.Text;

using Application.Abstracciones;
using Application.DTOs;
using MediatR;

namespace Application.Cuentas.Consultas.ObtenerCuentaPorId;

public class ObtenerCuentaPorIdHandler
    : IRequestHandler<ObtenerCuentaPorIdQuery, CuentaDto?>
{
    private readonly ICuentaRepository _cuentaRepository;

    public ObtenerCuentaPorIdHandler(ICuentaRepository cuentaRepository)
    {
        _cuentaRepository = cuentaRepository;
    }

    public async Task<CuentaDto?> Handle(
        ObtenerCuentaPorIdQuery request,
        CancellationToken cancellationToken)
    {
        var cuenta = await _cuentaRepository.ObtenerPorIdAsync(
            request.Id,
            cancellationToken);

        if (cuenta is null)
            return null;

        return new CuentaDto
        {
            Id = cuenta.Id,
            NumeroCuenta = cuenta.NumeroCuenta,
            Titular = cuenta.Titular,
            Saldo = cuenta.Saldo,
            Estado = cuenta.Estado,
            FechaCreacionUtc = cuenta.FechaCreacionUtc
        };
    }
}