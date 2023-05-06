using GraphQL.Types;
using RemoteControl.WebApp.GraphApi.Modules.Auth;

namespace RemoteControl.WebApp.GraphApi
{
    public class RootQueries : ObjectGraphType
    {
        public RootQueries()
        {
            Field<AuthQueries>()
                .Name("Auth")
                .Resolve(_ => new { });
        }
    }
}
