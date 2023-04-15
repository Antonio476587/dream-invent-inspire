using Microsoft.Azure.Cosmos;

namespace TodoApp.Models.Interfaces;

public interface ITodoAdapter
{
    Task<IEnumerable<Todo>> GetTodoList();
    Task<Todo> GetTodo(string todoId);
    Task<bool> CreateTodo(Todo todoToCreate);
    Task<bool> EditTodo(string todoId, Todo newTodo);
    void DeleteTodo(string todoId);
    Task<bool> ExistTodo(string todoId);
}