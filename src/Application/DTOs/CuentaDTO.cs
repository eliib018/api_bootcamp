using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs;

public class CuentaDto
{
    public Guid Id { get; set; }

    public string NumeroCuenta { get; set; } = string.Empty;

    public string Titular { get; set; } = string.Empty;

    public decimal Saldo { get; set; }

    public EstadoCuenta Estado { get; set; }

    public DateTime FechaCreacionUtc { get; set; }
}