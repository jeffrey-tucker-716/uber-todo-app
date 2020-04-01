using System;
using System.Collections.Generic;

namespace Todo.Database.Models
{
    public partial class RememberItems
    {
        public string RememberItemId { get; set; }
        public string RememberItemTitle { get; set; }
        public DateTime WhenEntered { get; set; }
        public string TodoItemId { get; set; }
        public DateTime? WhenConverted { get; set; }

        public virtual TodoItems TodoItem { get; set; }
    }
}
