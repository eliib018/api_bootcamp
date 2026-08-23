using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Cuentas.Consultas.ObtenerCuentas;

public record ObtenerCuentasQuery
    : IRequest<IReadOnlyCollection<CuentaDto>>;