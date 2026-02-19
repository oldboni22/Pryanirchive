namespace FileService.Infrastructure.Blob;

public class UploadOptions
{
    private const string ConfigurationSection = "Upload";
    
    public const string FileSection = $"{ConfigurationSection}:File";

    public const string FileOptionsKey = "FileOptions";
    
    public const string AvatarSection = $"{ConfigurationSection}:Avatar";

    public const string AvatarOptionsKey = "AvatarOptions";

    public int MaxMegabyteFileSize { get; init; } = 5000;
}
