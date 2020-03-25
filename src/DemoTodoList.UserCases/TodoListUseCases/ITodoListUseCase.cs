using DemoTodoList.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoTodoList.UserCases.TodoListUseCases
{
    public interface ITodoListUseCase
    {
        Task<IEnumerable<TodoList>> GetTodoLists();
        Task<TodoList> CreateTodoList(string title);
        Task<bool> SetNewMainTodoList(Guid todolistId);
        Task<IEnumerable<TodoItem>> GetTodoItems(Guid todolistId);
        Task<bool> DeleteTodoList(Guid todolistId);
    }
}
