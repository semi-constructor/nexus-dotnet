using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using System.Management;

namespace NEXUS.Commands;

public class SysInfo : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("sysinfo", "Provides information about the system.")]
    public InteractionMessageProperties SysInfoCommand()
    {
        var os = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
        var framework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
        var processArch = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture;

        var driveInfo = new System.IO.DriveInfo(
            System.IO.Path.GetPathRoot(Environment.SystemDirectory)!
        );
        var totalSpace = driveInfo.TotalSize / (1024 * 1024 * 1024);
        var freeSpace = driveInfo.AvailableFreeSpace / (1024 * 1024 * 1024);
        var usedSpace = totalSpace - freeSpace;

        var gpuModel = new ManagementObjectSearcher("select * from Win32_VideoController")
            .Get().Cast<ManagementObject>()
            .FirstOrDefault()?["Name"]?.ToString() ?? "Unknown GPU";

        var cpuObj = new ManagementObjectSearcher("select * from Win32_Processor")
            .Get().Cast<ManagementObject>()
            .FirstOrDefault();

        var cpuModel = cpuObj?["Name"]?.ToString() ?? "Unknown CPU";
        var cpuCores = Environment.ProcessorCount;
        var cpuClock = cpuObj?["MaxClockSpeed"]?.ToString() ?? "Unknown";

        var ramBytes = new ManagementObjectSearcher("select * from Win32_ComputerSystem")
            .Get().Cast<ManagementObject>()
            .FirstOrDefault()?["TotalPhysicalMemory"];

        var ram = ramBytes != null
            ? (Convert.ToDouble(ramBytes) / (1024.0 * 1024 * 1024)).ToString("0.00") + " GB"
            : "Unknown";

        var embed = new EmbedProperties()
            .WithTitle("⚙️ System Information")
            .WithColor(new Color(0x5865F2))
            .WithTimestamp(DateTimeOffset.UtcNow)
            .WithFields(new[]
            {
                new EmbedFieldProperties()
                    .WithName("🖥️ Operating System")
                    .WithValue(os)
                    .WithInline(false),

                new EmbedFieldProperties()
                    .WithName("🔧 .NET Framework")
                    .WithValue(framework)
                    .WithInline(true),

                new EmbedFieldProperties()
                    .WithName("🏗️ Architecture")
                    .WithValue(processArch.ToString())
                    .WithInline(true),

                new EmbedFieldProperties()
                    .WithName("\u200b")
                    .WithValue("\u200b")
                    .WithInline(false),

                new EmbedFieldProperties()
                    .WithName("🧠 CPU")
                    .WithValue($"{cpuModel}\n{cpuCores} cores @ {cpuClock} MHz")
                    .WithInline(true),

                new EmbedFieldProperties()
                    .WithName("🎮 GPU")
                    .WithValue(gpuModel)
                    .WithInline(true),

                new EmbedFieldProperties()
                    .WithName("💾 RAM")
                    .WithValue(ram)
                    .WithInline(true),

                new EmbedFieldProperties()
                    .WithName("💿 Storage")
                    .WithValue($"Total: {totalSpace} GB\nUsed: {usedSpace} GB\nFree: {freeSpace} GB")
                    .WithInline(false),
            })
            .WithFooter(new EmbedFooterProperties().WithText("NEXUS • System Info"));

        return new InteractionMessageProperties()
            .WithEmbeds([embed]);
    }
}