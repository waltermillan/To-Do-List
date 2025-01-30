using Core.Entities;
using Core.Interfases;
using Core.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Tests.UnitTests
{
    public class StateServiceTests
    {
        [Fact]
        public async System.Threading.Tasks.Task GetStateById_ReturnsState_WhenStateExists()
        {
            // Arrange
            var mockStateRepository = new Mock<IStateRepository>();
            var state = new State { Id = 1, Name = "State1" };
            mockStateRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(state);

            var stateService = new StateService(mockStateRepository.Object);

            // Act
            var result = await stateService.GetStateById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("State1", result.Name);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllStates_ReturnsStates_WhenStatesExist()
        {
            // Arrange
            var mockStateRepository = new Mock<IStateRepository>();
            var states = new List<State>
        {
            new State { Id = 1, Name = "State1" },
            new State { Id = 2, Name = "State2" }
        };
            mockStateRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(states);

            var stateService = new StateService(mockStateRepository.Object);

            // Act
            var result = await stateService.GeAllStates();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); // Usamos Count() para contar los elementos
            Assert.Contains(result, p => p.Name == "State1");
            Assert.Contains(result, p => p.Name == "State2");
        }

        [Fact]
        public void AddState_AddsState_WhenStateIsValid()
        {
            // arrange
            var mockStateRepository = new Mock<IStateRepository>();
            var state = new State { Id = 9, Name = "State1" };

            mockStateRepository.Setup(repo => repo.Add(It.IsAny<State>()));

            var stateService = new StateService(mockStateRepository.Object);

            // act
            stateService.AddState(state);

            // assert
            mockStateRepository.Verify(repo => repo.Add(It.Is<State>(c => c.Name == "State1" && c.Id == 9)), Times.Once);
        }

        [Fact]
        public void AddRange_AddsMultipleStates_WhenStatesAreValid()
        {
            // Arrange
            var mockStateRepository = new Mock<IStateRepository>();
            var states = new List<State>
                {
                    new State { Id = 9, Name = "State1" },
                    new State { Id = 13, Name = "State2" }
                };

            // Configuramos el mock para verificar que AddRange sea llamado con la lista correcta de gobiernos
            mockStateRepository.Setup(repo => repo.AddRange(It.IsAny<IEnumerable<State>>()));

            var stateService = new StateService(mockStateRepository.Object);

            // Act
            stateService.AddStates(states);

            // Assert
            mockStateRepository.Verify(repo => repo.AddRange(It.Is<IEnumerable<State>>(c => c.Count() == 2 && c.Any(x => x.Name == "State1") && c.Any(x => x.Name == "State2"))), Times.Once);
        }

        [Fact]
        public void UpdateState_UpdatesState_WhenStateIsValid()
        {
            // arrange
            var mockStateRepository = new Mock<IStateRepository>();
            var state = new State { Id = 9, Name = "NewState1" };

            // Configuramos el mock para que devuelva el lenguaje que estamos eliminando
            mockStateRepository.Setup(repo => repo.GetByIdAsync(9)).ReturnsAsync(new State { Id = 9, Name = "NewState1" }); // Simulamos que el estado con Id 9 existe


            mockStateRepository.Setup(repo => repo.Add(It.IsAny<State>()));

            var stateService = new StateService(mockStateRepository.Object);

            // act
            stateService.UpdateState(state);

            // assert
            mockStateRepository.Verify(repo => repo.Update(It.Is<State>(c => c.Name == "NewState1" && c.Id == 9)), Times.Once);
        }

        [Fact]
        public void DeleteState_DeletesState_WhenStateExists()
        {
            // arrange
            var mockStateRepository = new Mock<IStateRepository>();
            var state = new State { Id = 9, Name = "State1" };

            // Configuramos el mock para que devuelva el estado que estamos eliminando
            mockStateRepository.Setup(repo => repo.GetByIdAsync(9)).ReturnsAsync(new State { Id = 9, Name = "State1" }); // Simulamos que el estado con Id 9 existe

            mockStateRepository.Setup(repo => repo.Remove(It.IsAny<State>()));

            var stateService = new StateService(mockStateRepository.Object);

            // act
            stateService.DeleteState(state);

            // assert
            mockStateRepository.Verify(repo => repo.Remove(It.Is<State>(c => c.Name == "State1" && c.Id == 9)), Times.Once);
        }

        [Fact]
        public void UpdateState_ThrowsException_WhenStateToUpdateDoesNotExist()
        {
            // Arrange
            var mockStateRepository = new Mock<IStateRepository>();
            var state = new State { Id = 999, Name = "State" }; // ID que no existe
            mockStateRepository.Setup(repo => repo.GetByIdAsync(state.Id)).ReturnsAsync((State)null); // Simulamos que el estado no existe.

            var stateService = new StateService(mockStateRepository.Object);

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() => stateService.UpdateState(state));
            Assert.Equal("State to update not found", exception.Message); // Verificamos que el mensaje de la excepción sea el esperado
        }

        [Fact]
        public async System.Threading.Tasks.Task GetStateById_ThrowsException_WhenStateDoesNotExist()
        {
            // Arrange
            var mockStateRepository = new Mock<IStateRepository>();
            var stateId = 999; // ID que no existe en la base de datos

            // Configuramos el mock para que devuelva el lenguage que estamos buscando
            mockStateRepository.Setup(repo => repo.GetByIdAsync(9)).ReturnsAsync(new State { Id = 999, Name = "State" }); // Simulamos que el estado con Id 999 que no existe

            mockStateRepository.Setup(repo => repo.GetByIdAsync(stateId)).ReturnsAsync((State)null); // Simulamos que no se encuentra el continente.

            var stateService = new StateService(mockStateRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => stateService.GetStateById(stateId));
            Assert.Equal("State not found", exception.Message); // Verificamos que el mensaje de la excepción sea el esperado
        }
    }
}
