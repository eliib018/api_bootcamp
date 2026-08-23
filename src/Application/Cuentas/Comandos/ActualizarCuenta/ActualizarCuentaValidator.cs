using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Application.Cuentas.Comandos.ActualizarCuenta;

public class ActualizarCuentaValidator
    : AbstractValidator<ActualizarCuentaCommand>
{
    public ActualizarCuentaValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("El identificador de la cuenta es obligatorio.");

        RuleFor(x => x.Titular)
            .NotEmpty()
            .WithMessage("El titular es obligatorio.")
            .MaximumLength(100)
            .WithMessage("El titular no puede superar los 100 caracteres.");

        RuleFor(x => x.Saldo)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El saldo no puede ser negativo.");

        RuleFor(x => x.Estado)
            .IsInEnum()
            .WithMessage("El estado de la cuenta no es válido.");
    }
}