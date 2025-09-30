# CheckpointCSharp

# Checkpoint C# - Gerenciamento de Biblioteca

## 🎯 Tema e Objetivo
O tema escolhido para este projeto foi o **Gerenciamento de Biblioteca**.  

A aplicação permite **controlar livros, autores e usuários**, oferecendo funcionalidades completas de cadastro, consulta, atualização e exclusão. Dessa forma, é possível organizar e manipular os registros de uma biblioteca de maneira prática, garantindo a persistência dos dados em um banco relacional por meio do **Entity Framework Core**.  

O objetivo principal é **simular um sistema real de gestão de acervo** que poderia ser utilizado em bibliotecas, escolas ou centros de estudo, aplicando na prática os conceitos de programação em C# aliados a um ORM moderno.

---

## ⚙️ Configuração do Projeto

### Pré-requisitos
- [.NET 6 ou superior](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/)  
- [SQL Server](https://www.microsoft.com/sql-server/) ou outro banco compatível com Entity Framework Core
- Pacotes do NuGet:  
  - `Microsoft.EntityFrameworkCore`  
  - `Microsoft.EntityFrameworkCore.SqlServer`  
  - `Microsoft.EntityFrameworkCore.Tools`  

### Passos para Configuração
1. Clone o repositório:
   ```bash
   git clone <URL_DO_REPOSITORIO>
   ```

2. Acesse a pasta do projeto:
   ```bash
   cd checkpoint-csharp
   ```

3. Configure a **connection string** no arquivo `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=CheckpointDB;Trusted_Connection=True;"
     }
   }
   ```

4. Crie e aplique as migrações do Entity Framework:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

---

## ▶️ Execução

Para rodar o projeto:

```bash
dotnet run
```

A aplicação será iniciada no terminal (se for console app) ou abrirá no navegador (se for web app).  

---

## 👥 Integrantes

- Juliana Villalpando Maita — 99224  
- João Victor Dos Santos Morais — 550453  
- Luana Cabezaolias Miguel — 99320  
- Lucca Vilaça Okubo — 551538  
- Pedro Henrique Pontes Farath — 98608  

---
