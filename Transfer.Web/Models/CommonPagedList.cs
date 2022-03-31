using System;
using System.Collections.Generic;
using X.PagedList;

namespace Transfer.Web.Models;

public class CommonPagedList<T> : BasePagedList<T>
{
    public CommonPagedList() : base(1, 10, 0)
    {
    }

    public CommonPagedList(IEnumerable<T> pageItems, int pageNumber, int pageSize, int totalItemCount)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "Страница не может быть меньше чем 1.");
        }

        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Размер страницы не может быть меньше чем 1.");
        }

        TotalItemCount = totalItemCount;
        PageSize = pageSize;
        PageNumber = pageNumber;
        PageCount = TotalItemCount > 0 ? (int)Math.Ceiling(TotalItemCount / (double)PageSize) : 0;
        HasPreviousPage = PageNumber > 1;
        HasNextPage = PageNumber < PageCount;
        IsFirstPage = PageNumber == 1;
        IsLastPage = PageNumber >= PageCount;
        FirstItemOnPage = (PageNumber - 1) * PageSize + 1;
        var num = FirstItemOnPage + PageSize - 1;
        LastItemOnPage = num > TotalItemCount ? TotalItemCount : num;
        if (pageItems != null)
        {
            Subset.AddRange(pageItems);
        }
    }
}