using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure.Factories;

public static class RepositoryFactory
{
    public static ITaskRepository CreateTaskRepository(TodoListContext context)
    {
        return new TaskRepository(context);
    }

    public static IStateRepository CreateStateRepository(TodoListContext context)
    {
        return new StateRepository(context);
    }

    public static ITaskHistoryRepository CreateTaskHistoryRepository(TodoListContext context)
    {
        return new TaskHistoryRepository(context);
    }

    public static IUserRepository CreateUser(TodoListContext context)
    {
        return new UserRepository(context);
    }

    /* add other repositories if necessary */
}
