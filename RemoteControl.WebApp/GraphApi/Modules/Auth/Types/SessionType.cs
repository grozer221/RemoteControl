using GraphQL.Types;
using RemoteControl.Business.Models;
using RemoteControl.WebApp.GraphApi.Common;

namespace RemoteControl.WebApp.GraphApi.Modules.Auth.Types
{
    public class SessionType : BaseModelType<Session>
    {
        public SessionType()
            : base()
        {
            Field<NonNullGraphType<BooleanGraphType>, bool>()
               .Name("IsOnline")
               .Resolve(context => context.Source.IsOnline);

            Field<NonNullGraphType<DateTimeGraphType>, DateTime>()
               .Name("LastTimeOnline")
               .Resolve(context => context.Source.LastTimeOnline);
        }
    }
}
