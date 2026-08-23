using System;
using System.Collections.Generic;
using System.Text;

using Application.Abstracciones;
using Application.DTOs;
using Domain.Entidades;
using MediatR;

namespace Application.Cuentas.Comandos.CrearCuenta;

public class CrearCuentaHandler
    : IRequestHandler<CrearCuentaCommand, CuentaDto>
{
    private readonly ICuentaRepository _cuentaRepository;

    public CrearCuentaHandler(ICuentaRepository cuentaRepository)
    {
        _cuentaRepository = cuentaRepository;
    }

    public async Task<CuentaDto> Handle(
        CrearCuentaCommand request,
        CancellationToken cancellationToken)
    {

        var existeCuenta = await _cuentaRepository.ExisteNumeroCuentaAsync(
            request.NumeroCuenta,
            cancellationToken);

        if (existeCuenta)
        {
            throw new InvalidOperationException(
                "Ya existe una cuenta con ese número.");
        }

        var cuenta = new Cuenta(
            request.NumeroCuenta,
            request.Titular,
            request.Saldo,
            request.Estado);

        await _cuentaRepository.AgregarAsync(
            cuenta,
            cancellationToken);

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