using OllamaSharp;

namespace TestConsole;

public class Llama {
    private Uri uri;
    private OllamaApiClient ollama;

    public Llama() {
        uri = new Uri("http://10.0.1.35:11434");
        ollama = new OllamaApiClient(uri);
        ollama.SelectedModel = "deepseek-r1:7b";
    }

    public async Task UpdateModel() {
        Console.WriteLine("Fetching models...");
        var models = await ollama.ListLocalModelsAsync();

        int i = 0;
        var names = new List<string>();
        Console.WriteLine("Enter the model you wish to use and press enter:");

        foreach (var model in models) {
            i++;
            names.Add(model.Name);
            Console.WriteLine($"{i}: {model.Name}");
        }

        string userInput = Console.ReadLine();

        var inputSelection = int.Parse(userInput);
        var selection = names[inputSelection - 1];
        ollama.SelectedModel = selection;
        Console.WriteLine($"Selected: {selection}");
    }

    public async Task Chat() {

        var chat = new Chat(ollama);
        Console.WriteLine("Ollama Chatbot (type 'exit' to quit)");

        while (true) {
            Console.Write("You: ");
            string userInput = Console.ReadLine();

            if (userInput.ToLower() == "exit") { break; }

            Console.WriteLine("Thinking...");

            bool streamBegun = false;

            // Get streaming response from Ollama
            await foreach (var answerToken in chat.SendAsync(userInput)) {
                if (!streamBegun) {
                    Console.Write("Ollama: ");
                    streamBegun = true;
                }

                Console.Write(answerToken);
            }

            Console.WriteLine();
        }
    }
}
