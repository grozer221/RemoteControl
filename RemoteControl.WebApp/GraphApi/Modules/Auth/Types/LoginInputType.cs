using GraphQL.Types;

namespace RemoteControl.WebApp.GraphApi.Modules.Auth.Types
{
    public class LoginInputType : InputObjectGraphType<LoginInput>
    {
        public LoginInputType()
        {
            Field<NonNullGraphType<StringGraphType>>()
                .Name("Email")
                .Resolve(context => context.Source.Email);

            Field<NonNullGraphType<StringGraphType>>()
                .Name("Password")
                .Resolve(context => context.Source.Password);
        }
    }

    public class LoginInput
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
