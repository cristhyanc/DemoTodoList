using DemoTodoList.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoTodoList.UserCases
{
    public interface IUnitOfWork
    {
        IRepository<TodoListEntity> TodoListRepository { get; }
        IRepository<TodoItemEntity> TodoItemRepository { get; }
    }
}
