# Desafio WA Fenix Backend

API REST em .NET 8 para gerenciamento de livros, autores e gêneros literários.

## 📋 Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) ou [Azure SQL Database](https://azure.microsoft.com/en-us/services/sql-database/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

## 🚀 Configuração Inicial

### 1. Clone o repositório
```bash
git clone <url-do-repositorio>
cd desafio-wa-fenix-backend
```

### 2. Restaurar dependências
```bash
dotnet restore
```

### 3. Configurar connection string

Edite o arquivo `API/appsettings.json` ou `API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DesafioWAFenix;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

Para Azure SQL Database:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server.database.windows.net;Database=DesafioWAFenix;User Id=your-username;Password=your-password;TrustServerCertificate=true;"
  }
}
```

## 🗄️ Migrações do Entity Framework

### Gerar nova migração
```bash
dotnet ef migrations add NomeDaMigracao --project Infrastructure --startup-project API
```

### Aplicar migrações no banco
```bash
dotnet ef database update --project Infrastructure --startup-project API
```

### Remover última migração (se não aplicada)
```bash
dotnet ef migrations remove --project Infrastructure --startup-project API
```

### Listar migrações
```bash
dotnet ef migrations list --project Infrastructure --startup-project API
```

## 🏃‍♂️ Executando o Projeto

### Desenvolvimento
```bash
dotnet run --project API
```

### Produção
```bash
dotnet run --project API --environment Production
```

### Build para produção
```bash
dotnet build --configuration Release
```

## 📚 Swagger/OpenAPI

Após executar o projeto, acesse a documentação Swagger:

- **URL Principal**: http://localhost:5013
- **Swagger UI**: http://localhost:5013/swagger
- **OpenAPI JSON**: http://localhost:5013/swagger/v1/swagger.json

### Endpoints disponíveis:

#### Gêneros
- `GET /api/v1/generos` - Listar todos os gêneros
- `GET /api/v1/generos/{id}` - Buscar gênero por ID
- `POST /api/v1/generos` - Criar novo gênero
- `PUT /api/v1/generos/{id}` - Atualizar gênero
- `DELETE /api/v1/generos/{id}` - Excluir gênero

#### Autores
- `GET /api/v1/autores` - Listar todos os autores
- `GET /api/v1/autores/{id}` - Buscar autor por ID
- `POST /api/v1/autores` - Criar novo autor
- `PUT /api/v1/autores/{id}` - Atualizar autor
- `DELETE /api/v1/autores/{id}` - Excluir autor

#### Livros
- `GET /api/v1/livros` - Listar todos os livros
- `GET /api/v1/livros/{id}` - Buscar livro por ID
- `POST /api/v1/livros` - Criar novo livro
- `PUT /api/v1/livros/{id}` - Atualizar livro
- `DELETE /api/v1/livros/{id}` - Excluir livro

## 🧪 Testes

### Executar todos os testes
```bash
dotnet test
```

### Executar testes com cobertura
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## ☁️ Deploy no Azure

### 1. Azure SQL Database

1. Crie um Azure SQL Database no portal Azure
2. Configure firewall rules para permitir conexões
3. Obtenha a connection string no formato:
   ```
   Server=your-server.database.windows.net;Database=DesafioWAFenix;User Id=your-username;Password=your-password;TrustServerCertificate=true;
   ```

### 2. Azure App Service

#### Opção A: Deploy via Azure CLI

1. Instale Azure CLI
2. Faça login:
   ```bash
   az login
   ```

3. Crie um App Service:
   ```bash
   az group create --name desafio-wa-fenix-rg --location brazilsouth
   az appservice plan create --name desafio-wa-fenix-plan --resource-group desafio-wa-fenix-rg --sku B1
   az webapp create --name desafio-wa-fenix-api --resource-group desafio-wa-fenix-rg --plan desafio-wa-fenix-plan --runtime "DOTNETCORE:8.0"
   ```

4. Configure a connection string:
   ```bash
   az webapp config connection-string set --resource-group desafio-wa-fenix-rg --name desafio-wa-fenix-api --settings DefaultConnection="Server=your-server.database.windows.net;Database=DesafioWAFenix;User Id=your-username;Password=your-password;TrustServerCertificate=true;"
   ```

5. Deploy o código:
   ```bash
   dotnet publish -c Release
   az webapp deployment source config-zip --resource-group desafio-wa-fenix-rg --name desafio-wa-fenix-api --src ./API/bin/Release/net8.0/publish/API.zip
   ```

#### Opção B: Deploy via GitHub Actions

1. Crie um arquivo `.github/workflows/deploy.yml`:

```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore --configuration Release
    
    - name: Test
      run: dotnet test --no-build --verbosity normal
    
    - name: Publish
      run: dotnet publish API/API.csproj -c Release -o ${{env.DOTNET_ROOT}}/myapp
    
    - name: Deploy to Azure Web App
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'desafio-wa-fenix-api'
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ${{env.DOTNET_ROOT}}/myapp
```

2. Configure secrets no GitHub:
   - `AZURE_WEBAPP_PUBLISH_PROFILE`: Profile de publicação do Azure App Service

### 3. Configurações de Produção

No Azure App Service, configure as seguintes variáveis de ambiente:

- `ASPNETCORE_ENVIRONMENT`: Production
- `ConnectionStrings__DefaultConnection`: Sua connection string do Azure SQL

### 4. Aplicar Migrações no Azure

Após o deploy, execute as migrações:

```bash
# Via Azure CLI
az webapp ssh --name desafio-wa-fenix-api --resource-group desafio-wa-fenix-rg

# No terminal do App Service
dotnet ef database update --project Infrastructure --startup-project API
```

## 📁 Estrutura do Projeto

```
desafio-wa-fenix-backend/
├── API/                    # Camada de apresentação
│   ├── Controllers/        # Controllers da API
│   ├── Program.cs          # Configuração da aplicação
│   └── appsettings.json    # Configurações
├── Application/            # Camada de aplicação
│   ├── Services/           # Serviços de negócio
│   ├── DTOs/              # Data Transfer Objects
│   └── Common/            # Classes comuns
├── Domain/                # Camada de domínio
│   └── Entities/          # Entidades do domínio
├── Infrastructure/        # Camada de infraestrutura
│   ├── Data/              # Contexto do EF Core
│   └── Migrations/        # Migrações do banco
└── Tests/                 # Testes unitários
```

## 🔧 Tecnologias Utilizadas

- **.NET 8**
- **Entity Framework Core 8**
- **AutoMapper**
- **Swagger/OpenAPI**
- **API Versioning**
- **xUnit** (testes)
- **Azure SQL Database**

## 📝 Exemplos de Uso

### Criar um gênero
```bash
curl -X POST "http://localhost:5013/api/v1/generos" \
  -H "Content-Type: application/json" \
  -d '{"nome": "Ficção Científica"}'
```

### Criar um autor
```bash
curl -X POST "http://localhost:5013/api/v1/autores" \
  -H "Content-Type: application/json" \
  -d '{"nome": "Isaac Asimov"}'
```

### Criar um livro
```bash
curl -X POST "http://localhost:5013/api/v1/livros" \
  -H "Content-Type: application/json" \
  -d '{"titulo": "Fundação", "generoId": 1, "autorId": 1}'
```

## 🐛 Troubleshooting

### Erro de conexão com banco
- Verifique se o SQL Server está rodando
- Confirme a connection string
- Teste a conexão: `dotnet ef database update`

### Swagger não carrega
- Acesse http://localhost:5013 (sem /swagger)
- Verifique se o ambiente é Development

### Erro de migração
- Delete a pasta Migrations e gere novamente
- Verifique se o banco existe e está acessível

## 📞 Suporte

Para dúvidas ou problemas, abra uma issue no repositório. 