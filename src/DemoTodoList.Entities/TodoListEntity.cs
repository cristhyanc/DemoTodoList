using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoTodoList.Entities
{
  public  class TodoListEntity
    {
        [PrimaryKey]
        public Guid ListId { get; set; }
        public string Title { get; set; }        
        public bool IsActive { get; set; }
     

        public async Task<bool>  Save(IRepository<TodoListEntity> repository)
        {
            if(repository==null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if(string.IsNullOrWhiteSpace(this.Title ))
            {
                throw new ArgumentException("The Title is required");
            }

            if(ListId==Guid.Empty )
            {
                this.ListId = Guid.NewGuid();
                return await repository.Insert(this);
            }
            else
            {
                return await repository.Update(this);
            }
          
        }

    }
}
