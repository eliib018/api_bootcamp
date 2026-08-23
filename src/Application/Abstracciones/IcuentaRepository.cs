using Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstracciones;

public interface ICuentaRepository
{
    Task<IReadOnlyCollection<Cuenta>> ObtenerTodasAsync(
        CancellationToken cancellationToken = default);

    Task<Cuenta?> ObtenerPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> ExisteNumeroCuentaAsync(
        string numeroCuenta,
        CancellationToken cancellationToken = default);

    Task AgregarAsync(
        Cuenta cuenta,
        CancellationToken cancellationToken = default);

    Task EliminarAsync(
        Cuenta cuenta,
        CancellationToken cancellationToken = default);

    Task GuardarCambiosAsync(
        CancellationToken cancellationToken = default);
}