using Moq;
using StockControl.Application.Interfaces;
using StockControl.Application.Services;
using StockControl.Models;

namespace StockControl.Tests
{
    public class MaterialServiceTests
    {
        private readonly Mock<IMaterialRepository> _mockRepo = new();
        private readonly MaterialService _service;

        public MaterialServiceTests()
        {
            _service = new MaterialService(_mockRepo.Object);
        }

        [Fact]
        public async Task Create_DeveAdicionarMaterial()
        {
            // Arrange
            var material = new Material
            {
                Nome = "Cimento",
                UnidadeMedida = "Kg",
                CustoUnitario = 10
            };

            // Act
            await _service.CreateAsync(material);

            // Assert
            _mockRepo.Verify(x => x.AddAsync(material),Times.Once);
        }

        [Fact]
        public async Task Update_DeveChamarUpdate()
        {
            var material = new Material
            {
                Id = 1,
                Nome = "Cimento",
                UnidadeMedida = "Kg",
                CustoUnitario = 10
            };

            _mockRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(material);

            var input = new Material
            {
                Nome = "Cimento Premium",
                UnidadeMedida = "Kg",
                CustoUnitario = 12
            };

            await _service.UpdateAsync(1,input);

            Assert.Equal("Cimento Premium",material.Nome);
            Assert.Equal(12,material.CustoUnitario);
            _mockRepo.Verify(x => x.UpdateAsync(material),Times.Once);
        }

        [Fact]
        public async Task Delete_DeveRemoverMaterial()
        {
            var material = new Material { Id = 1 };
            _mockRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(material);

            await _service.DeleteAsync(1);

            _mockRepo.Verify(x => x.RemoveAsync(material),Times.Once);
        }

        [Fact]
        public async Task GetAll_DeveRetornarTodosMateriais()
        {
            var lista = new List<Material>
        {
            new Material { Id = 1, Nome = "Cimento" },
            new Material { Id = 2, Nome = "Areia" }
        };
            _mockRepo.Setup(x => x.ListAllAsync()).ReturnsAsync(lista);

            var result = await _service.GetAllAsync();
            Assert.Equal(2,result.Count);
        }

        [Fact]
        public async Task GetById_MaterialNaoEncontrado_DeveRetornarNull()
        {
            _mockRepo.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Material?)null);

            var result = await _service.GetByIdAsync(99);

            Assert.Null(result);
        }
    }
}