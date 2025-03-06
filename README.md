# Mercadoria Estoque
Um CRUD simples de cadastro de mercadorias e controle de estoque

## Tecnologias Utilizadas
Este projeto foi desenvolvido utilizando as seguintes tecnologias e práticas:

### **Arquitetura e Padrões**
- **Domain-Driven Design (DDD)**: Para organizar o código em camadas e garantir que o domínio do negócio seja o foco principal.
- **Data Transfer Objects (DTOs)**: Para transferir dados entre as camadas da aplicação de forma segura e eficiente.

### **Backend**
- **.NET 7**: A versão mais recente do framework .NET para desenvolvimento de aplicações modernas e de alto desempenho.
- **ASP.NET Core**: Para construir a API RESTful que expõe os endpoints do sistema.
- **Dapper**: Um micro ORM leve e rápido para acesso ao banco de dados, com suporte a consultas SQL mapeadas para objetos.
- **MySQL**: Banco de dados relacional utilizado para armazenar os dados da aplicação.

### **Testes**
- **xUnit**: Framework de testes unitários para garantir a qualidade do código.
- **Moq**: Biblioteca para criar mocks em testes unitários, permitindo simular comportamentos de dependências.
- **FluentAssertions**: Para escrever asserções mais legíveis e expressivas nos testes.

### **Ferramentas e Bibliotecas**
- **Swagger**: Para documentação e teste da API diretamente no navegador.
- **Dependency Injection**: Padrão nativo do ASP.NET Core para gerenciar dependências e promover um código mais testável e modular.
