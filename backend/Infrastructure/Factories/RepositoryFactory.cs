using Core.Interfases;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure.Factories
{
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

        // Aquí podrías agregar otros repositorios si los necesitaras
    }
}
