# API Geral - Gestão de Crédito e Débitos

## 📌 Descrição

Esta API fornece recursos para gerenciar autenticação, transações e expor os dados via api de relatório de consolidação diário 

## 🛠 Tecnologias Utilizadas

- .NET 8
- PostgreSQL
- MongoDB
- RabbitMQ

## 🗉 Pré-requisitos

Clone este projeto usando a URL: [https://github.com/euclidesbrasil/ArquiteturaDotNet.git](https://github.com/euclidesbrasil/ArquiteturaDotNet.git)

Antes de baixar o projeto, certifique-se de ter instalado:

- **Visual Studio** (Versão utilizada: Microsoft Visual Studio Community 2022 - Versão 17.10.1, preferencialmente após a versão 17.8)
- **PostgreSQL** (Versão utilizada: 17.2-3) [Baixar aqui](https://www.enterprisedb.com/downloads/postgres-postgresql-downloads)
- **MongoDB Community** (Versão utilizada: 7.0.16) [Baixar aqui](https://www.mongodb.com/try/download/community-edition/releases)
- **RabbitMQ** (Versão utilizada: 4.0.5) [Baixar aqui](https://www.rabbitmq.com/docs/install-windows)

## 🚀 Configuração Antes da Execução

### 1. Configuração do PostgreSQL

No projeto **ArquiteturaDesafio.General.Api**, abra o arquivo `appsettings.json` e ajuste a seção `DefaultConnection` com as credenciais do seu banco de dados local:

```json
"DefaultConnection": "Host=localhost;Port=5432;Database=ARQDESAFIODOTNET;Username=postgres;Password=admin"
```

### 2. Configuração do MongoDB

No mesmo arquivo `appsettings.json`, há uma seção `ConnectionString` que define a conexão com o MongoDB. Ajuste conforme necessário para o seu ambiente.

### 3. Executando o Projeto

Basta executar o projeto para iniciar a API. Na primeira execução, o banco de dados será criado automaticamente e os dados iniciais serão carregados. Poderá ser usado via Swagger;

Caso queira rodar via docker, abra o "PowerShell do Desenvolvedor" referente a raiz da solução e execute o comando:
```json
docker-compose up --build -d
```
Isso fará que o docker build a aplicação e suba as imagens necessárias.

ATENÇÃO! Em ambos os casos, há um Worker responsável por ler as mensagens enviadas via RabbitMQ para poder gerar a versão do relatório via MongoDB;

Localmente, você deve executar o exe manualmente, pelo visual studio (Depurar nova insância sem inicializar) ou navegar até a pasta do proejto, apos efetuar o Rebuild da aplicação e executar o ArquiteturaDesafio.Worker.exe: src\ArquiteturaDesafio.Worker\bin\Debug\net8.0 ou em src\ArquiteturaDesafio.Worker\bin\Release\net8.0

Já no Docker, caso o serviço não seja iniciado automaticamente, inicar o mesmo.
## 🔐 Autenticação

Para utilizar os endpoints, é necessário obter um token de autenticação. Utilize as credenciais iniciais:

- **Usuário:** admin
- **Senha:** s3nh@


## 📌 Endpoints

### 🔹 Autenticação
- **POST /auth/login** → Autentica um usuário e retorna um token JWT.

### 🔹 Relatórios
- **GET /Report/Daily/Mongodb?date=YYYY-MM-DD** → Retorna relatório diário do MongoDB.
- **GET /Report/Daily/Postgres?date=YYYY-MM-DD** → Retorna relatório diário do PostgreSQL.

### 🔹 Transações
- **POST /Transaction** → Cria uma nova transação.
- **PUT /Transaction?id=UUID** → Atualiza uma transação existente.
- **DELETE /Transaction?id=UUID** → Remove uma transação.
- **GET /Transaction** → Retorna uma lista paginada de transações.
- **GET /Transaction/{id}** → Retorna detalhes de uma transação específica.

### 🔹 Usuários
- **POST /Users** → Cria um novo usuário.
- **PUT /Users?id=UUID** → Atualiza um usuário existente.
- **GET /Users** → Retorna uma lista paginada de usuários.
- **GET /Users/{id}** → Retorna detalhes de um usuário específico.