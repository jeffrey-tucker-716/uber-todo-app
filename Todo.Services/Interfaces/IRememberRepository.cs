using Queries.Core.Repositories;
using Todo.Database.Models;

namespace Todo.Services.Interfaces
{
    interface IRememberRepository : IRepository<RememberItem>
    {
    }
}
