# 🚀 Quick Start - Desafio WA Fenix API

## ⚡ Acesso Rápido

### **URLs da API:**

#### **HTTP (Funcionando):**
```
http://localhost:5013
```

#### **HTTPS (Se configurado):**
```
https://localhost:7229
```

## 🔧 Como Executar

### **1. Executar em HTTP (Recomendado para desenvolvimento):**
```bash
dotnet run --project API --launch-profile http
```

### **2. Executar em HTTPS:**
```bash
dotnet run --project API --launch-profile https
```

### **3. Executar sem perfil específico:**
```bash
dotnet run --project API
```

## 📍 URLs Corretas

### **Swagger UI (Documentação):**
- **HTTP**: `http://localhost:5013`
- **HTTPS**: `https://localhost:7229` (se disponível)

### **Endpoints da API:**
- **HTTP**: `http://localhost:5013/api/v1/[controller]`
- **HTTPS**: `https://localhost:7229/api/v1/[controller]`

## 🎯 Exemplos de Uso

### **Acessar Swagger:**
```
http://localhost:5013
```

### **Testar Endpoints:**
```bash
# Listar gêneros
curl http://localhost:5013/api/v1/generos

# Criar gênero
curl -X POST http://localhost:5013/api/v1/generos \
  -H "Content-Type: application/json" \
  -d '{"nome": "Ficção Científica"}'
```

## ⚠️ Solução de Problemas

### **Erro 404 no Swagger:**
- Use `http://localhost:5013` (não `/swagger`)
- O Swagger está configurado na raiz da aplicação

### **Erro de Certificado HTTPS:**
- Use HTTP para desenvolvimento: `http://localhost:5013`
- Ou configure certificados: `dotnet dev-certs https --trust`

### **Aplicação não inicia:**
```bash
dotnet clean
dotnet restore
dotnet build
dotnet run --project API
```

## 📊 Status da Aplicação

- ✅ **Banco de dados**: Criado automaticamente
- ✅ **Tabelas**: Autores, Generos, Livros
- ✅ **API**: Funcionando em HTTP
- ✅ **Swagger**: Disponível na raiz
- ✅ **Testes**: 7/7 passando

## 🎉 Pronto para Usar!

Acesse `http://localhost:5013` e comece a usar a API! 