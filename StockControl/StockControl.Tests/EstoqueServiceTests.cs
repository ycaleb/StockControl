using Moq;
using StockControl.Application.Interfaces;
using StockControl.Application.Services;
using StockControl.Models;

namespace StockControl.Tests
{
    public class EstoqueServiceTests
    {
        private readonly Mock<IMaterialRepository> _materialRepoMock = new();
        private readonly Mock<IMovimentoEstoqueRepository> _movRepoMock = new();
        private readonly EstoqueService _service;

        public EstoqueServiceTests()
        {
            _service = new EstoqueService(_materialRepoMock.Object,_movRepoMock.Object);
        }

        [Fact]
        public async Task MovimentarAsync_EntradaAumentaEstoque()
        {
            var material = new Material { Id = 1,Nome = "Cimento",QuantidadeEstoque = 10,CustoUnitario = 5 };
            _materialRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(material);

            var dto = new MovimentoEstoqueCreateDTO { MaterialId = 1,Quantidade = 5,Tipo = "entrada" };

            var result = await _service.MovimentarAsync(dto);

            Assert.True(result.sucesso);
            Assert.Equal(15,result.saldo);
            _movRepoMock.Verify(r => r.AddAsync(It.IsAny<MovimentoEstoque>()),Times.Once);
            _materialRepoMock.Verify(r => r.UpdateAsync(material),Times.Once);
        }

        [Fact]
        public async Task MovimentarAsync_SaidaValidaDiminuiEstoque()
        {
            var material = new Material { Id = 1,Nome = "Cimento",QuantidadeEstoque = 10,CustoUnitario = 5 };
            _materialRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(material);

            var dto = new MovimentoEstoqueCreateDTO { MaterialId = 1,Quantidade = 4,Tipo = "saida" };

            var result = await _service.MovimentarAsync(dto);

            Assert.True(result.sucesso);
            Assert.Equal(6,result.saldo);
            _movRepoMock.Verify(r => r.AddAsync(It.IsAny<MovimentoEstoque>()),Times.Once);
            _materialRepoMock.Verify(r => r.UpdateAsync(material),Times.Once);
        }

        [Fact]
        public async Task MovimentarAsync_MaterialNaoEncontrado_DeveFalhar()
        {
            _materialRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Material?)null);

            var dto = new MovimentoEstoqueCreateDTO { MaterialId = 1,Quantidade = 5,Tipo = "entrada" };

            var result = await _service.MovimentarAsync(dto);

            Assert.False(result.sucesso);
            Assert.Equal("Material não encontrado",result.mensagem);
            _movRepoMock.Verify(r => r.AddAsync(It.IsAny<MovimentoEstoque>()),Times.Never);
            _materialRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Material>()),Times.Never);
        }

        [Fact]
        public async Task MovimentarAsync_TipoInvalido_DeveFalhar()
        {
            var material = new Material { Id = 1,Nome = "Cimento",QuantidadeEstoque = 10 };
            _materialRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(material);

            var dto = new MovimentoEstoqueCreateDTO { MaterialId = 1,Quantidade = 5,Tipo = "transferencia" };

            var result = await _service.MovimentarAsync(dto);

            Assert.False(result.sucesso);
            Assert.Equal("Tipo deve ser 'entrada' ou 'saida'",result.mensagem);
            Assert.Equal(10,result.saldo);
            _movRepoMock.Verify(r => r.AddAsync(It.IsAny<MovimentoEstoque>()),Times.Never);
            _materialRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Material>()),Times.Never);
        }

        [Fact]
        public async Task MovimentarAsync_SaidaComQuantidadeZeroOuNegativa_DeveFalhar()
        {
            var material = new Material { Id = 1,Nome = "Cimento",QuantidadeEstoque = 10 };
            _materialRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(material);

            var dto = new MovimentoEstoqueCreateDTO { MaterialId = 1,Quantidade = 0,Tipo = "saida" };

            var result = await _service.MovimentarAsync(dto);

            Assert.False(result.sucesso);
            Assert.Equal("Quantidade inválida",result.mensagem);
            _movRepoMock.Verify(r => r.AddAsync(It.IsAny<MovimentoEstoque>()),Times.Never);
            _materialRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Material>()),Times.Never);
        }

        [Fact]
        public async Task MovimentarAsync_SaidaComEstoqueInsuficiente_DeveFalhar()
        {
            var material = new Material { Id = 1,Nome = "Cimento",QuantidadeEstoque = 3 };
            _materialRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(material);

            var dto = new MovimentoEstoqueCreateDTO { MaterialId = 1,Quantidade = 10,Tipo = "saida" };

            var result = await _service.MovimentarAsync(dto);

            Assert.False(result.sucesso);
            Assert.Equal("Estoque insuficiente",result.mensagem);
            Assert.Equal(3,result.saldo);
            _movRepoMock.Verify(r => r.AddAsync(It.IsAny<MovimentoEstoque>()),Times.Never);
            _materialRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Material>()),Times.Never);
        }

        [Fact]
        public async Task ListarMovimentosAsync_DeveRetornarLista()
        {
            var lista = new List<MovimentoEstoque>
            {
                new MovimentoEstoque { Id = 1, Tipo = "entrada", Quantidade = 10 },
                new MovimentoEstoque { Id = 2, Tipo = "saida", Quantidade = 5 }
            };

            _movRepoMock.Setup(r => r.ListarAsync(null)).ReturnsAsync(lista);

            var result = await _service.ListarMovimentosAsync(null);

            Assert.Equal(2,result.Count);
        }
    }
}
