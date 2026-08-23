using Application.DTOs;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

using Application.Abstracciones;

namespace Application.Cuentas.Comandos.ActualizarCuenta;

public class ActualizarCuentaHandler
    : IRequestHandler<ActualizarCuentaCommand, CuentaDto?>
{
    private readonly ICuentaRepository _cuentaRepository;

    public ActualizarCuentaHandler(ICuentaRepository cuentaRepository)
    {
        _cuentaRepository = cuentaRepository;
    }

    public async Task<CuentaDto?> Handle(
        ActualizarCuentaCommand request,
        CancellationToken cancellationToken)
    {
        var cuenta = await _cuentaRepository.ObtenerPorIdAsync(
            request.Id,
            cancellationToken);

        if (cuenta is null)
        {
            return null;
        }

        cuenta.Actualizar(
            request.Titular,
            request.Saldo,
            request.Estado);

        await _cuentaRepository.GuardarCambiosAsync(
            cancellationToken);

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