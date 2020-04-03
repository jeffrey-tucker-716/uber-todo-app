using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Database.Models;



namespace Todo.Database.Tests
{
    /// <summary>
    /// This set of unit tests will exercise the model but all transactions take place in memory.
    /// </summary>
    public class TodoCRUDChecks
    {
        readonly List<TodoItem> todosToAdd = new List<TodoItem>()
        {
            new TodoItem() { TodoItemId = Guid.NewGuid().ToString(), IsComplete=true, TodoTitle="Start with database schema", WhenEntered=DateTime.UtcNow },
            new TodoItem() { TodoItemId = Guid.NewGuid().ToString(), IsComplete=false, TodoTitle="Scaffold the models in Entity Framework database class library", WhenEntered=DateTime.UtcNow },
            new TodoItem() { TodoItemId = Guid.NewGuid().ToString(), IsComplete=false, TodoTitle="Initialize in Startup", WhenEntered=DateTime.UtcNow },
            new TodoItem() { TodoItemId = Guid.NewGuid().ToString(), IsComplete=false, TodoTitle="Run CRUD tests in NUnit", WhenEntered=DateTime.UtcNow }
        };

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void WhenAddingTodosThenTheyExistOnReadback()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
               .UseInMemoryDatabase(databaseName: "Add_todos_to_database")
               .Options;

            using (var context = new TodoContext(options))
            {
                context.TodoItems.AddRange(todosToAdd.ToArray());
                context.SaveChanges();

                // verify the changes are there.
                Assert.AreEqual(todosToAdd.Count, context.TodoItems.Count());
                foreach (var item in context.TodoItems)
                {
                    TraceTodoItem(item);
                }

                // find the first one
                var firstItem = context.TodoItems.Where(r => r.TodoItemId == todosToAdd[0].TodoItemId).FirstOrDefault();
                Assert.IsNotNull(firstItem);

                var lastItem = context.TodoItems.Where(r => r.TodoItemId == todosToAdd[3].TodoItemId).FirstOrDefault();
                Assert.IsNotNull(lastItem);
            }
        }

        [Test]
        public void WhenUpdatingTodoItemThenChangesAreThereOnReadback()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
              .UseInMemoryDatabase(databaseName: "Update_todos_to_database")
              .Options;

            using (var context = new TodoContext(options))
            {
                context.TodoItems.AddRange(todosToAdd.ToArray());
                context.SaveChanges();

                var foundItem = context.TodoItems.Where(r => r.TodoTitle == "Initialize in Startup").FirstOrDefault();
                Assert.IsTrue(foundItem != null);
                Console.WriteLine("Found:");
                TraceTodoItem(foundItem);

                foundItem.TodoTitle = "Initialize in webapp Startup";
                foundItem.IsComplete = true;
                foundItem.WhenCompleted = DateTime.UtcNow;
                context.Update(foundItem);
                context.SaveChanges();

                var changedItem = context.TodoItems.Where(r => r.TodoTitle == "Initialize in webapp Startup").FirstOrDefault();
                Assert.IsTrue(changedItem != null);
                Assert.IsTrue(changedItem.IsComplete);
                Console.WriteLine("Changed to:");
                TraceTodoItem(changedItem);
               
            }
        }

        [Test]
        public void WhenDeleteOneItemThenTheCountDecrements()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
              .UseInMemoryDatabase(databaseName: "Delete_todos_to_database")
              .Options;
            using (var context = new TodoContext(options))
            {
                context.TodoItems.AddRange(todosToAdd.ToArray());
                context.SaveChanges();
                Assert.AreEqual(todosToAdd.Count, context.TodoItems.Count());
                // there should be 4
                var foundItem = context.TodoItems.Where(r => r.TodoTitle == "Initialize in Startup").FirstOrDefault();
                Assert.IsNotNull(foundItem);
                context.Remove(foundItem);
                context.SaveChanges();

                Assert.AreEqual(todosToAdd.Count - 1, context.TodoItems.Count()); 
            }
        }

        void TraceTodoItem(TodoItem todoItem)
        {
            Console.WriteLine($"Title={todoItem.TodoTitle}, WhenEntered={todoItem.WhenEntered}, Id={todoItem.TodoItemId}, IsComplete={todoItem.IsComplete}, WhenCompleted={todoItem.WhenCompleted}");
        }

    }
}