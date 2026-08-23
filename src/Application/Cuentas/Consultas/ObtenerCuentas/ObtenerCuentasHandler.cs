using Application.Cuentas.Comandos.CrearCuenta;
using System;
using System.Collections.Generic;
using System.Text;

using Application.Abstracciones;
using Application.DTOs;
using MediatR;

namespace Application.Cuentas.Consultas.ObtenerCuentas;

public class ObtenerCuentasHandler
    : IRequestHandler<
        ObtenerCuentasQuery,
        IReadOnlyCollection<CuentaDto>>
{
    private readonly ICuentaRepository _cuentaRepository;

    public ObtenerCuentasHandler(ICuentaRepository cuentaRepository)
    {
        _cuentaRepository = cuentaRepository;
    }

    public async Task<IReadOnlyCollection<CuentaDto>> Handle(
        ObtenerCuentasQuery request,
        CancellationToken cancellationToken)
    {
        var cuentas = await _cuentaRepository.ObtenerTodasAsync(
            cancellationToken);

        return cuentas
            .Select(cuenta => new CuentaDto
            {
                Id = cuenta.Id,
                NumeroCuenta = cuenta.NumeroCuenta,
                Titular = cuenta.Titular,
                Saldo = cuenta.Saldo,
                Estado = cuenta.Estado,
                FechaCreacionUtc = cuenta.FechaCreacionUtc
            })
            .ToList();
    }
}