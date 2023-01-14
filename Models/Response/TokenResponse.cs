namespace Lister.WebApi.Models.Response
{
    public class TokenResponse
    { 
        public string Bearer { get; set; } 

        public TokenResponse(string bearer)
        {
            Bearer = bearer;
        }
    }
}
