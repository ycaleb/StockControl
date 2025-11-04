# StockControl.API

Backend-only. 

API REST em **.NET 8** para controle de materiais e estoque, com:

- Clean Architecture
- Clean Code
- Autenticação JWT
- CRUD de usuários e materiais
- Movimentações de estoque (entrada/saída)
- Relatórios em PDF (QuestPDF)
- Seeds automáticos
- Swagger

Projeto simples, direto e funcional — ideal como referência de backend.

---

## 🧠 Arquitetura

**Clean Architecture (Camadas)**

StockControl.API → Controllers / Auth / Swagger
StockControl.Application → Services / DTOs / Regras de negócio
StockControl.Domain → Entidades / Interfaces / Regras de domínio
StockControl.Infrastructure → EF Core / DbContext / Repositórios

- Baixo acoplamento  
- Separação clara de responsabilidades  
- Repository pattern + DI  

---

## ✅ Funcionalidades

| Feature | Status |
|---|---|
Autenticação JWT | ✅
Login e seed do usuário admin | ✅
CRUD Usuários | ✅
CRUD Materiais | ✅
Movimentação de estoque | ✅
Relatório PDF de gastos | ✅
Testes unitários | ✅

---

## 🚀 Tecnologias

- .NET 8 Web API  
- EF Core  
- JWT Authentication  
- QuestPDF  
- Swagger  
- Testes unitários

---

## ▶️ Execução da API

```bash
dotnet restore
dotnet run
Swagger abre em:
https://localhost:5001/swagger

Usuário inicial (seed):
email: admin@admin.com
senha: 123

📂 Endpoints
🔐 Autenticação
Método	Rota	Descrição
POST	/api/usuarios/login	Login e geração JWT

👤 Usuários
Método	Rota
GET	    /api/usuarios
POST	/api/usuarios
PUT	    /api/usuarios/{id}
DELETE	/api/usuarios/{id}

📦 Materiais
Método	Rota
GET	    /api/materiais
POST	/api/materiais
PUT	    /api/materiais/{id}
DELETE	/api/materiais/{id}

📊 Movimentações de Estoque
Método	Rota
POST	/api/estoque/movimentar
GET	    /api/estoque/movimentos?materialId=1

Regras:
Não permite estoque negativo
Calcula valor total
Registra histórico

🧾 Relatórios PDF
Método	Rota
GET	/api/relatorios/gastos/pdf?inicio=01/01/2025&fim=31/12/2025

Gera PDF com:
Materiais
Quantidade
Total gasto (saídas)
Filtro por data
```
---

## 🇺🇸 English Version
# StockControl.API

Backend-only.

REST API in **.NET 8** for material and inventory control, featuring:

- Clean Architecture  
- Clean Code  
- JWT Authentication  
- Users & Materials CRUD  
- Stock movements (in/out)  
- PDF reports (QuestPDF)  
- Automatic seeds  
- Swagger  

Simple, direct, and functional — ideal as a backend reference.

---

## 🧠 Architecture

**Clean Architecture (Layers)**

StockControl.API → Controllers / Auth / Swagger  
StockControl.Application → Services / DTOs / Business rules  
StockControl.Domain → Entities / Interfaces / Domain rules  
StockControl.Infrastructure → EF Core / DbContext / Repositories  

- Low coupling  
- Clear separation of concerns  
- Repository pattern + DI  

---

## ✅ Features

| Feature | Status |
|---|---|
JWT Authentication | ✅
Admin login & seed | ✅
Users CRUD | ✅
Materials CRUD | ✅
Stock movement | ✅
PDF spending report | ✅
Unit tests | ✅

---

## 🚀 Tech Stack

- .NET 8 Web API  
- EF Core  
- JWT Authentication  
- QuestPDF  
- Swagger  
- Unit tests  

---

## ▶️ Run the API

```bash
dotnet restore
dotnet run
Swagger available at:
https://localhost:5001/swagger

Initial user (seed):
user: admin
password: 123

📂 Endpoints

🔐 Authentication - Login and JWT generation
Method	Route	
POST	/api/usuarios/login	

👤 Users
Method	Route
GET	    /api/usuarios
POST	/api/usuarios
PUT	    /api/usuarios/{id}
DELETE	/api/usuarios/{id}

📦 Materials
Method	Route
GET	    /api/materiais
POST	/api/materiais
PUT	    /api/materiais/{id}
DELETE	/api/materiais/{id}

📊 Stock Movements
Method	Route
POST	/api/estoque/movimentar
GET	    /api/estoque/movimentos?materialId=1

Rules:
No negative stock allowed
Calculates total value
Logs history

🧾 PDF Reports
Method	Route
GET	    /api/relatorios/gastos/pdf?inicio=01/01/2025&fim=31/12/2025

Generates PDF with:
Materials
Quantity
Total spent (outputs)
Date filter