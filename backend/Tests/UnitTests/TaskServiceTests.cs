using Core.Interfaces;
using Core.Services;
using Moq;

namespace Tests.UnitTests;

public class TaskServiceTests
{
    [Fact]
    public async System.Threading.Tasks.Task GetTaskById_ReturnsTask_WhenTaskExists()
    {
        // Arrange
        var mockTaskRepository = new Mock<ITaskRepository>();
        var task = new Core.Entities.Task { Id = 9, Name = "Task1" };
        mockTaskRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(task);

        var taskService = new TaskService(mockTaskRepository.Object);

        // Act
        var result = await taskService.GetTaskById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Task1", result.Name);
        Assert.Equal(9, result.Id);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetAllTasks_ReturnsTasks_WhenTasksExist()
    {
        // Arrange
        var mockTaskRepository = new Mock<ITaskRepository>();
        var tasks = new List<Core.Entities.Task>
        {
            new Core.Entities.Task { Id = 9, Name = "Task1" },
            new Core.Entities.Task { Id = 13, Name = "Task2" }
        };
        mockTaskRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tasks);

        var taskService = new TaskService(mockTaskRepository.Object);

        // Act
        var result = await taskService.GetCountryAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, p => p.Name == "Task1");
        Assert.Contains(result, p => p.Name == "Task2");
    }

    [Fact]
    public void AddTask_AddsTask_WhenCountryIsValid()
    {
        // arrange
        var mockTaskRepository = new Mock<ITaskRepository >();
        DateTime now = DateTime.Now;
        var task = new Core.Entities.Task { Id = 9, Name = "Task1", StateId = 1, FinishDate = now, InitialDate = now, Done = false };

        mockTaskRepository.Setup(repo => repo.Add(It.IsAny<Core.Entities.Task>()));

        var taskService = new TaskService(mockTaskRepository.Object);

        // act
        taskService.AddTask(task.Name, task.StateId, task.InitialDate, task.FinishDate, task.Done);

        // assert
        mockTaskRepository.Verify(repo => repo.Add(It.Is<Core.Entities.Task>(c => c.Name == "Task1" && c.StateId == 1 && c.FinishDate == now && c.InitialDate == now && c.Done == false)), Times.Once);
    }

    [Fact]
    public void AddRange_AddsMultipleTasks_WhenTasksAreValid()
    {
        // Arrange
        var mockTaskRepository = new Mock<ITaskRepository>();
        var tasks = new List<Core.Entities.Task>
    {
        new Core.Entities.Task { Id = 9, Name = "Task1" },
        new Core.Entities.Task { Id = 13, Name = "Task1" }
    };

        mockTaskRepository.Setup(repo => repo.AddRange(It.IsAny<IEnumerable<Core.Entities.Task>>()));

        var taskService = new TaskService(mockTaskRepository.Object);

        // Act
        taskService.AddTaskRange(tasks);

        // Assert
        mockTaskRepository.Verify(repo => repo.AddRange(It.Is<IEnumerable<Core.Entities.Task>>(c => c.Count() == 2 && c.Any(x => x.Name == "Task1") && c.Any(x => x.Name == "Task1"))), Times.Once);
    }

    [Fact]
    public void UpdateTask_UpdatesTask_WhenCountryIsValid()
    {
        // arrange
        var mockTaskRepository = new Mock<ITaskRepository>();
        var task = new Core.Entities.Task { Id = 9, Name = "NewTask1" };

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(9)).ReturnsAsync(new Core.Entities.Task { Id = 9, Name = "NewTask1" });

        mockTaskRepository.Setup(repo => repo.Add(It.IsAny<Core.Entities.Task>()));

        var taskService = new TaskService(mockTaskRepository.Object);

        // act
        taskService.UpdateTask(task);

        // assert
        mockTaskRepository.Verify(repo => repo.Update(It.Is<Core.Entities.Task>(c => c.Name == "NewTask1" && c.Id == 9)), Times.Once);
    }

    [Fact]
    public void DeleteTask_DeletesTask_WhenCountryExists()
    {
        // arrange
        var mockTaskRepository = new Mock<ITaskRepository>();
        var task = new Core.Entities.Task { Id = 9, Name = "NewTask1" };

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(9)).ReturnsAsync(new Core.Entities.Task { Id = 9, Name = "NewTask1" });

        mockTaskRepository.Setup(repo => repo.Remove(It.IsAny<Core.Entities.Task>()));

        var taskService = new TaskService(mockTaskRepository.Object);

        // act
        taskService.DeleteTask(task);

        // assert
        mockTaskRepository.Verify(repo => repo.Remove(It.Is<Core.Entities.Task>(c => c.Name == "NewTask1" && c.Id == 9)), Times.Once);
    }

    [Fact]
    public void UpdateTask_ThrowsException_WhenTaskToUpdateDoesNotExist()
    {
        // Arrange
        var mockTaskRepository = new Mock<ITaskRepository>();
        var task = new Core.Entities.Task { Id = 999, Name = "NonExistingTask" };

        // Configuramos el mock para que devuelva el continente que estamos actalizando
        mockTaskRepository.Setup(repo => repo.GetByIdAsync(9)).ReturnsAsync(new Core.Entities.Task { Id = 999, Name = "NonExistingTask" });

        mockTaskRepository.Setup(repo => repo.GetByIdAsync(task.Id)).ReturnsAsync((Core.Entities.Task)null);

        var taskService = new TaskService(mockTaskRepository.Object);

        // Act & Assert
        var exception = Assert.Throws<KeyNotFoundException>(() => taskService.UpdateTask(task));
        Assert.Equal("Task to update not found", exception.Message);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetCountryById_ThrowsException_WhenCountryDoesNotExist()
    {
        // Arrange
        var mockTaskRepository = new Mock<ITaskRepository>();
        var taskId = 999; // ID que no existe en la base de datos

        // Simulamos que no se encuentra el país con el ID 999
        mockTaskRepository.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync((Core.Entities.Task)null);

        var taskService = new TaskService(mockTaskRepository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => taskService.GetTaskById(taskId));

        Assert.Equal("Task not found", exception.Message);
    }

}
