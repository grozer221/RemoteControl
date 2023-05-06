using RemoteControl.Business.Common;

namespace RemoteControl.Business.Models
{
    public class User : BaseModel
    {
        public string Email { get; set; }

        public string Salt { get; set; }

        public string Password { get; set; }
    }
}