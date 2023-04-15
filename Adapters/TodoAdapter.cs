using dii.storage.cosmos;
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
        Todo createdTodo = await TodoContainer.CreateItemAsync<Todo>(todoToCreate).ConfigureAwait(false);

        if (createdTodo.id == todoToCreate.id) return true;
        return false;
    }

    public async void DeleteTodo(Guid todoId)
    {
        await TodoContainer.DeleteItemAsync<Todo>($"{todoId}", new PartitionKey($"{todoId}")).ConfigureAwait(false);
    }

    public async Task<bool> ExistTodo(Guid todoId)
    {
        Todo todo = await TodoContainer.ReadItemAsync<Todo>($"{todoId}", new PartitionKey($"{todoId}")).ConfigureAwait(false);
        if (todo != null) return true;
        return false;
    }

    public async Task<Todo> GetTodo(Guid todoId)
    {
        Todo todo = await TodoContainer.ReadItemAsync<Todo>($"{todoId}", new PartitionKey($"{todoId}")).ConfigureAwait(false);
        return todo;
    }

    public FeedIterator<Todo> GetTodoList()
    {
        return TodoContainer.GetItemQueryIterator<Todo>("SELECT * FROM c");
    }

    public async Task<bool> EditTodo(Guid todoId, Todo newTodo)
    {
        var patchItemRequestOptions = new PatchItemRequestOptions
        {
            EnableContentResponseOnWrite = false,
        };

        var patchOperations = new List<PatchOperation>()
        {
            PatchOperation.Set<string>("/Title", newTodo.Title?? "N/A"),
            PatchOperation.Set<string>("/Content", newTodo.Content?? " "),
            PatchOperation.Set<int>("/Status", newTodo.Status)
        };

        Todo createdTodo = await TodoContainer.PatchItemAsync<Todo>($"{todoId}", new PartitionKey($"{todoId}"), patchOperations, patchItemRequestOptions).ConfigureAwait(false);

        if (createdTodo.id == newTodo.id) return true;
        return false;

    }
}