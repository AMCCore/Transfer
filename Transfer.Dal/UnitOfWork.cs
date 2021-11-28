using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common;
using Transfer.Dal.Context;

namespace Transfer.Dal
{
    public class  UnitOfWork : BaseUnitOfWork<TransferContext>
    {
        private readonly string _connectionString;

        /// <summary>
        ///     инициализация UnitOfWork
        /// </summary>
        public UnitOfWork(TransferContext context, IConfiguration settings) : base(context)
        {
            _connectionString = settings.GetConnectionString("TransferDb");
        }

        public UnitOfWork(string connString)
        {
            _connectionString = connString;
            var options = new DbContextOptionsBuilder<TransferContext>()
                .UseLazyLoadingProxies()
                .UseSqlServer(connString);
            _context = new TransferContext(options.Options);
            _connectionStringInited = true;
        }
    }
}
