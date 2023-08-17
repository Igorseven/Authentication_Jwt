namespace Authentication.Domain.Entities;
public sealed class RefreshToken
{
    public int RefreshTokenId { get; set; }
    public required string UserName { get; set; }
    public required string Token { get; set; }
}
