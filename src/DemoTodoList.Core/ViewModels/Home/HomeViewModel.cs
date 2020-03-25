using DemoTodoList.Shared;
using DemoTodoList.UserCases.TodoListUseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Acr.UserDialogs;
using DemoTodoList.Core.Helper;
using MvvmCross.Commands;
using System.Windows.Input;
using MvvmCross.Navigation;

namespace DemoTodoList.Core.ViewModels.Home
{
    public class HomeViewModel : BaseViewModel
    {
        private ITodoListUseCase _todoListUseCase;
        private IUserDialogs _userDialogs;
        private IMvxNavigationService _navigationService;

        public IMvxCommand RefreshTodoListCommand { get; private set; }
        public IMvxAsyncCommand<Guid> MakeMainListCommand { get; set; }
        public IMvxAsyncCommand AddNewListCommand { get; set; }        
        public IMvxAsyncCommand<Guid> DeleteCommand => new MvxAsyncCommand<Guid>(DeleteTodoList);

        private ObservableCollection<TodoList> _todoLists;
        public ObservableCollection<TodoList> TodoLists
        {
            get
            {
                return _todoLists;
            }
            set
            {
                _todoLists = value;
                RaisePropertyChanged(() => TodoLists);
            }
        }
        
        public TodoList SelectedTodoLists
        {
            get
            {
                return null;
            }
            set
            {
              
                if(value!=null)
                {
                    _navigationService.Navigate<TodoListViewModel, TodoList>(value);
                }
                RaisePropertyChanged(() => SelectedTodoLists);
            }
        }



        public HomeViewModel(ITodoListUseCase todoListUseCase, IUserDialogs userDialogs, IMvxNavigationService navigationService)
        {
            this._todoListUseCase = todoListUseCase;
            this._userDialogs = userDialogs;
            this.TodoLists = new ObservableCollection<TodoList>();
            RefreshTodoListCommand = new MvxAsyncCommand(GetTodoLists);
            MakeMainListCommand = new MvxAsyncCommand<Guid>(MakeMainList);
            AddNewListCommand = new MvxAsyncCommand(CreateNewTodoList);
            _navigationService = navigationService;
        }

        public override Task Initialize()
        {
            GetTodoLists();
            return Task.FromResult(0);
        }

     

        private async Task MakeMainList(Guid todoListId)
        {
            try
            {
                if (await this._userDialogs.ConfirmAsync("Do you want to make it the main Todo List?"))
                {
                    this.IsBusy = true;
                    if(await this._todoListUseCase.SetNewMainTodoList(todoListId))
                    {
                        await this._userDialogs.AlertAsync("Done");
                        
                    }
                    else
                    {
                        await this._userDialogs.AlertAsync("There was a problem, please try again","Error");
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
                await GetTodoLists();
            }

        }

        private async Task DeleteTodoList(Guid todoListId)
        {
            try
            {
                if (await this._userDialogs.ConfirmAsync("Do you want to delete this list?", "", "Yes", "No"))
                {
                    this.IsBusy = true;
                   
                    if(await  this._todoListUseCase.DeleteTodoList(todoListId))
                    {
                        await this.GetTodoLists();
                    }
                    else
                    {
                        await this._userDialogs.AlertAsync("The list could not be deleted, try again");
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

        private async Task GetTodoLists()
        {
            try
            {
                this.IsBusy = true;
                var result = await this._todoListUseCase.GetTodoLists();

                if (!result.Any())
                {
                    await CreateNewTodoList();
                }

                result = result.OrderByDescending(x => x.IsActive);

                this.TodoLists = new ObservableCollection<TodoList>(result);
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


        private async Task CreateNewTodoList()
        {
            try
            {
               

                PromptResult promptResult = await this._userDialogs.PromptAsync("Let's create a new Todo List", "Add a todo list", "Save", null, "Title", InputType.Default).ConfigureAwait(false);
                if(!string.IsNullOrWhiteSpace(promptResult.Value))
                {
                    this.IsBusy = true;
                    var result= await this._todoListUseCase.CreateTodoList(promptResult.Value);
                    if(result==null )
                    {
                      await  this._userDialogs.AlertAsync("The todo list could not be created");
                    }else
                    {
                        this.TodoLists.Add(result);
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
