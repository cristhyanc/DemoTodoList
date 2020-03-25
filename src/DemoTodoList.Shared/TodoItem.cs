using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoTodoList.Shared
{
  public  class TodoItem
    {
        public Guid TodoId { get; set; }
        public Guid ListId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

       

    }
}
