# BlogAspNet - Performance
Projeto para revisão de conceito e aprendizado,
continuação do projeto [BlogAspNet](https://github.com/thiagokj/BlogAspNet_Validations)

Alguns exemplos sobre Ambiente e Documentação.

## Debug VS Release
Debug | Modo para depurar a aplicação em busca de comportamento natural e casos de erro.
Nesse modo, a compilação gera arquivos adicionais (ex: *.pdb) e otimiza o programa para inspeção

Release | Versão testada e aprovada para ser publicada para uso em produção.
Comprime e otimiza o código para aproveitar da melhor forma os recursos.

Para verificar qual ambiente está sendo executada a aplicação, crie chaves no appsettings e adicione uma
chamada no HomeController.

```Csharp

// Adicionado valor "Prod" no arquivo de Release...
{
  "Environment": "Dev",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}

public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult Get(
            [FromServices] IConfiguration config)
        {
            var env = config.GetValue<string>("Environment");
            return Ok(new
            {
                environment = env
            });
        }
    }
```

Para compilar em modo Release, utilize o comando **dotnet build -c Release**

```Csharp

// Tambem pode ser especificado a solution a ser compilada

dotnet build BlogAspNet_Environment.sln -c Release

```

Os arquivos da versão de release ficam dentro da pasta **bin**.

## Versão do Ambiente
Para alterar entre ambientes, pode ser adicionada verificação antes do app.Run()

```Csharp
if (app.Environment.IsDevelopment())
    Console.WriteLine("Ambiente de Desenvolvimento.");
```

E então executar os builders específicos para Desenvolvimento e para Produção.

## Habilitar HTTPS
Habilite o Https antes da autenticação para redirecionar requisições para uma conexão encriptada
e segura.

**app.UseHttpsRedirection()**

## ConnectionStrings
As conexões devem ser adicionadas ao appsettings. O padrão de mercado é utilizar a palavra reservada
ConnectionString. Ela cria uma section que armazena todas as conexões e possui integração com a Azure.

```Csharp
{
  "Environment": "Prod",
  "ConnectionStrings": {
    "DefaultConnection": "MinhaConexao"
  },...
```

Faça a alteração no DataContext para que o construtor receba a conexão derivada de DbContext

```Csharp
...
public class BlogDataContext : DbContext
{
    public BlogDataContext(DbContextOptions<BlogDataContext> options) : base(options) { }
...

// Remova o método protected override void OnConfiguring(DbContextOptionsBuilder options)
```

Agora recupere a conexao no ConfigureServices do Builder.

```Csharp
void ConfigureServices(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration
        .GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<BlogDataContext>(options =>
    options.UseSqlServer(connectionString));
...
}
```

## Documentação da API

A forma para informar como utilizar a API pode ser feita usando uma OpenAPI. Toda documentação
pode ser gerada via Swagger. Instale o pacote **dotnet add package Swashbuckle.AspNetCore**.

Agora altere o builder para habilitar o Swagger.

```Csharp
...
var builder = WebApplication.CreateBuilder(args);
ConfigureAuthentication(builder);
ConfigureMVC(builder);
ConfigureServices(builder);

// Adiciona o Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Habilita o Swagger. 
// O mesmo tambem pode ser habilitado para produção, sempre se atentando para segurança das informações. 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

Para acessar a documentação gerada, utilize o link https://enderecodaapi:porta/swagger/

