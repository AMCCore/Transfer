namespace Transfer.Common.Security
{
    public interface ISecurityService
    {
        /// <summary>
        /// Признак авторизованности текущего аккаунта
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Идентификатор текущего аккаунта
        /// </summary>
        Guid CurrentAccountId { get; }

        /// <summary>
        /// Идентификатор организации текущего аккаунта
        /// </summary>
        Guid? CurrentAccountOrganisationId { get; }

        /// <summary>
        /// Проверка наличия права
        /// </summary>
        bool HasRightForSomeOrganisation(Enum right, Guid? organisation = null);

        /// <summary>
        /// Проверка наличия любого из прав
        /// </summary>
        bool HasAnyRightForSomeOrganisation(IEnumerable<Enum> rights, Guid? organisation = null);

        /// <summary>
        /// Список организаций для которых имеется право
        /// </summary>
        Guid[] HasOrganisationsForRight(Enum right);

        /// <summary>
        /// Список организаций для которых есть хотя бы одно любое право
        /// </summary>
        /// <returns></returns>
        public Guid[] GetAvailableOrgs();

        /// <summary>
        /// Возвращает список прав текущего пользователя
        /// </summary>
        IDictionary<Guid, IList<Guid>> GetRights();
    }
}
