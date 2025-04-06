// We use an explicit alias for Core.Entities.Task
using Core.Entities;

namespace Core.Factories
{
    public static class TaskHistoryFactory
    {
        // Method for creating tasks
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
