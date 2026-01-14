namespace CDO.Core.WordInterop;

public sealed class TemplateProvider : ITemplateProvider {
    private const string SharedDrive = @"Z:\Templates";
    private readonly string _fallbackPath;

    public TemplateProvider() {
        _fallbackPath = Path.Combine(
            AppContext.BaseDirectory, "Assets", "Templates"
        );
    }

    public string GetTemplate(string templateName) {
        var shared = Path.Combine(SharedDrive, templateName);
        if (File.Exists(shared)) return shared;

        var fallback = Path.Combine(_fallbackPath, templateName);
        if (!File.Exists(fallback))
            throw new FileNotFoundException("Template not found.", templateName);

        return fallback;
    }
}
