using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace NEXUS.Commands;

public class GuildInfoModule : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("guildinfo", "Provides information about the guild.")]

    public InteractionMessageProperties GuildInfo()
    {
        var guild = Context.Guild;

        if (guild == null)
            return new InteractionMessageProperties().WithContent("This command can only be used in a guild.");

        var embed = new EmbedProperties()
        .WithTitle(guild.Name)
        .WithColor(new Color(255, 0, 0))
        .AddFields(
            new EmbedFieldProperties()
        );

        return new InteractionMessageProperties().AddEmbeds(embed);
    }
}