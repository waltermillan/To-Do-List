// Usamos un alias explícito para Core.Entities.Task
using TaskEntity = Core.Entities.Task;

namespace Core.Factories
{
    public static class TaskFactory
    {
        // Método para crear tareas
        public static TaskEntity CreateTask(string name, int stateId, DateTime initialDate, DateTime finishDate, bool? done = null)
        {
            // Usamos Core.Entities.Task para crear la tarea
            return new TaskEntity
            {
                Name = name,
                StateId = stateId,
                InitialDate = initialDate,
                FinishDate = finishDate,
                Done = done
            };
        }
    }
}
