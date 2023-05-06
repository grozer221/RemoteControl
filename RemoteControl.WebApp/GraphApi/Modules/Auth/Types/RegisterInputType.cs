using GraphQL.Types;
using System.ComponentModel.DataAnnotations;

namespace RemoteControl.WebApp.GraphApi.Modules.Auth.Types
{
    public class RegisterInputType : InputObjectGraphType<RegisterInput>
    {
        public RegisterInputType()
            : base()
        {
            Field<StringGraphType, string>()
              .Name("Email")
              .Resolve(context => context.Source.Email);

            Field<NonNullGraphType<StringGraphType>, string>()
               .Name("Password")
               .Resolve(context => context.Source.Password);
        }
    }

    public class RegisterInput
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
