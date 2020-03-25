using DemoTodoList.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using DemoTodoList.Shared.ORM;
using DemoTodoList.Entities;
using System.Linq;

namespace DemoTodoList.UserCases.TodoListUseCases
{
  public  class TodoListUseCase: ITodoListUseCase
    {
        IUnitOfWork uow;
        public TodoListUseCase(IUnitOfWork uow)
        {
            this.uow = uow;
        }
                    
        public async Task<IEnumerable<TodoList>> GetTodoLists()
        {
            IEnumerable<TodoList> result = new List<TodoList>();
            var todoLists  = await uow.TodoListRepository.GetAll().ConfigureAwait(false);
            if(todoLists?.Count()>0)
            {
                result = todoLists.Select(x => x.ConverToDto()); 
            }
            return result;
        }
        public async Task<TodoList> CreateTodoList(string title)
        {
            var newLsit = new TodoListEntity();
            newLsit.Title = title;
            
            var todoLists = await uow.TodoListRepository.GetAll().ConfigureAwait(false);
            if (!todoLists.Any())
            {
                newLsit.IsActive = true;
            }

            if(await newLsit.Save(uow.TodoListRepository).ConfigureAwait(false))
            {
                return newLsit.ConverToDto();
            }

            return null;
        }
        public async Task<bool> SetNewMainTodoList(Guid todolistId)
        {
            var mainTodoLists = await uow.TodoListRepository.GetAll(x => x.IsActive).ConfigureAwait(false);
            var newActiveList = await uow.TodoListRepository.Get(todolistId).ConfigureAwait(false);

            if (newActiveList == null)
            {
                throw new ArgumentException("Todo List could not be found");
            }

            if (mainTodoLists.Count() > 0)
            {
                mainTodoLists = mainTodoLists.Select((x) =>
                 {
                     x.IsActive = false;
                     return x;
                 });

                await uow.TodoListRepository.Update(mainTodoLists);
            }
            newActiveList.IsActive = true;
            return await newActiveList.Save(uow.TodoListRepository);
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItems(Guid todolistId)
        {
            IEnumerable<TodoItem> result = new List<TodoItem>();
            var todoItems = await uow.TodoItemRepository.GetAll(x=> x.ListId== todolistId).ConfigureAwait(false);
            if (todoItems?.Count() > 0)
            {
                result = todoItems.Select(x => x.ConverToDto());
            }
            return result;
        }

        public async Task<bool> DeleteTodoList(Guid todolistId)
        {

            var item = await uow.TodoListRepository.Get(todolistId).ConfigureAwait(false);

            if (item == null)
            {
                throw new ArgumentException("Todo List could not be found");
            }

            if (await item.Delete(uow.TodoListRepository, uow.TodoItemRepository))
            {
                if(item.IsActive )
                {
                    item = (await uow.TodoListRepository.GetAll().ConfigureAwait(false)).FirstOrDefault();
                    if (item != null)
                    {
                        return await this.SetNewMainTodoList(item.ListId);
                    }
                }
               
                return true;
            }

            return false;
        }
    }
}
