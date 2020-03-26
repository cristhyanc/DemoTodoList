using DemoTodoList.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DemoTodoList.Shared.ORM;
using DemoTodoList.Entities;

namespace DemoTodoList.UserCases.TodoItemUserCases
{
  public  class TodoItemUseCase: ITodoItemUseCase
    {
        IUnitOfWork uow;
        public TodoItemUseCase(IUnitOfWork uow)
        {
            this.uow = uow;
        }      

        public async Task<TodoItem> CreateTodoItem(string title, string description, Guid todoListId)
        {
            var list = await uow.TodoListRepository.Get(todoListId);
            if (list == null)
            {
                throw new ArgumentException("The todo list does not exist");
            }

            var newItem = new TodoItemEntity();
            newItem.Title = title;
            newItem.Description = description;
            newItem.ListId = todoListId;

            if (await newItem.Save(uow.TodoItemRepository).ConfigureAwait(false))
            {
                return newItem.ConverToDto();
            }

            return null;
        }

        public async Task<bool> MarkTodoAsCompleted(Guid todoItemId)
        {
            var item = await uow.TodoItemRepository.Get(todoItemId);
            if (item == null)
            {
                throw new ArgumentException("The todo item does not exist");
            }

            item.IsCompleted = true;
            return await item.Save(uow.TodoItemRepository).ConfigureAwait(false);
        }

        public async Task<bool> DeleteTodoItem(Guid todoItemId)
        {
            var item = await uow.TodoItemRepository.Get(todoItemId);
            if (item == null)
            {
                throw new ArgumentException("The todo item does not exist");
            }
                      
            return await uow.TodoItemRepository.Delete(item).ConfigureAwait(false);
        }

    }
}
