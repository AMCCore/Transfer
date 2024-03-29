﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Common.Security;

public interface IAdvancedSecurityService : ISecurityService
{
    /// <summary>
    /// Признак административного доступа
    /// </summary>
    bool IsAdmin { get; }

    /// <summary>
    /// Идентификатор организации текущего аккаунта
    /// </summary>
    Guid? CurrentAccountOrganisationId { get; }

    /// <summary>
    /// Проверка наличия права (для люблой из организаций)
    /// </summary>
    bool HasRightForSomeOrganisation(Enum right, Guid? organisation = null);

    /// <summary>
    /// Проверка наличия права (для люблой из организаций)
    /// </summary>
    bool HasRightForSomeOrganisation(Guid right, Guid? organisation = null);

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
    //public Guid[] GetAvailableOrgs();

    /// <summary>
    /// Возвращает список прав текущего пользователя
    /// </summary>
    IDictionary<Guid, IList<Guid>> Rights { get; }
}
