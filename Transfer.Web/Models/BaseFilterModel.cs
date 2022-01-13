using System.Collections.Generic;

namespace Transfer.Web.Models
{
    /// <summary>
    ///     Базовая модель для фильтруемых списков
    /// </summary>
    public abstract class BaseFilterModel<T>
    {
        protected BaseFilterModel() : this(new List<T>()) { }
        
        protected BaseFilterModel(IEnumerable<T> input, int pageSize = 10)
        {
            Results = new CommonPagedList<T>(input, 1, pageSize, 0);
            PageNumber = 1;
            PageSize = pageSize;
        }
        
        /// <summary>
        ///     Результаты
        /// </summary>
        public CommonPagedList<T> Results { get; set; }

        /// <summary>
        ///     Номер страницы
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        ///     Размер страницы
        /// </summary>
        public int PageSize { get; set; }

        internal int StartRecord => (PageNumber - 1) * PageSize;

        /// <summary>
        ///     Общее кол-во записей
        /// </summary>
        public int TotalCount => Results?.Count ?? 0;
    }
}