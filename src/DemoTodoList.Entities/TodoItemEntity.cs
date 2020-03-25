using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoTodoList.Entities
{
   public class TodoItemEntity
    {
        [PrimaryKey]
        public Guid TodoId { get; set; }
        public Guid ListId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }




        public async Task<bool> Save(IRepository<TodoItemEntity> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (string.IsNullOrWhiteSpace(this.Title))
            {
                throw new ArgumentException("The Todo Title is required");
            }

            if (string.IsNullOrWhiteSpace(this.Description ))
            {
                throw new ArgumentException("The Todo description is required");
            }

            if (TodoId == Guid.Empty)
            {
                this.TodoId = Guid.NewGuid();
                return await repository.Insert(this);
            }
            else
            {
                return await repository.Update(this);
            }

        }
    }
}
