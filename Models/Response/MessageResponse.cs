namespace Lister.WebApi.Models.Response
{
    public class MessageResponse
    {
        public string ToName { get; set; }
        public string Text { get; set; }

        public MessageResponse(string toName, string text)
        {
            ToName = toName;
            Text = text;
        }
    }
}