using NetCord.Services.ApplicationCommands;

namespace NEXUS.Commands;

public class PingModule : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("ping", "Responds with Pong!")]
    public string Ping() => "Pong!";
}