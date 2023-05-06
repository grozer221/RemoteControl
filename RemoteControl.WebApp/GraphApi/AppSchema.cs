using GraphQL.Types;

namespace RemoteControl.WebApp.GraphApi
{
    public class AppSchema : Schema
    {
        public AppSchema(IServiceProvider provider)
            : base(provider)
        {
            Query = provider.GetRequiredService<RootQueries>();
            Mutation = provider.GetRequiredService<RootMutations>();
        }
    }
}
