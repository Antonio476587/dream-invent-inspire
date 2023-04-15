using Microsoft.Azure.Cosmos;

namespace TodoApp.Models.Interfaces;

public interface ITodoAdapter
{
    FeedIterator<Todo> GetTodoList();
    Task<Todo> GetTodo(Guid todoId);
    Task<bool> CreateTodo(Todo todoToCreate);
    Task<bool> EditTodo(Guid todoId, Todo newTodo);
    void DeleteTodo(Guid todoId);
    Task<bool> ExistTodo(Guid todoId);
}