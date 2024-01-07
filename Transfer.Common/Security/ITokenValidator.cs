namespace Transfer.Common.Security;

public interface ITokenValidator
{
    bool IsTokenValid(string token);
}
