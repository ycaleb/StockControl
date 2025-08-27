
[EN]
# StockControl.API

.NET 8 API for expense control, with inventory control and PDF report export (QuestPDF) of desired materials.

## Technologies
- .NET 8 (ASP.NET Core Web API)
- EF Core InMemory
- Swagger (documentation)
- QuestPDF (PDF Report)

## How to run
```bash
dotnet restore
dotnet run
```

## Endpoints

### Materials
- `GET /api/materials` — list materials
- `GET /api/materials/{id}` — details
- `POST /api/materials` — create
- `PUT /api/materials/{id}` — update
- `DELETE /api/materials/{id}` — remove

Example of material:
```json
{
  “name”: “CP-II Cement”,
  “unitOfMeasure”: “bag”,
  “unitCost”: 35.50,
  “stockQuantity”: 0
}
```

### Inventory
- `POST /api/inventory/move` — entry/exit
- `GET /api/stock/movements?materialId=1` — list movements

Example of movement (entry):
```json
{
  “materialId”: 1,
  “quantity”: 10,
  “type”: “entry”,
  “observation”: “Purchase from supplier X”
}
```

Example of movement (output):
```json
{
  “materialId”: 1,
  “quantity”: 3,
  “type”: “output”,
  “observation”: “Use in project Y”
}
```

### Reports
- `GET /api/reports/expenses/pdf?start=2025-01-01&end=2025-12-31` — downloads PDF with expenses (only “output”).

## Next steps
⬜ Improvements in validations
⬜ Frontend - React
⬜ JWT token

---

[PT]
# StockControl.API

API .NET 8 para controle de gastos, com controle de estoque e exportação de relatório em PDF (QuestPDF) dos materiais desejados.

## Tecnologias
- .NET 8 (ASP.NET Core Web API)
- EF Core InMemory
- Swagger (documentação)
- QuestPDF (Relatório PDF)

## Como rodar
```bash
dotnet restore
dotnet run
```

## Endpoints

### Materiais
- `GET /api/materiais` — lista materiais
- `GET /api/materiais/{id}` — detalhe
- `POST /api/materiais` — cria
- `PUT /api/materiais/{id}` — atualiza
- `DELETE /api/materiais/{id}` — remove

Exemplo de material:
```json
{
  "nome": "Cimento CP-II",
  "unidadeMedida": "saco",
  "custoUnitario": 35.50,
  "quantidadeEstoque": 0
}
```

### Estoque
- `POST /api/estoque/movimentar` — entrada/saída
- `GET /api/estoque/movimentos?materialId=1` — lista movimentos

Exemplo de movimento (entrada):
```json
{
  "materialId": 1,
  "quantidade": 10,
  "tipo": "entrada",
  "observacao": "Compra fornecedor X"
}
```

Exemplo de movimento (saída):
```json
{
  "materialId": 1,
  "quantidade": 3,
  "tipo": "saida",
  "observacao": "Uso na obra Y"
}
```

### Relatórios
- `GET /api/relatorios/gastos/pdf?inicio=2025-01-01&fim=2025-12-31` — baixa PDF com gastos (somente "saída").

## Próximos passos
⬜ Frontend - React
