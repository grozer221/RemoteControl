using RemoteControl.Business.Common;

namespace RemoteControl.Business.Models;
public class Session : BaseModel
{
    public string Token { get; set; }

    public bool IsOnline { get; set; }

    public DateTime LastTimeOnline { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }
}
