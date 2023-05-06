using GraphQL.Types;
using RemoteControl.Business.Common;

namespace RemoteControl.WebApp.GraphApi.Common
{
    public abstract class BaseModelType<T> : ObjectGraphType<T>
        where T : BaseModel
    {
        public BaseModelType()
        {
            Field<NonNullGraphType<IdGraphType>, Guid>()
               .Name("Id")
               .Resolve(context => context.Source.Id);

            Field<NonNullGraphType<DateTimeGraphType>, DateTime>()
                .Name("CreatedAt")
                .Resolve(context => context.Source.CreatedAt);

            Field<NonNullGraphType<DateTimeGraphType>, DateTime>()
               .Name("UpdatedAt")
               .Resolve(context => context.Source.UpdatedAt);
        }
    }
}
