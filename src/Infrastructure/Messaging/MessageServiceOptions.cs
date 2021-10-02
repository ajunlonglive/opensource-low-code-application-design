namespace Infrastructure.Messaging
{
    public class MessageServiceOptions
    {
        public string HostAddress { get; set; }
        public int  PortNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}