namespace WebAPITodoList.Settings;
public class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpireMinutes { get; set; }
}
public class RateLimitSettings
{
    public int LIMIT { get; set; } = 100; // Numero massimo di richieste consentite
    public int PERIOD { get; set; } = 10; // Periodo in secondi per il conteggio delle richieste
}
