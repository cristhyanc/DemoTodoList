using DemoTodoList.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTodoList.Repository
{
 public   class DBContext : IDisposable
    {
        static readonly Lazy<SQLiteAsyncConnection> _databaseConnectionHolder = new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection(DemoTodoList.Shared.Helper.DBFilePath , SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache));
        public readonly static  SQLiteAsyncConnection Database = _databaseConnectionHolder.Value;
        static  bool initialized = false;

        public DBContext()
        {
            InitializeAsync();
        }

         void InitializeAsync()
        {
            try
            {
                if (!initialized)
                {
                    if (!Database.TableMappings.Any(x => x.MappedType == typeof(TodoListEntity)))
                    {
                        Database.CreateTableAsync<TodoListEntity>(CreateFlags.None).Wait();
                    }

                    if (!Database.TableMappings.Any(x => x.MappedType == typeof(TodoItemEntity)))
                    {
                         Database.CreateTableAsync<TodoItemEntity>(CreateFlags.None).Wait();
                    }

                    initialized = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }             
        }
        
        public void Dispose()
        {
             Database.CloseAsync().ConfigureAwait(false);            
        }
    }

    public static class TaskExtensions
    {
        // NOTE: Async void is intentional here. This provides a way
        // to call an async method from the constructor while
        // communicating intent to fire and forget, and allow
        // handling of exceptions
        public static async void SafeFireAndForget(this Task task,
            bool returnToCallingContext,
            Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }

            // if the provided action is not null, catch and
            // pass the thrown exception
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }
    }
}
