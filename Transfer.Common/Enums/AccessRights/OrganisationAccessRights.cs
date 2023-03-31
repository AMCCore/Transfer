using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common.Attributes;

namespace Transfer.Common.Enums.AccessRights;

public enum OrganisationAccessRights
{
    /// <summary>
    /// Полное всеобъемлющее право на любые действия с организациями
    /// </summary>
    [Description("Полное всеобъемлющее право на любые действия с организациями")]
    [EnumGuid("9fcd2576-15e1-4482-aa50-575aa32246cb")]
    Admin,

    /// <summary>
    /// Просмотр пользователей (списка)
    /// </summary>
    [Description("Просмотр пользователей (списка)")]
    [EnumGuid("26a34874-cc9a-41b2-b31d-b918f833f16f")]
    ViewUserList,

    /// <summary>
    /// Редактирование пользователей
    /// </summary>
    [Description("Редактирование пользователей")]
    [EnumGuid("743b1d0d-e49e-4a02-8cba-8467057dc359")]
    EditUser,

}
