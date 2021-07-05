# CarRental

Faz uma simulação simples dos serviços de backend de uma locadora de veículos.

## Serviços

São 3 serviços distintos:
Vehicles: responsável pelo CRUD de veículos, marcas, modelos e categorias de veiculos
Person: responsável pelo CRUD de usuarios, clientes e operadores. Em uma aplicação expandida, esse serviço seria quebrado em três serviços com atribuições especificas
Reservation: responsável por gerenciar reservas, fazer o retorno dos veículos, gerar contrato.

##Tecnologias

Entity Framework foi usado por ser um framework que conheço e já usei. Nesse projeto, usou-se uma in-memory database, para facilitar o desenvolvimento.

xUnit Tests foi a escolha para os testes unitários. Sua escolha se deve a simplicidade de implementação

Swagger foi usado para gerar a documentação das APIs. É uma ferramenta que já usei em diferentes versões, que tem uma implementação rapida e tem bons resultados.

Ocelot foi a escolha para o API Gateway. Sua escolha é devido a ser leve, simples de implementar e pode ter o deploy no mesmo ambiente que os serviços

Microsoft Identity Framework foi usado para o gerenciamento de identidades, junto com JWTBearer para autenticação/autorização. Simples de implementar, bem integrado com uma aplicação .NET
