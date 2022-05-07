using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Common.Settings;

public class TransferSettings
{
    public string TGBotToken { get; set; }

    public string TGBotHost { get; set; }

    public int TablePageSize { get; set; } = 50;

    public string FileStoragePath { get; set; } = "/Files";
}
