using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Cuentas.Comandos.EliminarCuenta;

public record EliminarCuentaCommand(Guid Id)
    : IRequest<bool>;