using Application.Cuentas.Comandos.CrearCuenta;
using Application.DTOs;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Cuentas.Comandos.CrearCuenta;

public record CrearCuentaCommand(
    string NumeroCuenta,
    string Titular,
    decimal Saldo,
    EstadoCuenta Estado
) : IRequest<CuentaDto>;