using Core.Entities;
using Core.Interfases;
using Core.Services;
using Infrastructure.Repositories;
using Moq;

namespace Tests.UnitTests;

public class TaskHistoryServiceTests
{
    [Fact]
    public async System.Threading.Tasks.Task GetTaskHistoryById_ReturnsTaskHistory_WhenTaskHistoryExists()
    {
        // Arrange
        var mockTaskHistoryRepository = new Mock<ITaskHistoryRepository>();
        var task = new TaskHistory { Id = 9};
        mockTaskHistoryRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(task);

        var taskHistoryService = new TaskHistoryService(mockTaskHistoryRepository.Object);

        // Act
        var result = await taskHistoryService.GetTaskHistoryById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(9, result.Id);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetAllTaskHistories_ReturnsTaskHistories_WhenTaskHistoriesExist()
    {
        // Arrange
        var mockTaskHistoryRepository = new Mock<ITaskHistoryRepository>();
        var tasks = new List<TaskHistory>
        {
            new TaskHistory { Id = 9 },
            new TaskHistory { Id = 13 }
        };
        mockTaskHistoryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tasks);

        var taskHistoryService = new TaskHistoryService(mockTaskHistoryRepository.Object);

        // Act
        var result = await taskHistoryService.GetTaskHistoryAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count()); // Usamos Count() para contar los elementos
        Assert.Contains(result, p => p.Id == 9);
        Assert.Contains(result, p => p.Id == 13);
    }

    [Fact]
    public void AddTaskHistory_AddsTaskHistory_WhenTaskHistoryIsValid()
    {
        // arrange
        var mockTaskHistoryRepository = new Mock<ITaskHistoryRepository>();
        var taskHistory = new Core.Entities.TaskHistory { Id = 9};

        mockTaskHistoryRepository.Setup(repo => repo.Add(It.IsAny<TaskHistory>()));

        var taskHistoryService = new TaskHistoryService(mockTaskHistoryRepository.Object);

        // act
        taskHistoryService.AddTaskHistory(taskHistory.Id, taskHistory.TaskId, taskHistory.StateId, taskHistory.ChangedDate);

        // assert
        mockTaskHistoryRepository.Verify(repo => repo.Add(It.Is<TaskHistory>(c => c.Id == 9)), Times.Once);
    }

    [Fact]
    public void AddRange_AddsMultipleTaskHistories_WhenTaskHistoriesAreValid()
    {
        // Arrange
        var mockTaskHistoryRepository = new Mock<ITaskHistoryRepository>();
        DateTime date = DateTime.Now;
        var taskHistories = new List<TaskHistory>
                {
                    new TaskHistory { Id = 9, ChangedDate = date, StateId = 1,  TaskId = 1 },
                    new TaskHistory { Id = 13, ChangedDate = date, StateId = 1,  TaskId = 2 },
                };

        // Configuramos el mock para verificar que AddRange sea llamado con la lista correcta de gobiernos
        mockTaskHistoryRepository.Setup(repo => repo.AddRange(It.IsAny<IEnumerable<TaskHistory>>()));

        var stateService = new TaskHistoryService(mockTaskHistoryRepository.Object);

        // Act
        stateService.AddTaskHistoryRange(taskHistories);

        // Assert
        mockTaskHistoryRepository.Verify(repo => repo.AddRange(It.Is<IEnumerable<TaskHistory>>(c => c.Count() == 2 && c.Any(x => x.Id == 9) && c.Any(x => x.ChangedDate == date) && c.Any(x => x.StateId == 1)  && c.Any(x => x.TaskId == 1)
                                                                                                                   && c.Any(x => x.Id == 13) && c.Any(x => x.ChangedDate == date) && c.Any(x => x.StateId == 1) && c.Any(x => x.TaskId == 2))), Times.Once);
    }

    [Fact]
    public void UpdateTaskHistory_UpdatesTaskHistory_WhenTaskHistoryIsValid()
    {
        // arrange
        var mockTaskHistoryRepository = new Mock<ITaskHistoryRepository>();
        var taskHistory = new TaskHistory { Id = 9 };

        // Configuramos el mock para que devuelva el continente que estamos eliminando
        mockTaskHistoryRepository.Setup(repo => repo.GetByIdAsync(9)).ReturnsAsync(new TaskHistory { Id = 9 }); // Simulamos que el tarea con Id 9 existe

        mockTaskHistoryRepository.Setup(repo => repo.Add(It.IsAny<TaskHistory>()));

        var taskHistoryService = new TaskHistoryService(mockTaskHistoryRepository.Object);

        // act
        taskHistoryService.UpdateTaskHistory(taskHistory);

        // assert
        mockTaskHistoryRepository.Verify(repo => repo.Update(It.Is<TaskHistory>(c => c.Id == 9)), Times.Once);
    }

    [Fact]
    public void DeleteTaskHistory_DeletesTaskHistory_WhenTaskHistoryExists()
    {
        // arrange
        var mockTaskHistoryRepository = new Mock<ITaskHistoryRepository>();
        var taskHistory = new TaskHistory { Id = 9};

        // Configuramos el mock para que devuelva el continente que estamos eliminando
        mockTaskHistoryRepository.Setup(repo => repo.GetByIdAsync(9)).ReturnsAsync(new TaskHistory { Id = 9 }); // Simulamos que el tarea con Id 9 existe

        mockTaskHistoryRepository.Setup(repo => repo.Remove(It.IsAny<TaskHistory>()));

        var taskHistoryService = new TaskHistoryService(mockTaskHistoryRepository.Object);

        // act
        taskHistoryService.DeleteTaskHistory(taskHistory);

        // assert
        mockTaskHistoryRepository.Verify(repo => repo.Remove(It.Is<TaskHistory>(c =>  c.Id == 9)), Times.Once);
    }

    [Fact]
    public void UpdateTaskHistory_ThrowsException_WhenTaskHistoryToUpdateDoesNotExist()
    {
        // Arrange
        var mockTaskHistoryRepository = new Mock<ITaskHistoryRepository>();
        var taskHistory = new TaskHistory { Id = 999, StateId = 1, TaskId = 1 }; // ID que no existe
        mockTaskHistoryRepository.Setup(repo => repo.GetByIdAsync(taskHistory.Id)).ReturnsAsync((TaskHistory)null); // Simulamos que el estado no existe.

        var taskHistoryService = new TaskHistoryService(mockTaskHistoryRepository.Object);

        // Act & Assert
        var exception = Assert.Throws<KeyNotFoundException>(() => taskHistoryService.UpdateTaskHistory(taskHistory));
        Assert.Equal("TaskHistory to update not found", exception.Message); // Verificamos que el mensaje de la excepción sea el esperado
    }

    [Fact]
    public async System.Threading.Tasks.Task GetTaskHistoryById_ThrowsException_WhenTaskHistoryDoesNotExist()
    {
        // Arrange
        var mockTaskHistoryRepository = new Mock<ITaskHistoryRepository>();
        var taskHistoryId = 999; // ID que no existe en la base de datos

        // Simulamos que no se encuentra el país con el ID 999
        mockTaskHistoryRepository.Setup(repo => repo.GetByIdAsync(taskHistoryId)).ReturnsAsync((TaskHistory)null); // Devuelve null para el ID 999

        var taskHistoryService = new TaskHistoryService(mockTaskHistoryRepository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => taskHistoryService.GetTaskHistoryById(taskHistoryId));

        // Asegúrate de que el mensaje de la excepción sea el esperado
        Assert.Equal("TaskHistory not found", exception.Message); // Verifica que el mensaje de la excepción sea "Task not found"
    }

}
