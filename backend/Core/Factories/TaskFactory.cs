// We use an explicit alias for Core.Entities.Task
using TaskEntity = Core.Entities.Task;

namespace Core.Factories;

public static class TaskFactory
{
    // The FACTORY PATTERN is a creative design pattern used to create objects
    // without specifying the exact class of the object to be created.
    public static TaskEntity CreateTask(string name, int stateId, DateTime initialDate, DateTime finishDate, bool? done = null)
    {
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
