using Core.Entities;
using Core.Interfases;
using System.Collections.Generic;
using System.Numerics;

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

    public void AddTask(Core.Entities.Task task) 
    { 
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
