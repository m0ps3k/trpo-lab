using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Runtime.Serialization;
using System.ServiceModel;
using JsonParsing.Models;
using System.IO;
using System.Text;

namespace JsonParsing.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public string labelContent = "label";
    [ObservableProperty]
    public string labelContent2 = "label";
    [ObservableProperty]
    public string textBoxContent = "textbox";
    [ObservableProperty]
    public string textBoxContent2 = "textbox";

    [RelayCommand]
    public void ButtonPress()
    {
        Item item1 = new Item(10, "Стол", 4500);
        LabelContent = $"{item1.Code} {item1.Name} {item1.Price}"; //первый итем

        System.Runtime.Serialization.Json.DataContractJsonSerializer ps = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(Item));
        Stream stream = new MemoryStream();
        ps.WriteObject(stream, item1);

        stream.Position = 0;
        StreamReader streamReader = new StreamReader(stream);
        string read = streamReader.ReadToEnd();

        TextBoxContent = read; //прочитанный json

        string read1 = "{\"Code\":11,\"Name\":\"Стул\",\"Price\":500}";
        Stream memory = new MemoryStream(Encoding.UTF8.GetBytes(read1));

        TextBoxContent2 = $"{memory.Length}"; //размер потока


        Item item2 = (Item)ps.ReadObject(memory)!;
        LabelContent2 = $"{item2.Code} {item2.Name} {item2.Price}"; //второй лейбл прочитанный json
    }
}

