namespace Lister.WebApi.SignalR
{
    public class Message
    {
        public string ToName { get; set; }
        public string Text { get; set; }

        public Message(string toName, string text)
        {
            ToName = toName;
            Text = text;
        }
    }
}