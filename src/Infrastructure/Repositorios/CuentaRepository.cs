using Application.Abstracciones;
using Domain.Entidades;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositorios;

public class CuentaRepository : ICuentaRepository
{
    private readonly AppDbContext _context;

    public CuentaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Cuenta>> ObtenerTodasAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Cuentas
            .AsNoTracking()
            .OrderBy(x => x.FechaCreacionUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<Cuenta?> ObtenerPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Cuentas
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<bool> ExisteNumeroCuentaAsync(
        string numeroCuenta,
        CancellationToken cancellationToken = default)
    {
        return await _context.Cuentas
            .AnyAsync(
                x => x.NumeroCuenta == numeroCuenta,
                cancellationToken);
    }

    public async Task AgregarAsync(
        Cuenta cuenta,
        CancellationToken cancellationToken = default)
    {
        await _context.Cuentas.AddAsync(
            cuenta,
            cancellationToken);
    }

    public Task EliminarAsync(
        Cuenta cuenta,
        CancellationToken cancellationToken = default)
    {
        _context.Cuentas.Remove(cuenta);

        return Task.CompletedTask;
    }

    public async Task GuardarCambiosAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(
            cancellationToken);
    }
}