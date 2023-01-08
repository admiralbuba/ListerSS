namespace ListerSS.Dto
{
    public class MessageDto
    {
        public string ToName { get; set; }
        public string Text { get; set; }

        public MessageDto(string toName, string text)
        {
            ToName = toName;
            Text = text;
        }
    }
}
