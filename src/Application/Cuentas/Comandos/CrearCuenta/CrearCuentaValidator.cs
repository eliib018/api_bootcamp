using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Cuentas.Comandos.CrearCuenta;

public class CrearCuentaValidator : AbstractValidator<CrearCuentaCommand>
{
    public CrearCuentaValidator()
    {
        RuleFor(x => x.NumeroCuenta)
            .NotEmpty()
            .WithMessage("El número de cuenta es obligatorio.")
            .MaximumLength(20)
            .WithMessage("El número de cuenta no puede superar los 20 caracteres.");

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