# Comandos de Migration - Entity Framework Core

## Pré-requisitos
Certifique-se de que o Entity Framework Tools está instalado:
```bash
dotnet tool install --global dotnet-ef
```

## Comandos Principais

### 1. Criar Migration Inicial
```bash
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API
```

### 2. Aplicar Migrations no Banco de Dados
```bash
dotnet ef database update --project Infrastructure --startup-project API
```

### 3. Remover Migration (se necessário)
```bash
dotnet ef migrations remove --project Infrastructure --startup-project API
```

### 4. Gerar Script SQL das Migrations
```bash
dotnet ef migrations script --project Infrastructure --startup-project API
```

### 5. Verificar Status das Migrations
```bash
dotnet ef migrations list --project Infrastructure --startup-project API
```

## Exemplo de Uso Completo

1. **Criar a migration inicial:**
```bash
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API
```

2. **Aplicar no banco de dados:**
```bash
dotnet ef database update --project Infrastructure --startup-project API
```

3. **Verificar se foi aplicada:**
```bash
dotnet ef migrations list --project Infrastructure --startup-project API
```

## Notas Importantes

- O projeto `Infrastructure` contém o `DbContext`
- O projeto `API` é o projeto de inicialização
- As migrations serão criadas na pasta `Infrastructure/Migrations/`
- O banco de dados será criado automaticamente na primeira execução da aplicação

## Troubleshooting

Se encontrar erros de conexão:
1. Verifique se a connection string está correta
2. Certifique-se de que o servidor Azure SQL está acessível
3. Verifique se as credenciais estão corretas
4. Teste a conexão com o SQL Server Management Studio 