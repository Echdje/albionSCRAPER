using System.Reflection;
using System.Text.Json;
using albionSCRAPER.Models;

namespace albionSCRAPER;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
        DataInput();
        
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }

    private void dir()
    {
        string path = "/Users/adrianstanisz/RiderProjects/albionSCRAPER/albionSCRAPER/Data/Testowy.json";

        if (File.Exists(path))
        {
            Console.WriteLine("plik istnieje");
        }
    }
    public static void DataInput()
    {
        string filename = "Testowy.json";
        string destinationPath = Path.Combine(FileSystem.AppDataDirectory, filename);

        // Jeżeli plik jeszcze nie istnieje w AppDataDirectory — kopiujemy go z zasobu
        if (!File.Exists(destinationPath))
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "albionSCRAPER.Data.Testowy.json"; // ← pełna ścieżka z przestrzenią nazw!

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                Console.WriteLine(" Nie można odnaleźć zasobu embedded: " + resourceName);
                return;
            }

            using var fileStream = File.Create(destinationPath);
            stream.CopyTo(fileStream);

            Console.WriteLine($"✅ Skopiowano plik do: {destinationPath}");
        }

        // Teraz odczytujemy z AppDataDirectory
        try
        {
            string json = File.ReadAllText(destinationPath);
            var items = JsonSerializer.Deserialize<List<Item>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (items is null || items.Count == 0)
            {
                Console.WriteLine("Brak danych lub błędna struktura JSON.");
                return;
            }

            foreach (var item in items)
            {
                Console.WriteLine($"🧾 Nazwa: {item.LocalizedNames["PL-PL"]}");
                Console.WriteLine($"🔗 UniqueName: {item.UniqueName}");
                Console.WriteLine($"⭐ Tier: {item.Tier}, 📦 Kategoria: {item.Category}, 🏰 Pochodzenie: {item.Faction}");
                Console.WriteLine("──────────────");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd: " + ex.Message);
        }
    }
}