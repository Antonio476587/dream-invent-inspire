using dii.storage;
using dii.storage.cosmos;
using dii.storage.cosmos.Models;
using Microsoft.Azure.Cosmos;
using TodoApp.Models;
using TodoApp.Models.Interfaces;

namespace TodoApp.Adapters;

public class TodoAdapter : DiiCosmosAdapter<Todo>, ITodoAdapter
{
    DiiCosmosContext diiCosmosContext { get; }
    Container TodoContainer { get; }

    public TodoAdapter() {
        diiCosmosContext = DiiCosmosContext.Get();  
        TodoContainer = diiCosmosContext.Db.GetContainer("Todos");
    }

    public async Task<bool> CreateTodo(Todo todoToCreate)
    {
        Todo createdTodo = await base.CreateAsync(todoToCreate).ConfigureAwait(false);

        if (createdTodo.id == todoToCreate.id) return true;
        return false;
    }

    public async void DeleteTodo(string todoId)
    {
        await base.DeleteAsync(todoId, todoId).ConfigureAwait(false);
    }

    public async Task<bool> ExistTodo(string todoId)
    {
        Todo todo = await base.GetAsync(todoId, todoId).ConfigureAwait(false);
        if (todo != null) return true;
        return false;
    }

    public async Task<Todo> GetTodo(string todoId)
    {
        return await base.GetAsync(todoId, todoId).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Todo>> GetTodoList()
    {
        PagedList<Todo> pagedList = await base.GetPagedAsync("SELECT * FROM c");

        return pagedList.AsEnumerable();
    }

    public async Task<bool> EditTodo(string todoId, Todo newTodo)
    {
        var patchItemRequestOptions = new PatchItemRequestOptions
        {
            EnableContentResponseOnWrite = true,
        };

        var patchOperations = new List<PatchOperation>()
        {
            PatchOperation.Set<string>("/Title", newTodo.Title?? "N/A"),
            PatchOperation.Set<string>("/Content", newTodo.Content?? " "),
            PatchOperation.Set<int>("/Status", newTodo.Status),
        };

        Todo createdTodo = await base.PatchAsync(todoId, todoId, patchOperations, patchItemRequestOptions).ConfigureAwait(false);
        
        if (createdTodo.id == newTodo.id) return true;
        return false;

    }
}