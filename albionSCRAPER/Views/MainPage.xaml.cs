
using albionSCRAPER.ViewModels;

namespace albionSCRAPER;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }

 
    private void dir()
    {
        string path = "/Users/adrianstanisz/RiderProjects/albionSCRAPER/albionSCRAPER/Data/Testowy.json";

        if (File.Exists(path))
        {
            Console.WriteLine("plik istnieje");
        }
    }
}