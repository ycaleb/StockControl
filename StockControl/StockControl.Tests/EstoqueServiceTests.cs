using Moq;
using StockControl.Application.Interfaces;
using StockControl.Application.Services;
using StockControl.Models;

namespace StockControl.Tests
{
    public class EstoqueServiceTests
    {
        [Fact]
        public async Task MovimentarAsync_EntradaAumentaEstoque()
        {
            // Arrange
            var material = new Material { Id = 1,Nome = "Cimento",QuantidadeEstoque = 10,CustoUnitario = 5 };

            var materialRepoMock = new Mock<IMaterialRepository>();
            materialRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(material);
            materialRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Material>())).Returns(Task.CompletedTask);

            var movRepoMock = new Mock<IMovimentoEstoqueRepository>();
            movRepoMock.Setup(r => r.AddAsync(It.IsAny<MovimentoEstoque>())).Returns(Task.CompletedTask);

            var service = new EstoqueService(materialRepoMock.Object,movRepoMock.Object);

            var dto = new MovimentoEstoqueCreateDTO
            {
                MaterialId = 1,
                Quantidade = 5,
                Tipo = "entrada"
            };

            // Act
            var result = await service.MovimentarAsync(dto);

            // Assert
            Assert.True(result.sucesso);
            Assert.Equal(15,result.saldo);
        }
    }
}
