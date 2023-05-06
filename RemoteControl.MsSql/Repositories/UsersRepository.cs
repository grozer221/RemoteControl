using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RemoteControl.Business.Models;
using RemoteControl.MsSql;
using RemoteControl.MsSql.Common;
using RemoteControl.WebApp.Attributes;
using System.Linq.Expressions;

namespace RemoteControl.Repositories
{
    [InjectableService(ServiceLifetime.Transient)]
    public class UsersRepository : BaseRepository<User>
    {

        public UsersRepository(AppDbContext appDbContext)
            : base(appDbContext)
        {
        }

        public Task<User?> GetByEmailAsync(string email, params Expression<Func<User, object>>[] includes)
        {
            return includes.Aggregate(context.Users.AsQueryable(),
                (current, include) => current.Include(include))
                    .FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}
