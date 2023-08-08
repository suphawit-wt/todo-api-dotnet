namespace TodoApp.Core.Models.Response
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }

        public TokenResponse(string accessToken)
        {
            AccessToken = accessToken;
        }

    }
}