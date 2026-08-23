using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Cuentas.Comandos.EliminarCuenta;

public class EliminarCuentaValidator
    : AbstractValidator<EliminarCuentaCommand>
{
    public EliminarCuentaValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("El identificador de la cuenta es obligatorio.");
    }
}