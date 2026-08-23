using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Cuentas.Consultas.ObtenerCuentaPorId;

public record ObtenerCuentaPorIdQuery(Guid Id)
    : IRequest<CuentaDto?>;
