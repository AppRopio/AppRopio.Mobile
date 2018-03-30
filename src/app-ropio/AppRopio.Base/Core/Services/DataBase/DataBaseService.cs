using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AppRopio.Base.Core.Messsages.DataBase;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Realms;

namespace AppRopio.Base.Core.Services.DataBase
{
    public abstract class DataBaseService<TDbo, TModel, ID> : IDataBaseService<TModel, ID>
        where TDbo : RealmObject, new()
        where TModel : class
        where ID : IComparable, IComparable<ID>, IEquatable<ID>
    {
        protected readonly ID BAD_ID = default(ID);

        protected virtual RealmConfiguration Configuration
        {
            get
            {
                return new RealmConfiguration(DBName)
                {
                    ObjectClasses = new[] { typeof(TDbo) },
                    SchemaVersion = 1,
                    MigrationCallback = HandleMigrationCallbackDelegate
                };
            }
        }

        protected Realm Instance { get { return Realm.GetInstance(Configuration); } }

        public virtual string DBName { get { return "database.realm"; } }

        protected DataBaseService()
        {
            if (AppSettings.ApiKey.IsNullOrEmtpy())
                throw new ArgumentNullException(nameof(AppSettings.ApiKey));
        }

        protected virtual void HandleMigrationCallbackDelegate(Migration migration, ulong oldSchemaVersion)
        {

        }

        protected Expression<Func<TDbo, bool>> IdentifyModelByIdInArray(IEnumerable<ID> enumerable)
        {
            return dbo => enumerable.Contains(GetId(dbo));
        }

        protected Func<TDbo, bool> EqualDboToModel(TModel model)
        {
            return (arg) => GetId(arg).Equals(GetId(model));
        }

        #region Abstract

        protected abstract TDbo ConvertToDBO(TDbo dbo, TModel model);
        protected abstract TModel ConvertFromDBO(TDbo dbo);

        protected abstract ID GetId(TModel model);
        protected abstract ID GetId(TDbo dbo);

        #endregion

        #region IDataBaseService implementation

        public int Count()
        {
            return Instance.All<TDbo>().Count();
        }

        #region IsertOrUpdate

        public void InsertOrUpdate(TModel model)
        {
            //return Task.Run(() =>
            //{
            TDbo existsDbo = null;

            lock (this)
            {
                var allDbo = Instance.All<TDbo>();
                existsDbo = allDbo != null && allDbo.Any() ? allDbo.FirstOrDefault(EqualDboToModel(model)) : null;

                using (var transaction = Instance.BeginWrite())
                {
                    Instance.Add(ConvertToDBO(existsDbo, model), existsDbo != null);

                    transaction.Commit();
                }
            }

            if (existsDbo != null)
                Mvx.Resolve<IMvxMessenger>().Publish(new DataBaseUpdatedMessage<TModel>(this, new List<TModel> { model }));
            else
                Mvx.Resolve<IMvxMessenger>().Publish(new DataBaseAddedMessage<TModel>(this, new List<TModel> { model }));
            //});
        }

        public void InsertOrUpdate(IEnumerable<TModel> models)
        {
            //return Task.Run(() =>
            //{
            var update = true;

            lock (this)
            {
                using (var transaction = Instance.BeginWrite())
                {
                    foreach (var model in models)
                    {
                        var allDbo = Instance.All<TDbo>();
                        var existsDbo = allDbo != null && allDbo.Any() ? allDbo.FirstOrDefault(EqualDboToModel(model)) : null;

                        Instance.Add(ConvertToDBO(existsDbo, model), existsDbo != null);

                        if (update && existsDbo == null)
                            update = false;


                    }
                    transaction.Commit();
                }
            }

            if (update)
                Mvx.Resolve<IMvxMessenger>().Publish(new DataBaseUpdatedMessage<TModel>(this, models.ToList()));
            else
                Mvx.Resolve<IMvxMessenger>().Publish(new DataBaseAddedMessage<TModel>(this, models.ToList()));
            //});
        }

        #endregion

        #region Load

        public TModel LoadById(ID id)
        {
            //return Task.Run(() =>
            //{
            TDbo existsDbo = null;

            lock (this)
            {
                existsDbo = Instance.All<TDbo>().FirstOrDefault(dbo => GetId(dbo).Equals(id));
            }

            return ConvertFromDBO(existsDbo);
            //});
        }

        public IEnumerable<TModel> LoadAll()
        {
            //return Task.Run(() =>
            //{
            IEnumerable<TModel> models;

            lock (this)
            {
                models = Instance.All<TDbo>().Select(ConvertFromDBO);
            }

            return models;
            //});
        }

        public IEnumerable<TModel> LoadAll(int skip, int count = 10)
        {
            //return Task.Run(() =>
            //{
            if (skip <= 0)
                skip = 0;

            if (count <= 0)
                count = 10;

            IEnumerable<TModel> models;

            lock (this)
            {
                models = Instance.All<TDbo>().Skip(skip).Take(count).Select(ConvertFromDBO);
            }

            return models;
            //});
        }

        public IEnumerable<TModel> LoadAllMatches(Expression<Func<TModel, bool>> predicate)
        {
            //return Task.Run(() =>
            //{
            IEnumerable<TModel> models;

            lock (this)
            {
                models = Instance.All<TDbo>().Select(ConvertFromDBO).AsQueryable().Where(predicate);
            }

            return models;
            //});
        }

        public IEnumerable<TModel> LoadByIds(IEnumerable<ID> ids)
        {
            //return Task.Run(() =>
            //{
            IQueryable<TDbo> results;
            lock (this)
            {
                results = Instance.All<TDbo>();
            }
            return results.Where(IdentifyModelByIdInArray(ids)).Select(ConvertFromDBO);
            //});
        }

        public IEnumerable<TModel> LoadAllExceptIds(IEnumerable<ID> ids, int skip = 0, int count = 10)
        {
            //return Task.Run(() =>
            //{
            if (skip <= 0)
                skip = 0;

            if (count <= 0)
                count = 10;

            var negativeExpr = IdentifyModelByIdInArray(ids).Not();

            IEnumerable<TModel> models;

            lock (this)
            {
                models = Instance.All<TDbo>()
                                 .Where(negativeExpr)
                                 .Skip(skip)
                                 .Take(count)
                                 .Select(x => ConvertFromDBO(x));
            }

            return models;
            //});
        }

        #endregion

        #region Delete

        public void Delete(ID id)
        {
            //return Task.Run(() =>
            //{
            if ((id == null && BAD_ID == null) || id.Equals(BAD_ID))
                return;

            TDbo deletedItem = null;

            lock (this)
            {
                deletedItem = Instance.All<TDbo>().FirstOrDefault(d => GetId(d).Equals(id));

                using (var transaction = Instance.BeginWrite())
                {
                    Instance.Remove(deletedItem);
                    transaction.Commit();
                }
            }

            Mvx.Resolve<IMvxMessenger>().Publish(new DataBaseRemovedMessage<TModel>(this, new List<TModel> { ConvertFromDBO(deletedItem) }));
            //});
        }

        public void Delete(TModel model)
        {
            Delete(GetId(model));
        }

        public void DeleteAll()
        {
            //return Task.Run(() =>
            //{
            var deletedItems = new List<TModel>();

            lock (this)
            {
                deletedItems = Instance.All<TDbo>().Select(ConvertFromDBO).ToList();
                using (var transaction = Instance.BeginWrite())
                {
                    Instance.RemoveAll();
                    transaction.Commit();
                }
            }

            Mvx.Resolve<IMvxMessenger>().Publish(new DataBaseRemovedMessage<TModel>(this, deletedItems));
            //});
        }

        public void DeleteAllMatches(Expression<Func<TModel, bool>> predicate)
        {
            //return Task.Run(() =>
            //{
            var deletedItems = new List<TModel>();

            lock (this)
            {
                var data = Instance.All<TDbo>().Where(dbo => predicate.Compile().Invoke(ConvertFromDBO(dbo))).ToList();
                using (var transaction = Instance.BeginWrite())
                {
                    foreach (var item in data)
                    {
                        deletedItems.Add(ConvertFromDBO(item));
                        Instance.Remove(item);
                    }

                    transaction.Commit();
                }
            }

            Mvx.Resolve<IMvxMessenger>().Publish(new DataBaseRemovedMessage<TModel>(this, deletedItems));
            //});
        }

        #endregion

        #endregion
    }
}

