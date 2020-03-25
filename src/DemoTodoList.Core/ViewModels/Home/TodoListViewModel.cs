using Acr.UserDialogs;
using DemoTodoList.Core.Helper;
using DemoTodoList.Shared;
using DemoTodoList.UserCases.TodoItemUserCases;
using DemoTodoList.UserCases.TodoListUseCases;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTodoList.Core.ViewModels.Home
{
    public class TodoListViewModel : BaseViewModel<TodoList>
    {
        private ITodoItemUseCase _todoItemUseCase;
        private IUserDialogs _userDialogs;
        private IMvxNavigationService _navigationService;
        private ITodoListUseCase _todoListUseCase;

        public IMvxCommand RefreshTodoItemCommand { get; private set; }
        public IMvxAsyncCommand<Guid> MarkAsCompleteCommand { get; private set; }
        public IMvxAsyncCommand AddNewItemCommand { get; private set; }
        public IMvxAsyncCommand<Guid> DeleteCommand => new MvxAsyncCommand<Guid>(DeleteTodoItem);

       

        TodoList _todoList;
        public TodoList TodoList
        {
            get
            {
                return _todoList;
            }
            set
            {
                _todoList = value;
                RaisePropertyChanged(() => TodoList);
            }
        }

        private ObservableCollection<TodoItem> _todoItems;
        public ObservableCollection<TodoItem> TodoItems
        {
            get
            {
                return _todoItems;
            }
            set
            {
                _todoItems = value;
                RaisePropertyChanged(() => TodoItems);
            }
        }




        public TodoListViewModel(ITodoListUseCase todoListUseCase, ITodoItemUseCase todoItemUseCase, IUserDialogs userDialogs, IMvxNavigationService navigationService)
        {
            this._todoListUseCase = todoListUseCase;
            this._todoItemUseCase = todoItemUseCase;
            this._userDialogs = userDialogs;
            this.TodoItems = new ObservableCollection<TodoItem>();
            RefreshTodoItemCommand = new MvxAsyncCommand(GetTodoItems);
            MarkAsCompleteCommand = new MvxAsyncCommand<Guid>(MarkTodoAsCompleted);
            AddNewItemCommand = new MvxAsyncCommand(CreateNewTodoItem);
            _navigationService = navigationService;
        }

        public override void Prepare(TodoList parameter)
        {
            TodoList = parameter;
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            await GetTodoItems();
        }

        private async Task DeleteTodoItem(Guid todoItemId)
        {
            try
            {
                if(await this._userDialogs.ConfirmAsync("Do you want to delete this todo?","","Yes","No"))
                {
                    this.IsBusy = true;
                    var result = await this._todoItemUseCase.DeleteTodoItem (todoItemId);

                    if (result)
                    {
                        var item = this.TodoItems.Where(x => x.TodoId == todoItemId).FirstOrDefault();
                        this.TodoItems.Remove(item);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ProcessException(ex, this._userDialogs);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async Task MarkTodoAsCompleted(Guid todoItemId)
        {
            try
            {
                this.IsBusy = true;
                var result = await this._todoItemUseCase.MarkTodoAsCompleted(todoItemId);

                if (result)
                {
                  await  GetTodoItems();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ProcessException(ex, this._userDialogs);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async Task GetTodoItems()
        {
            try
            {
                this.IsBusy = true;
                var result = await this._todoListUseCase.GetTodoItems(this.TodoList.ListId);

                if (result.Any()  )
                {
                    result = result.OrderBy(x => x.Title).OrderBy(x => x.IsCompleted);
                    this.TodoItems = new ObservableCollection<TodoItem>(result);
                }               
            }
            catch (Exception ex)
            {
                ExceptionHandler.ProcessException(ex, this._userDialogs);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async Task CreateNewTodoItem()
        {
            try
            {
                string title;
                PromptResult promptResult = await this._userDialogs.PromptAsync("Let's create a new Todo", "Add a todo list", "Next", null, "Title", InputType.Default).ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(promptResult.Value))
                {
                  
                    title = promptResult.Value;

                    promptResult = await this._userDialogs.PromptAsync("One more step", "Add a todo list", "Save", null, "Description", InputType.Default).ConfigureAwait(false);
                    
                    if (!string.IsNullOrWhiteSpace(promptResult.Value))
                    {                       
                        this.IsBusy = true;
                        var result = await this._todoItemUseCase.CreateTodoItem(title, promptResult.Value, this.TodoList.ListId);
                        if (result == null)
                        {
                            await this._userDialogs.AlertAsync("The todo Item could not be created");
                        }
                        else
                        {
                            this.TodoItems.Add(result);
                        }
                    }    
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ProcessException(ex, this._userDialogs);
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}
