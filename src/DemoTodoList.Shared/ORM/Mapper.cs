using DemoTodoList.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoTodoList.Shared.ORM
{
   public static class Mapper
    {
        public static TodoItem ConverToDto(this TodoItemEntity todoItem )
        {
            var newValue = new TodoItem();
            newValue.Description = todoItem.Description;
            newValue.IsCompleted = todoItem.IsCompleted;
            newValue.ListId = todoItem.ListId;
            newValue.Title = todoItem.Title;
            newValue.TodoId = todoItem.TodoId;
            return newValue;
        }

        public static TodoItemEntity ConverToEntity(this TodoItem todoItem)
        {
            var newValue = new TodoItemEntity();
            newValue.Description = todoItem.Description;
            newValue.IsCompleted = todoItem.IsCompleted;
            newValue.ListId = todoItem.ListId;
            newValue.Title = todoItem.Title;
            newValue.TodoId = todoItem.TodoId;
            return newValue;
        }

        public static TodoList  ConverToDto(this TodoListEntity todoItem)
        {
            var newValue = new TodoList();
            newValue.IsActive = todoItem.IsActive;
            newValue.ListId = todoItem.ListId;           
            newValue.Title = todoItem.Title;
         
            return newValue;
        }

        public static TodoListEntity ConverToEntity(this TodoList todoItem)
        {
            var newValue = new TodoListEntity();
            newValue.IsActive = todoItem.IsActive;
            newValue.ListId = todoItem.ListId;
            newValue.Title = todoItem.Title;
            return newValue;
        }
    }
}
