using Application.Abstracciones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Cuentas.Comandos.EliminarCuenta;

public class EliminarCuentaHandler
    : IRequestHandler<EliminarCuentaCommand, bool>
{
    private readonly ICuentaRepository _cuentaRepository;

    public EliminarCuentaHandler(ICuentaRepository cuentaRepository)
    {
        _cuentaRepository = cuentaRepository;
    }

    public async Task<bool> Handle(
        EliminarCuentaCommand request,
        CancellationToken cancellationToken)
    {
        var cuenta = await _cuentaRepository.ObtenerPorIdAsync(
            request.Id,
            cancellationToken);

        if (cuenta is null)
        {
            return false;
        }

        await _cuentaRepository.EliminarAsync(
            cuenta,
            cancellationToken);

        await _cuentaRepository.GuardarCambiosAsync(
            cancellationToken);

        return true;
    }
}