# WebScrapperEmpregos

Este é um projeto de um simples aplicativo de console usando ASP.NET Core 8.0, para busca informações de vagas no site [empregos.com](https://www.empregos.com.br/).

## Como Rodar

Certifique-se de ter o SDK do .NET 8.0 instalado. Você pode baixá-lo em [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/8.0).

```bash
    git clone https://github.com/RyanFurt12/DotNetWebScrapperEmpregos.git
    cd DotNetWebScrapperEmpregos
    
    dotnet run
```
    
## Resposta

A aplicação executará no seu console, e pedirá:
```bash
    Empregos.com
    O que esta procurando?
```
Assim, você passa qual vaga deseja, e ao enviar, o script irá rodar, assim ele criará um txt com o nome que vc pesquisou, e anotara todos os resultados encontratos no arquivo