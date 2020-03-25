using DemoTodoList.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoTodoList.UserCases.TodoItemUserCases
{
 public  interface ITodoItemUseCase
    {
        Task<TodoItem> CreateTodoItem(string title, string description, Guid todoListId);
        Task<bool> MarkTodoAsCompleted(Guid todoItemId);
        Task<bool> DeleteTodoItem(Guid todoItemId);
    }
}
