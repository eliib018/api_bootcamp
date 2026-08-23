using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entidades;

public class Cuenta
{
    public Guid Id { get; private set; }

    public string NumeroCuenta { get; private set; } = string.Empty;

    public string Titular { get; private set; } = string.Empty;

    public decimal Saldo { get; private set; }

    public EstadoCuenta Estado { get; private set; }

    public DateTime FechaCreacionUtc { get; private set; }

    private Cuenta()
    {
    }

    public Cuenta(
        string numeroCuenta,
        string titular,
        decimal saldo,
        EstadoCuenta estado)
    {
        Id = Guid.NewGuid();
        NumeroCuenta = numeroCuenta;
        Titular = titular;
        Saldo = saldo;
        Estado = estado;
        FechaCreacionUtc = DateTime.UtcNow;
    }

    public void Actualizar(
        string titular,
        decimal saldo,
        EstadoCuenta estado)
    {
        Titular = titular;
        Saldo = saldo;
        Estado = estado;
    }
}