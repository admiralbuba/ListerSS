namespace Lister.WebApi.Models.Response
{
    public class Token
    { 
        public string Bearer { get; set; } 

        public Token(string bearer)
        {
            Bearer = bearer;
        }
    }
}
