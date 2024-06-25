using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_template.Controllers;

[ApiController]
[Route("[controller]")]
public class ExampleController : ControllerBase
{
    private readonly ILogger<ExampleController> _logger;
    private readonly HttpClient _client;

    public ExampleController(ILogger<ExampleController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _client = httpClient;
    }

    [HttpGet("/hello")]
    [AllowAnonymous]
    public string HelloWorld()
    {
        _logger.LogInformation("Hello world");
        return "Hello world";
    }
    
    [HttpGet("/secure")]
    public string SecuredEndpoint()
    {
        return HelloWorld();
    }
}