using System;
using System.Collections.Generic;
using System.Text;

namespace DemoTodoList.Shared
{
  public  class TodoList
    {


        public Guid ListId { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<TodoItem > Items { get; set; }
    }
}
