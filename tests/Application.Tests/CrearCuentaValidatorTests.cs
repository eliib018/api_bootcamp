using System;
using System.Collections.Generic;
using System.Text;
using Application.Cuentas.Comandos.CrearCuenta;
using Domain.Enums;

namespace Application.Tests;

public class CrearCuentaValidatorTests
{
    private readonly CrearCuentaValidator _validator = new();

    [Fact]
    public void DatosValidos()
    {
        var command = new CrearCuentaCommand(
            "001-000001",
            "Maria Lopez",
            150000,
            EstadoCuenta.Activa);

        var resultado = _validator.Validate(command);

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void SaldoNegativo()
    {
        var command = new CrearCuentaCommand(
            "001-000002",
            "Maria Lopez",
            -1,
            EstadoCuenta.Activa);

        var resultado = _validator.Validate(command);

        Assert.False(resultado.IsValid);

        Assert.Contains(
            resultado.Errors,
            error => error.PropertyName ==
                nameof(CrearCuentaCommand.Saldo));
    }

    [Fact]
    public void NumeroCuentaVacio()
    {
        var command = new CrearCuentaCommand(
            string.Empty,
            "Maria Lopez",
            1000,
            EstadoCuenta.Activa);

        var resultado = _validator.Validate(command);

        Assert.False(resultado.IsValid);

        Assert.Contains(
            resultado.Errors,
            error => error.PropertyName ==
                nameof(CrearCuentaCommand.NumeroCuenta));
    }

    [Fact]
    public void TitularVacio()
    {
        var command = new CrearCuentaCommand(
            "001-000003",
            string.Empty,
            1000,
            EstadoCuenta.Activa);

        var resultado = _validator.Validate(command);

        Assert.False(resultado.IsValid);

        Assert.Contains(
            resultado.Errors,
            error => error.PropertyName ==
                nameof(CrearCuentaCommand.Titular));
    }
}