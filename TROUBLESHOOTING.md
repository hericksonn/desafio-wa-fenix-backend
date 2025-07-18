# Troubleshooting - Desafio WA Fenix Backend

## Problemas Comuns e Soluções

### 1. Erro de Globalização Invariante

**Erro:**
```
System.Globalization.CultureNotFoundException: Only the invariant culture is supported in globalization-invariant mode
```

**Causa:**
A configuração `<InvariantGlobalization>true</InvariantGlobalization>` no arquivo `API.csproj` está causando conflito com o Entity Framework Core.

**Solução:**
Remover a linha `<InvariantGlobalization>true</InvariantGlobalization>` do arquivo `API/API.csproj`.

### 2. Erro de Conexão com Banco de Dados

**Erro:**
```
SqlException: Cannot open database "desafiofdb" requested by the login
```

**Causa:**
- Banco de dados não existe
- Credenciais incorretas
- Servidor não acessível

**Soluções:**
1. Verificar se o banco `desafiofdb` existe no Azure SQL
2. Confirmar as credenciais na connection string
3. Verificar se o servidor está acessível
4. Aplicar migrations: `dotnet ef database update --project Infrastructure --startup-project API`

### 3. Erro de Certificado SSL

**Erro:**
```
The SSL connection could not be established
```

**Solução:**
- Em desenvolvimento, acesse `http://localhost:5000` em vez de `https://localhost:7000`
- Ou configure certificados de desenvolvimento: `dotnet dev-certs https --trust`

### 4. Erro de Dependências

**Erro:**
```
Package restore failed
```

**Solução:**
```bash
dotnet restore
dotnet clean
dotnet build
```

### 5. Erro de Migration

**Erro:**
```
No database provider has been configured for this DbContext
```

**Solução:**
1. Verificar se o `ApplicationDbContext` está registrado no `Program.cs`
2. Confirmar se a connection string está configurada
3. Verificar se o projeto Infrastructure está referenciado

### 6. Erro de Swagger

**Erro:**
```
Swagger page not found
```

**Solução:**
- Acesse a URL correta: `https://localhost:7000` (não `/swagger`)
- Verifique se o Swagger está configurado no `Program.cs`

## Comandos Úteis

### Verificar Status da Aplicação
```bash
# Verificar se está rodando
Get-Process -Name "API" -ErrorAction SilentlyContinue

# Parar aplicação
Stop-Process -Name "API" -Force
```

### Testar Conexão com Banco
```bash
# Aplicar migrations
dotnet ef database update --project Infrastructure --startup-project API

# Verificar migrations
dotnet ef migrations list --project Infrastructure --startup-project API
```

### Limpar e Reconstruir
```bash
dotnet clean
dotnet restore
dotnet build
dotnet test
```

## Logs e Debug

### Habilitar Logs Detalhados
Adicione no `appsettings.Development.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

### Verificar Logs da Aplicação
Os logs aparecem no console quando a aplicação está rodando.

## Contatos para Suporte

- **Problemas de Banco**: Verificar Azure SQL Database
- **Problemas de Rede**: Verificar conectividade com Azure
- **Problemas de Código**: Verificar logs e configurações 