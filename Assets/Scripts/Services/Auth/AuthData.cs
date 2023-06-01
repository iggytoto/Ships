public class AuthData
{
    public AuthData(string id, string token)
    {
        Id = id;
        Token = token;
    }

    private string Token { get; }
    private string Id { get; }
}