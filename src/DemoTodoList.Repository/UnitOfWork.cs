using DemoTodoList.Entities;
using DemoTodoList.UserCases;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoTodoList.Repository
{
   public class UnitOfWork: IUnitOfWork
    {
        IRepository<TodoListEntity> todoListRepository;
        IRepository<TodoItemEntity> todoItemRepository;
        private DBContext _context;

        public UnitOfWork()
        {
            _context = new DBContext();

        }

        public IRepository<TodoItemEntity> TodoItemRepository
        {
            get
            {
                if (todoItemRepository == null)
                {
                    todoItemRepository = new Repository<TodoItemEntity>(_context);
                }
                return todoItemRepository;
            }
        }

        public IRepository<TodoListEntity> TodoListRepository
        {
            get
            {
                if (todoListRepository == null)
                {
                    todoListRepository = new Repository<TodoListEntity>(_context);
                }
                return todoListRepository;
            }
        }

    }
}
