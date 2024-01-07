namespace Transfer.Common.Security;

public interface ISecurityService
{
    /// <summary>
    /// Идентификатор текущего аккаунта
    /// </summary>
    Guid CurrentAccountId { get; }

    /// <summary>
    /// Признак авторизованности текущего аккаунта
    /// </summary>
    bool IsAuthenticated { get; }
}
