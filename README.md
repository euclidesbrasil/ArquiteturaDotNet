# API Geral - Gestão de Crédito e Débitos

## 📌 Descrição

Esta API fornece recursos para gerenciar autenticação, transações e expor os dados via api de relatório de consolidação diário 

## 🛠 Tecnologias Utilizadas

- .NET 8
- PostgreSQL
- MongoDB
- RabbitMQ

## 🗉 Pré-requisitos

Clone este projeto usando a URL: [https://github.com/euclidesbrasil/AMBEV.git](https://github.com/euclidesbrasil/ArquiteturaDotNet.git)

Antes de baixar o projeto, certifique-se de ter instalado:

- **Visual Studio** (Versão utilizada: Microsoft Visual Studio Community 2022 - Versão 17.10.1, preferencialmente após a versão 17.8)
- **PostgreSQL** (Versão utilizada: 17.2-3) [Baixar aqui](https://www.enterprisedb.com/downloads/postgres-postgresql-downloads)
- **MongoDB Community** (Versão utilizada: 7.0.16) [Baixar aqui](https://www.mongodb.com/try/download/community-edition/releases)
- **RabbitMQ** (Versão utilizada: 4.0.5) [Baixar aqui](https://www.rabbitmq.com/docs/install-windows)

## 🚀 Configuração Antes da Execução

### 1. Configuração do PostgreSQL

No projeto **Ambev.General.Api**, abra o arquivo `appsettings.json` e ajuste a seção `DefaultConnection` com as credenciais do seu banco de dados local:

```json
"DefaultConnection": "Host=localhost;Port=5432;Database=ARQDESAFIODOTNET;Username=postgres;Password=admin"
```

### 2. Configuração do MongoDB

No mesmo arquivo `appsettings.json`, há uma seção `ConnectionString` que define a conexão com o MongoDB. Ajuste conforme necessário para o seu ambiente.

### 3. Executando o Projeto

Basta executar o projeto para iniciar a API. Na primeira execução, o banco de dados será criado automaticamente e os dados iniciais serão carregados. Poderá ser usado via Swagger;

Caso queira rodar via docker, abrea o "PowerShell do Desenvolvedor" refernte a raiz da solução e execute o comando:
```json
docker-compose up --build -d
```
Isso fará que o docker build a aplicação e suba as imagens necessárias.

ATENÇÃO! Em ambos os casos, há um Worker responsável por ler as mensagens enviadas via RabbitMQ para poder gerar a versão do relatório via MongoDB;

Localmente, você deve executar o exe manualmente, pelo visual studio (Depuprar nova insância sem inicializar) ou navegar até a pasta do proejto, apos efetuar o Rebuild da aplicação e executar o ArquiteturaDesafio.Worker.exe: src\ArquiteturaDesafio.Worker\bin\Debug\net8.0 ou em src\ArquiteturaDesafio.Worker\bin\Release\net8.0

Já no Docker, caso o serviço não seja iniciado automaticamente, inicar o mesmo.
## 🔐 Autenticação

Para utilizar os endpoints, é necessário obter um token de autenticação. Utilize as credenciais iniciais:

- **Usuário:** admin
- **Senha:** s3nh@