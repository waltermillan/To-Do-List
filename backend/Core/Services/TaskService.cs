using Core.Entities;
using Core.Interfases;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using TaskEntity = Core.Entities.Task;

namespace Core.Services;

public class TaskService
{
    private readonly ITaskRepository _taskRepository;
    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Core.Entities.Task> GetTaskById(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            throw new KeyNotFoundException("Task not found");
        }
        return task;
    }

    public async Task<IEnumerable<Core.Entities.Task>> GetCountryAll()
    {
        return await _taskRepository.GetAllAsync();
    }

    public void AddTask(string name, int stateId, DateTime initialDate, DateTime finishDate, bool? done = null)
    {
        // Usamos el TaskFactory para crear la tarea
        var task = Core.Factories.TaskFactory.CreateTask(name, stateId, initialDate, finishDate, done);

        // Ahora agregamos la tarea usando el repositorio
        _taskRepository.Add(task);
    }

    public void AddTaskRange(IEnumerable<Core.Entities.Task> tasks)
    {
        _taskRepository.AddRange(tasks);
    }

    public void UpdateTask(Core.Entities.Task task)
    {
        var existingTask = _taskRepository.GetByIdAsync(task.Id).Result;
        if (existingTask == null)
        {
            throw new KeyNotFoundException("Task to update not found");
        }
        _taskRepository.Update(task);
    }

    public void DeleteTask(Core.Entities.Task task)
    {
        var existingTask = _taskRepository.GetByIdAsync(task.Id).Result;
        if (existingTask == null)
        {
            throw new KeyNotFoundException("Task to delete not found");
        }
        _taskRepository.Remove(task);
    }
}
