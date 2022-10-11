using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Common;

public interface IMailModule
{
    Task SendEmailPlainTextAsync(string body, string subject, string recipient, bool isHtml = false);
}
