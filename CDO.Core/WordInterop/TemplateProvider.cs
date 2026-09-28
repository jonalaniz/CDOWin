namespace CDO.Core.WordInterop;

public static class TemplateProvider {
    private static readonly string _rootPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Templates");

    public static string GetTemplate(string templateName) {
        var path = Path.Combine(_rootPath, templateName);
        if (!File.Exists(path)) throw new FileNotFoundException("Template not found.", templateName);

        return path;
    }
}
