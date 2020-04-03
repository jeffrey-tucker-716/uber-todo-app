using Queries.Core.Repositories;
using System.Collections.Generic;
using Todo.Database.Models;

namespace Todo.Services.Interfaces
{
    public interface ITodoRepository : IRepository<TodoItem>
    {
        IEnumerable<TodoItem> GetIncompleteItems();

        IEnumerable<TodoItem> GetCompletedItems();

        IEnumerable<TodoItem> GetItemsByUser(string userId);

        IEnumerable<TodoItem> GetRemindersByUser(string userId);

        IEnumerable<TodoItem> GetCompletedItemsByUser(string userId);    // might need date ranges

    }
}
