using System;
using System.Collections.Generic;

namespace Todo.Database.Models
{
    public partial class TodoItem
    {
        public TodoItem()
        {
            RememberItems = new HashSet<RememberItem>();
        }

        public string TodoItemId { get; set; }
        public bool IsComplete { get; set; }
        public string TodoTitle { get; set; }
        public DateTime WhenEntered { get; set; }
        public DateTime? WhenCompleted { get; set; }

        public virtual ICollection<RememberItem> RememberItems { get; set; }
    }
}
