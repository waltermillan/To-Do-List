// Usamos un alias explícito para Core.Entities.Task
using Core.Entities;
using TaskEntity = Core.Entities.Task;

namespace Core.Factories
{
    public static class TaskHistoryFactory
    {
        // Método para crear tareas
        public static TaskHistory CreateTaskHistory(int id, int taskId, int stateId, DateTime changedDate)
        {
            return new TaskHistory
            {
                Id = id,
                TaskId = taskId,
                StateId = stateId,
                ChangedDate = changedDate
            };
        }
    }
}
