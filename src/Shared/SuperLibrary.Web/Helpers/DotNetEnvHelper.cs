namespace SuperLibrary.Web.Helpers;

public static class DotNetEnvHelper
{
    public static string GetEnvironmentVariableOrThrow(string key) 
        => Environment.GetEnvironmentVariable(key) 
           ?? throw new InvalidOperationException($"Environment variable {key} is not set");

    public static string? GetEnvironmentVariableOrDefault(string key) 
        => Environment.GetEnvironmentVariable(key);
}