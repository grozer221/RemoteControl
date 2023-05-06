using GraphQL.Types;
using RemoteControl.WebApp.GraphApi.Modules.Auth;

namespace RemoteControl.WebApp.GraphApi
{
    public class RootMutations : ObjectGraphType
    {
        public RootMutations()
        {
            Field<AuthMutations>()
                .Name("Auth")
                .Resolve(_ => new { });
        }
    }
}
