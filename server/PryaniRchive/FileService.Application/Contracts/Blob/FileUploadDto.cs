namespace FileService.Application.Contracts.Blob;

public record FileUploadDto(string Url, Dictionary<string,string> FormData);