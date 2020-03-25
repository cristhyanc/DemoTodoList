using MvvmCross.IoC;
using MvvmCross.ViewModels;
using DemoTodoList.Core.ViewModels.Home;
using MvvmCross;
using DemoTodoList.UserCases.TodoListUseCases;
using DemoTodoList.Repository;
using DemoTodoList.UserCases;
using Acr.UserDialogs;
using MvvmCross.Plugin;
using DemoTodoList.UserCases.TodoItemUserCases;

namespace DemoTodoList.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<HomeViewModel>();
            Mvx.IoCProvider.RegisterType<IUnitOfWork, UnitOfWork>();
            Mvx.IoCProvider.RegisterType<ITodoListUseCase, TodoListUseCase>();
            Mvx.IoCProvider.RegisterType<ITodoItemUseCase, TodoItemUseCase>();            
        }

       
    }
}
