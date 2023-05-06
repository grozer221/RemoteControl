using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RemoteControl.Business.Models;
using RemoteControl.MsSql;
using RemoteControl.MsSql.Common;
using RemoteControl.WebApp.Attributes;

namespace RemoteControl.Repositories;

[InjectableService(ServiceLifetime.Transient)]
public class SessionRepository : BaseRepository<Session>
{
    private readonly AppDbContext context;

    public SessionRepository(AppDbContext context)
        : base(context)
    {
        this.context = context;
    }

    public async Task MakeAllOfflineAsync()
    {
        var sessions = await context.Sessions
            .Where(s => s.IsOnline == true)
            .ToListAsync();
        foreach (var session in sessions)
        {
            session.IsOnline = false;
            await UpdateAsync(session);
        }
    }

    public Task<Session?> GetByTokenAsync(string token)
    {
        return context.Sessions.SingleOrDefaultAsync(s => s.Token == token);
    }

    public Task<Session?> GetLastActiveAsync(Guid userId)
    {
        return context.Sessions
            .OrderByDescending(s => s.LastTimeOnline)
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }

    public async Task RemoveAllForUserAsync(Guid userId)
    {
        var tokens = await GetAsync(t => t.UserId == userId);
        foreach (var token in tokens)
            context.Sessions.Remove(token);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid userId, string token)
    {
        var tokens = await GetAsync(t => t.UserId == userId && t.Token == token);
        foreach (var t in tokens)
            context.Sessions.Remove(t);
        await context.SaveChangesAsync();
    }
}
