# Controle de Gastos Residenciais


O projeto permite o **cadastro de pessoas**, o **registro de receitas e despesas** e a **consulta consolidada dos gastos**, apresentando o saldo individual de cada pessoa e o saldo geral da residência.

---

#  Funcionalidades

##  Cadastro de Pessoas

- ✅ Criar pessoas
- ✅ Listar pessoas
- ✅ Remover pessoas
- ✅ Exclusão em cascata das transações relacionadas

Cada pessoa possui:

| Campo | Descrição |
|--------|-----------|
| ID | Gerado automaticamente |
| Nome | Nome da pessoa |
| Idade | Idade da pessoa |

---

##  Cadastro de Transações

É possível registrar movimentações financeiras vinculadas a uma pessoa.

Cada transação possui:

| Campo | Descrição |
|--------|-----------|
| ID | Gerado automaticamente |
| Descrição | Descrição da movimentação |
| Valor | Valor monetário |
| Tipo | Receita ou Despesa |
| Pessoa | Pessoa responsável pela movimentação |

> **Observação:** edição e exclusão de transações não fazem parte do escopo do desafio.

---

## Consulta de Totais

O sistema apresenta:

- Total de receitas por pessoa;
- Total de despesas por pessoa;
- Saldo individual (Receitas − Despesas);
- Total geral de receitas;
- Total geral de despesas;
- Saldo consolidado da residência.

---

# 📌 Regras 

### Menores de idade

Pessoas com menos de **18 anos** **não podem possuir receitas cadastradas**.

Caso seja feita uma tentativa de cadastrar uma receita para um menor de idade, a API retorna uma mensagem explicando a violação da regra de negócio.

---

# 🛠 Tecnologias Utilizadas

| Backend | Frontend | Hospedagem |
|---------|----------|------------|
| .NET 10 | React | Render |
| ASP.NET Core Web API | TypeScript | Vercel |
| C# | Vite | Docker |
| Controllers | Tailwind CSS | |

---

# 🏛 Arquitetura

O backend foi desenvolvido seguindo uma arquitetura em camadas, buscando separação de responsabilidades e facilidade de manutenção.

```
Controllers
      │
      ▼
   Services
      │
      ▼
Repositories
      │
      ▼
Database
```

Cada camada possui uma responsabilidade específica:

| Camada | Responsabilidade |
|---------|------------------|
| Controllers | Recebem as requisições HTTP |
| Services | Implementam as regras de negócio |
| Repositories | Acesso aos dados |
| Database | Persistência das informações |

---

# 🌐 Aplicação Publicada

| Serviço | Link |
|----------|------|
| Frontend | https://gastos-residenciais-six.vercel.app/ |
| Backend | https://gastos-residenciais-e93m.onrender.com |

---

## ⚠️ Observação sobre a hospedagem

O backend está hospedado no **Render (plano gratuito)**.

Por esse motivo, após alguns minutos sem utilização o serviço entra em estado de hibernação.

Na primeira requisição pode ocorrer:

- demora de **30 a 60 segundos**;
- uma mensagem de erro temporária na interface.

Após o serviço ser reativado, as próximas requisições funcionam normalmente.

> Esse comportamento é uma limitação da hospedagem gratuita e não da aplicação.

Para uma experiência completa, recomenda-se executar o projeto localmente.

---

# Executando o projeto

## Backend

```bash
git clone <url-do-repositório>

cd backend

dotnet restore

dotnet run
```

---

## Frontend

```bash
cd frontend

npm install

npm run dev
```

A aplicação estará disponível em:

```
http://localhost:5173
```

---

# Resumo das Funcionalidades

| Funcionalidade | Status |
|---------------|:------:|
| Cadastro de pessoas | ✅ |
| Exclusão de pessoas | ✅ |
| Cadastro de receitas | ✅ |
| Cadastro de despesas | ✅ |
| Listagem de transações | ✅ |
| Consulta de totais | ✅ |
| Exclusão em cascata | ✅ |
| Validação para menores de idade | ✅ |

---
