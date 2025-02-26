// Usamos un alias explícito para Core.Entities.Task
using Core.Entities;

namespace Core.Factories
{
    public static class StateFactory
    {
        // Método para crear tareas
        public static State CreateState(string name)
        {
            return new State
            {
                Name = name
            };
        }
    }
}
