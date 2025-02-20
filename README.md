# ManipulaeHealth.DesafioBack

API desenvolvida para consumir dados do YouTube e armazená-los em um banco SQLite.  

## 🚀 Tecnologias Utilizadas  
- .NET Core  
- Entity Framework Core  
- SQLite  
- Swagger (para documentação da API)  

---

## 📂 **Pré-requisitos**  
Antes de começar, você precisará ter instalado em sua máquina:  
- [.NET SDK](https://dotnet.microsoft.com/en-us/download)  
- [SQLite](https://www.sqlite.org/download.html) (opcional, caso queira visualizar os dados diretamente)  

---

## 📦 **Configuração do Projeto**  

### 1️ **Restaurar Dependências**  
- dotnet restore

### 2 **Criar o Banco de Dados e Aplicar Migrações**
- dotnet ef database update
- **caso precise refazer as migracoes:**
- dotnet ef migrations remove
- dotnet ef migrations add InitialCreate
- dotnet ef database update


### 3 **Executar Projeto**
- dotnet run