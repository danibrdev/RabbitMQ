#region

using Microsoft.AspNetCore.Mvc;
using RabbitMq.Domain.DTOs.InputModels;
using RabbitMq.Domain.Interfaces.Services;

#endregion

namespace RabbitMq.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly IRabbitMqService _rabbitMqService;

    public HomeController(IRabbitMqService rabbitMqService)
        => _rabbitMqService = rabbitMqService;

    [HttpPost]
    public async Task<IActionResult> SendMessage()
    {
        var mensagem = new RabbitMqMessageInputModel()
        {
            Nome = "Teste", Mensagem = "Mensgem teste"
        };

        await _rabbitMqService.SendMessageAsync(mensagem);

        return Ok(); 
    }
}