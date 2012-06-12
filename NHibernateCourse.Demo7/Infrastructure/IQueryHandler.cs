using System.Collections.Generic;

namespace NHibernateCourse.Demo7.Infrastructure
{
    public interface IQueryHandler
    {
    }

    public interface IQueryUnique<out TResult> : IQueryHandler
    {
        TResult UniqueResult();
    }

    public interface IQueryUnique<out TResult, in TParameter> : IQueryHandler
    {
        TResult UniqueResult(TParameter parameter);
    }

    public interface IQueryList<out TResult> : IQueryHandler
    {
        IEnumerable<TResult> List();
    }

    public interface IQueryList<out TResult, in TParameter> : IQueryHandler
    {
        IEnumerable<TResult> List(TParameter parameter);
    }
}