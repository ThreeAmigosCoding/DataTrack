namespace DataTrack.Dto;

public class MyToken
{
    public string Token { get; set; }

    public MyToken()
    {
    }

    public MyToken(string token)
    {
        Token = token;
    }
}