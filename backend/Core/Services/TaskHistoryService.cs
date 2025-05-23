﻿using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;

namespace Core.Services;

public class TaskHistoryService
{
    private readonly ITaskHistoryRepository _taskHistoryRepository;
    public TaskHistoryService(ITaskHistoryRepository taskHistoryRepository)
    {
        _taskHistoryRepository = taskHistoryRepository;
    }

    public async Task<TaskHistory> GetTaskHistoryById(int id)
    {
        var taskHistory = await _taskHistoryRepository.GetByIdAsync(id);

        if (taskHistory is null)
            throw new KeyNotFoundException("TaskHistory not found");

        return taskHistory;
    }

    public async Task<IEnumerable<TaskHistory>> GetTaskHistoryAll()
    {
        return await _taskHistoryRepository.GetAllAsync();
    }

    public void AddTaskHistory(int id, int taskId, int stateId, DateTime changedDate) 
    {
        var taskHistory = Core.Factories.TaskHistoryFactory.CreateTaskHistory(id, taskId, stateId, changedDate);

        _taskHistoryRepository.Add(taskHistory);
    }

    public void AddTaskHistoryRange(IEnumerable<TaskHistory> taskHistories)
    {
        _taskHistoryRepository.AddRange(taskHistories);
    }

    public void UpdateTaskHistory(TaskHistory taskHistory)
    {
        var existingState = _taskHistoryRepository.GetByIdAsync(taskHistory.Id).Result;

        if (existingState is null)
            throw new KeyNotFoundException("TaskHistory to update not found");

        _taskHistoryRepository.Update(taskHistory);
    }

    public void DeleteTaskHistory(TaskHistory taskHistory)
    {
        var existingTaskHistory = _taskHistoryRepository.GetByIdAsync(taskHistory.Id).Result;

        if (existingTaskHistory is null)
            throw new KeyNotFoundException("TaskHistory to delete not found");

        _taskHistoryRepository.Remove(taskHistory);
    }
}
