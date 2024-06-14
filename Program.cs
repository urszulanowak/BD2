using System;
using System.Windows.Forms;

public class XmlProcessorForm : Form
{
    private readonly XmlProcessor _xmlProcessor;
    private TextBox _nameTextBox;
    private TextBox _contentTextBox;
    private ListBox _documentListBox;
    private Button _deleteButton;
    private string _selectedDocumentName;
    private TextBox _xpathTextBox;
    private TextBox _newValueTextBox;
    private Button _searchButton;
    private Button _updateButton;

    public XmlProcessorForm()
    {
        _xmlProcessor = new XmlProcessor();
        InitializeComponents();
        LoadDocumentList();
    }

    private void InitializeComponents()
    {
        Label nameLabel = new Label
        {
            Text = "Nazwa dokumentu:",
            AutoSize = true
        };

        _nameTextBox = new TextBox
        {
            Width = 250
        };

        Label contentLabel = new Label
        {
            Text = "Zawartość dokumentu:",
            AutoSize = true
        };

        _contentTextBox = new TextBox
        {
            Multiline = true,
            Height = 100,
            Width = 250
        };

        Button saveButton = new Button
        {
            Text = "Zapisz dokument",
            Width = 120
        };
        saveButton.Click += SaveButton_Click;


        _documentListBox = new ListBox
        {
            Width = 250,
            Height = 150
        };
        _documentListBox.SelectedIndexChanged += DocumentListBox_SelectedIndexChanged;

        FlowLayoutPanel panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            Padding = new Padding(10)
        };

        _deleteButton = new Button
        {
            Text = "Usuń dokument",
            Width = 120,
            Visible = false // Ukryj przycisk usuwania na początku
        };
        _deleteButton.Click += DeleteButton_Click;

        Label xpathLabel = new Label
        {
            Text = "XPath:",
            AutoSize = true
        };

        _xpathTextBox = new TextBox
        {
            Width = 250
        };

        Label newValueLabel = new Label
        {
            Text = "Nowa wartość:",
            AutoSize = true
        };

        _newValueTextBox = new TextBox
        {
            Width = 250
        };

        _searchButton = new Button
        {
            Text = "Wyszukaj",
            Width = 120
        };
        _searchButton.Click += SearchButton_Click;

        _updateButton = new Button
        {
            Text = "Zaktualizuj",
            Width = 120
        };
        _updateButton.Click += UpdateButton_Click;

        panel.Controls.AddRange(new Control[] {nameLabel, _nameTextBox, contentLabel, _contentTextBox, saveButton, _documentListBox, _deleteButton, xpathLabel, _xpathTextBox, newValueLabel, _newValueTextBox, _searchButton, _updateButton });

        // panel.Controls.AddRange(new Control[] { nameLabel, _nameTextBox, contentLabel, _contentTextBox, saveButton, _deleteButton, _documentListBox });

        Controls.Add(panel);
        Text = "Przetwarzanie dokumentów XML";
        ClientSize = new System.Drawing.Size(600, 400);
    }

    private void LoadDocumentList()
    {
        _documentListBox.Items.Clear();
        var documentNames = _xmlProcessor.GetXmlDocumentNames();
        foreach (var name in documentNames)
        {
            _documentListBox.Items.Add(name);
        }
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
        if (!string.IsNullOrEmpty(_selectedDocumentName))
        {
            try
            {
                _xmlProcessor.DeleteXmlDocument(_selectedDocumentName);
                MessageBox.Show("Dokument XML został pomyślnie usunięty.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDocumentList(); // Odświeżenie listy dokumentów
                _deleteButton.Visible = false; // Ukrycie przycisku po usunięciu dokumentu
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

    private void DocumentListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_documentListBox.SelectedIndex >= 0)
        {
            _selectedDocumentName = _documentListBox.SelectedItem.ToString();
            _deleteButton.Visible = true; // Wyświetlenie przycisku usuwania po wybraniu dokumentu
        }
        else
        {
            _selectedDocumentName = null;
            _deleteButton.Visible = false; // Ukrycie przycisku, jeśli nie wybrano dokumentu
        }
    }

    private void SearchButton_Click(object sender, EventArgs e)
    {
        string xpath = _xpathTextBox.Text;
        string result = _xmlProcessor.SearchXmlDocuments(xpath);
        MessageBox.Show(result, "Wyniki wyszukiwania", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void UpdateButton_Click(object sender, EventArgs e)
    {
        string name = _selectedDocumentName; // użyj wybranej nazwy dokumentu z listy
        string xpath = _xpathTextBox.Text;
        string newValue = _newValueTextBox.Text;
        try
        {
            _xmlProcessor.UpdateXmlDocument(name, xpath, newValue);
            MessageBox.Show("Dokument XML został pomyślnie zaktualizowany.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd podczas aktualizacji dokumentu XML: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
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