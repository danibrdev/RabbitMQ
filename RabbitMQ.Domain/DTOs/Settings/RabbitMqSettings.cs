﻿namespace RabbitMq.Domain.DTOs.Settings;

public class RabbitMqSettings
{
    public string? HostName { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Environment { get; set; }
    public int Port { get; set; }
}