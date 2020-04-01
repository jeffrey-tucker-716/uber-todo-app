using System;
using System.Collections.Generic;

namespace Todo.Database.Models
{
    public partial class TodoItems
    {
        public TodoItems()
        {
            RememberItems = new HashSet<RememberItems>();
        }

        public string TodoItemId { get; set; }
        public bool IsComplete { get; set; }
        public string TodoTitle { get; set; }
        public DateTime WhenEntered { get; set; }
        public DateTime? WhenCompleted { get; set; }

        public virtual ICollection<RememberItems> RememberItems { get; set; }
    }
}
