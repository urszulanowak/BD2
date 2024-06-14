using System;
using System.Windows.Forms;
using System.Collections.Generic;

public class XmlProcessorForm : Form
{
    private readonly XmlProcessor _xmlProcessor;
    private TextBox _nameTextBox;
    private TextBox _contentTextBox;
    private ListBox _documentListBox;

    public XmlProcessorForm()
    {
        _xmlProcessor = new XmlProcessor();

        InitializeComponents();
        LoadDocumentList();
    }

    private void InitializeComponents()
    {
        // Tworzenie kontrolek
        Label nameLabel = new Label
        {
            Text = "Nazwa dokumentu:"
        };

        _nameTextBox = new TextBox();

        Label contentLabel = new Label
        {
            Text = "Zawartość dokumentu:"
        };

        _contentTextBox = new TextBox
        {
            Multiline = true,
            Height = 100
        };

        Button saveButton = new Button
        {
            Text = "Zapisz dokument"
        };
        saveButton.Click += SaveButton_Click;

        Button deleteButton = new Button
        {
            Text = "Usuń dokument"
        };
        deleteButton.Click += DeleteButton_Click;

        _documentListBox = new ListBox
        {
            Width = 200,
            Height = 300
        };

        // Tworzenie panelu dla kontrolek
        FlowLayoutPanel panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            Padding = new Padding(10)
        };

        panel.Controls.AddRange(new Control[] { nameLabel, _nameTextBox, contentLabel, _contentTextBox, saveButton, deleteButton });

        // Tworzenie SplitContainer i dodanie paneli
        SplitContainer splitContainer = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            SplitterDistance = 300
        };

        splitContainer.Panel1.Controls.Add(panel);
        splitContainer.Panel2.Controls.Add(_documentListBox);

        Controls.Add(splitContainer);

        // Ustawienie właściwości okna
        Text = "Przetwarzanie dokumentów XML";
        ClientSize = new System.Drawing.Size(600, 300); // Zwiększenie rozmiaru okna
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = true; // Możesz ustawić na false, jeśli nie chcesz przycisku minimalizacji

        // Ustawienie na środku ekranu
        StartPosition = FormStartPosition.CenterScreen;
    }

    private void LoadDocumentList()
    {
        _documentListBox.Items.Clear();
        List<string> documentNames = _xmlProcessor.GetXmlDocumentNames();
        _documentListBox.Items.AddRange(documentNames.ToArray());
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        try
        {
            string name = _nameTextBox.Text;
            string content = _contentTextBox.Text;
            _xmlProcessor.SaveXmlDocument(name, content);
            MessageBox.Show("Dokument XML został pomyślnie zapisany w bazie danych.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDocumentList(); // Odświeżenie listy dokumentów
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Wystąpił błąd podczas zapisywania dokumentu: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


    private void DeleteButton_Click(object sender, EventArgs e)
    {
        try
        {
            string name = _nameTextBox.Text;
            _xmlProcessor.DeleteXmlDocument(name);
            MessageBox.Show("Dokument XML został pomyślnie usunięty.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDocumentList(); // Odświeżenie listy dokumentów
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Wystąpił błąd podczas usuwania dokumentu: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new XmlProcessorForm());
    }
}















































// using System;

// class Program
// {
//     static void Main()
//     {
//         // Utwórz instancję XmlProcessor
//         var xmlProcessor = new XmlProcessor();

//         // Zapisz przykładowy dokument XML
//         string documentName = "ExampleDocument";
//         string documentContent = "<root><element>Some content</element></root>";
//         xmlProcessor.SaveXmlDocument(documentName, documentContent);

//         string documentName2 = "ExampleDocument2";
//         string documentContent2 = "<root><element>A little bit longer content</element></root>";
//         xmlProcessor.SaveXmlDocument(documentName2, documentContent2);

//         Console.WriteLine("Dokument XML został pomyślnie zapisany w bazie danych.");

//         xmlProcessor.DeleteXmlDocument("ExampleDocument");
//     }
// }

// using Microsoft.Extensions.Hosting;

// namespace MyProject
// {
//     public class Program
//     {
//         public static void Main(string[] args)
//         {
//             CreateHostBuilder(args).Build().Run();
//         }

//         public static IHostBuilder CreateHostBuilder(string[] args) =>
//             Host.CreateDefaultBuilder(args)
//                 .ConfigureWebHostDefaults(webBuilder =>
//                 {
//                     webBuilder.UseStartup<Startup>();
//                 });
//     }
// }

// using System;
// using System;
// using System.Reflection;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.Hosting;
// using System.Security.Permissions;

// [assembly:AssemblyVersionAttribute("1.0.2000.0")]

// namespace MyProject
// {
//     public class Program
//     {
//         public static void Main(string[] args)
//         {
//             CreateHostBuilder(args).Build().Run();
//         }

//         public static IHostBuilder CreateHostBuilder(string[] args) =>
//             Host.CreateDefaultBuilder(args)
//                 .ConfigureWebHostDefaults(webBuilder =>
//                 {
//                     webBuilder.UseStartup<Startup>();
//                 });
//     }
// }


// using System;

// class Program
// {
//     static void Main()
//     {
//         // Utwórz instancję XmlProcessor
//         var xmlProcessor = new XmlProcessor();

//         Console.WriteLine("Wybierz opcję:");
//         Console.WriteLine("1. Zapisz nowy dokument XML");
//         Console.WriteLine("2. Usuń istniejący dokument XML");
//         Console.WriteLine("0. Wyjdź");

//         while (true)
//         {
//             Console.Write("Wybierz opcję: ");
//             string option = Console.ReadLine();

//             switch (option)
//             {
//                 case "1":
//                     Console.Write("Podaj nazwę nowego dokumentu XML: ");
//                     string documentName = Console.ReadLine();
//                     Console.Write("Podaj zawartość nowego dokumentu XML: ");
//                     string documentContent = Console.ReadLine();
//                     xmlProcessor.SaveXmlDocument(documentName, documentContent);
//                     Console.WriteLine("Dokument XML został pomyślnie zapisany w bazie danych.");
//                     break;

//                 case "2":
//                     Console.Write("Podaj nazwę dokumentu XML do usunięcia: ");
//                     string documentNameToDelete = Console.ReadLine();
//                     xmlProcessor.DeleteXmlDocument(documentNameToDelete);
//                     break;

//                 case "0":
//                     Console.WriteLine("Zakończono program.");
//                     return;

//                 default:
//                     Console.WriteLine("Niepoprawna opcja. Wybierz jeszcze raz.");
//                     break;
//             }
//         }
//     }
// }