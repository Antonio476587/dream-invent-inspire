using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dii.storage.cosmos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;
using Microsoft.Azure.Cosmos;

namespace todoApp.Controllers
{
    public class TodosController : Controller
    {

        DiiCosmosContext diiCosmosContext { get; set; }

        public TodosController() {
            diiCosmosContext = DiiCosmosContext.Get();
        }
        // GET: Todos
        public IActionResult Index()
        {
            var todo = diiCosmosContext.Db.GetContainer("Todos").GetItemQueryIterator<Todo>("SELECT * FROM c");

            return View(todo);
        }

        // GET: Todos/Details/cc2e02b3-17e2-4078-bdf5-aa035351943b
        public async Task<IActionResult> Details(Guid? id)
        {   
            if (id == null)
            {
                return NotFound();
            }

            Todo todo = await diiCosmosContext.Db.GetContainer("Todos").ReadItemAsync<Todo>($"{id}", new PartitionKey($"{id}")).ConfigureAwait(false);

            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // GET: Todos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,Status")] Todo todo)
        {
            todo.id = Guid.NewGuid();
            todo.CreatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                await diiCosmosContext.Db.GetContainer("Todos").CreateItemAsync<Todo>(todo).ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
        
            Todo todo = await diiCosmosContext.Db.GetContainer("Todos").ReadItemAsync<Todo>($"{id}", new PartitionKey($"{id}")).ConfigureAwait(false);

            if (todo == null)
            {
                return NotFound();
            }
            return View(todo);
        }

        // POST: Todos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,Content,Status")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                // diiCosmosContext.Db.GetContainer("Todos").PatchItemAsync<Todo>($"{id}", new PartitionKey($"{id}"), PatchOperation.Set<Todo>("Todos/Items", todo)).ConfigureAwait(false);
                // I have to be honest, I didn't know how to patch the todo object... I try I bit more I will know, but for now...
                Todo todoTwo = await diiCosmosContext.Db.GetContainer("Todos").ReadItemAsync<Todo>($"{id}", new PartitionKey($"{id}")).ConfigureAwait(false);
                todo.CreatedAt = todoTwo.CreatedAt;
                todo.id = todoTwo.id;

                await diiCosmosContext.Db.GetContainer("Todos").DeleteItemAsync<Todo>($"{id}", new PartitionKey($"{id}")).ConfigureAwait(false);

                await diiCosmosContext.Db.GetContainer("Todos").CreateItemAsync<Todo>(todo).ConfigureAwait(false);

                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Todo todo = await diiCosmosContext.Db.GetContainer("Todos").ReadItemAsync<Todo>($"{id}", new PartitionKey($"{id}")).ConfigureAwait(false);

            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: Todos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Todo todo = await diiCosmosContext.Db.GetContainer("Todos").ReadItemAsync<Todo>($"{id}", new PartitionKey($"{id}")).ConfigureAwait(false);

            if (todo != null)
            {
                await diiCosmosContext.Db.GetContainer("Todos").DeleteItemAsync<Todo>($"{id}", new PartitionKey($"{id}")).ConfigureAwait(false);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TodoExists(Guid id)
        {
            Todo todo = await diiCosmosContext.Db.GetContainer("Todos").ReadItemAsync<Todo>($"{id}", new PartitionKey($"{id}"));
            if (todo != null) return true;
            return false;
        }
    }
}
