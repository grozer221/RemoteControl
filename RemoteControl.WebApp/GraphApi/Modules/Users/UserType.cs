using GraphQL.Types;
using RemoteControl.Business.Models;
using RemoteControl.WebApp.GraphApi.Common;

namespace RemoteControl.WebApp.GraphApi.Modules.Users
{
    public class UserType : BaseModelType<User>
    {
        public UserType()
            : base()
        {
            Field<NonNullGraphType<StringGraphType>, string>()
                .Name("Email")
                .Resolve(context => context.Source.Email);
        }
    }
}
