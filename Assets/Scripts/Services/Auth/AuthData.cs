public class AuthData
{
    public AuthData(string id, string token)
    {
        Id = id;
        Token = token;
    }

    public string Token {  get; }
    public string Id { get; }
}