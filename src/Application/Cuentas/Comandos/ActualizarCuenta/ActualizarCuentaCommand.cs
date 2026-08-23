using Application.DTOs;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Cuentas.Comandos.ActualizarCuenta;

public record ActualizarCuentaCommand(
    Guid Id,
    string Titular,
    decimal Saldo,
    EstadoCuenta Estado
) : IRequest<CuentaDto?>;