using GraphQL.Types;
using RemoteControl.Business.Models;
using RemoteControl.Repositories;

namespace RemoteControl.WebApp.GraphApi.Modules.Users
{
    public class UsersQueries : ObjectGraphType
    {
        public UsersQueries(UsersRepository userRepository)
        {
            Field<UserType, User>()
               .Name("GetById")
               .Resolve(_ => new User());
        }
    }
}
