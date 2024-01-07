namespace Transfer.Common.Security;

public interface ITokenService : ITokenValidator
{
    string BuildToken(Guid userId);
}
