namespace Kaneko.IdentityCenter.Models
{
    public class UserData
    {
        public string UserId { get; internal set; }
        public string UserName { get; internal set; }
        public object RealName { get; internal set; }
        public string Email { get; internal set; }
        public object Photo { get; internal set; }
        public string ClientId { get; internal set; }
    }
}
