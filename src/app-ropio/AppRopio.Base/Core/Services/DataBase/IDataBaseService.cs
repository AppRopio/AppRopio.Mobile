using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AppRopio.Base.Core.Services.DataBase
{
    public interface IDataBaseService
    {
        string DBName { get; }
    }

    public interface IDataBaseService<TModel, ID> : IDataBaseService
        where TModel : class
        where ID : IComparable, IComparable<ID>, IEquatable<ID>
    {
        int Count();

        void InsertOrUpdate(TModel model);

        void InsertOrUpdate(IEnumerable<TModel> model);

        TModel LoadById(ID id);

        IEnumerable<TModel> LoadAll();

        IEnumerable<TModel> LoadAll(int skip, int count = 10);

        IEnumerable<TModel> LoadAllMatches(Expression<Func<TModel, bool>> predicate);

        IEnumerable<TModel> LoadByIds(IEnumerable<ID> ids);

        IEnumerable<TModel> LoadAllExceptIds(IEnumerable<ID> ids, int skip = 0, int count = 10);

        void Delete(ID id);

        void Delete(TModel model);

        void DeleteAll();

        void DeleteAllMatches(Expression<Func<TModel, bool>> predicate);
    }
}

