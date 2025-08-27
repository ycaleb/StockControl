using StockControl.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace StockControl.Services
{
    public class RelatorioService
    {
        public byte[] GerarRelatorioGastos(List<MovimentoEstoque> movimentos,DateTime? inicio = null,DateTime? fim = null)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var periodo = $"{(inicio.HasValue ? inicio.Value.ToString("dd/MM/yyyy") : "--")} a {(fim.HasValue ? fim.Value.ToString("dd/MM/yyyy") : "--")}";

            var entradas = movimentos.Where(m => m.Tipo == "entrada").ToList();
            var saidas = movimentos.Where(m => m.Tipo == "saida").ToList();

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(t => t.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("Relatório de Movimentações - Construção Civil")
                            .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);
                        col.Item().Text($"Período: {periodo}").FontSize(10);
                    });

                    page.Content().Column(col =>
                    {
                        if(entradas.Any())
                        {
                            col.Item().Text("Entradas").SemiBold().FontSize(14).FontColor(Colors.Green.Medium);
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(70);
                                    columns.RelativeColumn(2);
                                    columns.ConstantColumn(50);
                                    columns.ConstantColumn(70);
                                    columns.RelativeColumn(1);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Data").SemiBold();
                                    header.Cell().Text("Material").SemiBold();
                                    header.Cell().Text("Qtd").SemiBold();
                                    header.Cell().Text("Custo Unit.").SemiBold();
                                    header.Cell().Text("Valor").SemiBold();
                                });

                                foreach(var mov in entradas)
                                {
                                    table.Cell().Text(mov.Data.ToLocalTime().ToString("dd/MM/yyyy"));
                                    table.Cell().Text(mov.Material.Nome);
                                    table.Cell().Text(mov.Quantidade.ToString());
                                    table.Cell().Text($"R$ {mov.Material.CustoUnitario:F2}");
                                    table.Cell().Text($"R$ {mov.ValorTotal:F2}");
                                }
                            });
                        }

                        if(saidas.Any())
                        {
                            col.Item().Text("Saídas").SemiBold().FontSize(14).FontColor(Colors.Red.Medium);
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(70);
                                    columns.RelativeColumn(2);
                                    columns.ConstantColumn(50);
                                    columns.ConstantColumn(70);
                                    columns.RelativeColumn(1);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Data").SemiBold();
                                    header.Cell().Text("Material").SemiBold();
                                    header.Cell().Text("Qtd").SemiBold();
                                    header.Cell().Text("Custo Unit.").SemiBold();
                                    header.Cell().Text("Valor").SemiBold();
                                });

                                foreach(var mov in saidas)
                                {
                                    table.Cell().Text(mov.Data.ToLocalTime().ToString("dd/MM/yyyy"));
                                    table.Cell().Text(mov.Material.Nome);
                                    table.Cell().Text(mov.Quantidade.ToString());
                                    table.Cell().Text($"R$ {mov.Material.CustoUnitario:F2}");
                                    table.Cell().Text($"R$ {mov.ValorTotal:F2}");
                                }
                            });
                        }
                    });

                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("Total geral: ").SemiBold();
                        txt.Span($" R$ {movimentos.Sum(m => m.ValorTotal):F2}");
                        txt.Span("  |  ");
                        txt.Span($"Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}");
                    });
                });
            });

            return documento.GeneratePdf();
        }
    }
}