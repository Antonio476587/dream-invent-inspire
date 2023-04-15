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
using TodoApp.Adapters;

namespace todoApp.Controllers
{
    public class TodosController : Controller
    {

        TodoAdapter todoAdapter { get; }

        public TodosController() {
            todoAdapter = new TodoAdapter();
        }
        // GET: Todos
        public async Task<IActionResult> Index()
        {
            var todo = await todoAdapter.GetTodoList();

            return View(todo);
        }

        // GET: Todos/Details/cc2e02b3-17e2-4078-bdf5-aa035351943b
        public async Task<IActionResult> Details(string? id)
        {   
            if (id == null)
            {
                return NotFound();
            }

            // Todo todo = await todoAdapter.GetTodo(id.ToString());
            Todo todo = await todoAdapter.GetTodo(id);

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
            todo.id = Guid.NewGuid().ToString();
            todo.CreatedAt = DateTime.Now;
            todo.DataVersion = "1";

            if (ModelState.IsValid && todo.Status >= 0 && todo.Status < 2)
            {
                if (await todoAdapter.CreateTodo(todo)) {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(todo);
        }

        // GET: Todos/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
        
            Todo todo = await todoAdapter.GetTodo(id);

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
        public async Task<IActionResult> Edit(string id, [Bind("Title,Content,Status")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                todo.id = id;
                if (await todoAdapter.EditTodo(id, todo)) {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(todo);
        }

        // GET: Todos/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Todo todo = await todoAdapter.GetTodo(id);

            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: Todos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            if (await todoAdapter.ExistTodo(id))
            {
                todoAdapter.DeleteTodo(id);
            }
            
            return RedirectToAction(nameof(Index));
        }

    }
}
