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
    private TextBox _xpathTextBoxSearch;
    private TextBox _xpathTextBoxNewContent;
    private TextBox _newValueTextBox;
    private Button _searchButton;
    private Button _updateButton;
    private TextBox _displayContentTextBox;
    private Label xpathLabel2; // Przeniesione na poziom klasy
    private Label newValueLabel; // Przeniesione na poziom klasy
    private Label infoLabel;

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
            Width = 120,
        };
        saveButton.Click += SaveButton_Click;

        Label listLabel = new Label
        {
            Text = "Lista dokumentów w bazie danych:",
            AutoSize = true
        };

        _documentListBox = new ListBox
        {
            Width = 250,
            Height = 150
        };
        _documentListBox.SelectedIndexChanged += DocumentListBox_SelectedIndexChanged;

        _searchButton = new Button
        {
            Text = "Wyszukaj węzeł/atrybut",
            Width = 120
        };
        _searchButton.Click += SearchButton_Click;

        _updateButton = new Button
        {
            Text = "Zaktualizuj węzeł/atrybut",
            Width = 120,
            Visible = false
        };
        _updateButton.Click += UpdateButton_Click;

        FlowLayoutPanel panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown            
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

        _xpathTextBoxSearch = new TextBox
        {
            Width = 250
        };

        infoLabel = new Label{
            Text = "Wybierz element z listy",
            Margin = new Padding(0, 0, 0, 10),
            AutoSize = true,
            Visible = true
        };

        xpathLabel2 = new Label // Zmienna na poziomie klasy
        {
            Text = "XPath:",
            AutoSize = true,
            Visible = false
        };

        _xpathTextBoxNewContent = new TextBox
        {
            Width = 250,
            Visible = false
        };

        newValueLabel = new Label // Zmienna na poziomie klasy
        {
            Text = "Nowa wartość:",
            AutoSize = true,
            Visible = false
        };

        _newValueTextBox = new TextBox
        {
            Width = 250,
            Visible = false
        };

        _displayContentTextBox = new TextBox
        {
            Multiline = true,
            Height = 200,
            Width = 250,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical, // Dodanie pionowych pasków przewijania
            Visible = false
        };

        panel.Controls.AddRange(new Control[] { nameLabel, _nameTextBox, contentLabel, _contentTextBox, saveButton, listLabel, _documentListBox, infoLabel, _deleteButton, xpathLabel, _xpathTextBoxSearch, _searchButton, xpathLabel2, _xpathTextBoxNewContent, newValueLabel, _newValueTextBox, _updateButton, _displayContentTextBox });

        Controls.Add(panel);
        Text = "Przetwarzanie dokumentów XML";
        ClientSize = new System.Drawing.Size(550, 400);
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
            _nameTextBox.Text = ""; // Wyczyszczenie zawartości pola nazwy dokumentu
            _contentTextBox.Text = ""; // Wyczyszczenie zawartości pola zawartości dokumentu
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
                infoLabel.Visible = true;
                _deleteButton.Visible = false; // Ukrycie przycisku po usunięciu dokumentu
                _displayContentTextBox.Visible = false;
                xpathLabel2.Visible = false;
                _xpathTextBoxNewContent.Visible = false;
                newValueLabel.Visible = false;
                _newValueTextBox.Visible = false;
                _updateButton.Visible = false;
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
            infoLabel.Visible = false;
            _deleteButton.Visible = true; // Wyświetlenie przycisku usuwania po wybraniu dokumentu

            // Pobierz zawartość wybranego dokumentu i wyświetl w _displayContentTextBox
            _displayContentTextBox.Visible = true;
            xpathLabel2.Visible = true;
            _xpathTextBoxNewContent.Visible = true;
            newValueLabel.Visible = true;
            _newValueTextBox.Visible = true;
            _updateButton.Visible = true;
            string content = _xmlProcessor.GetXmlDocumentContent(_selectedDocumentName);
            _displayContentTextBox.Text = content;
        }
        else
        {
            _selectedDocumentName = null;
            _deleteButton.Visible = false; // Ukrycie przycisku, jeśli nie wybrano dokumentu
            infoLabel.Visible = true;
            _displayContentTextBox.Visible = false;
            xpathLabel2.Visible = false;
            _xpathTextBoxNewContent.Visible = false;
            newValueLabel.Visible = false;
            _newValueTextBox.Visible = false;
            _updateButton.Visible = false;
        }
    }

    private void SearchButton_Click(object sender, EventArgs e)
    {
        string xpath = _xpathTextBoxSearch.Text.Trim();
        if (string.IsNullOrEmpty(xpath))
        {
            MessageBox.Show("Proszę podać XPath do wyszukania.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            string result = _xmlProcessor.SearchXmlDocuments(xpath);
            MessageBox.Show(result, "Wyniki wyszukiwania", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Wystąpił nieoczekiwany błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _xpathTextBoxSearch.Text = "";
        }
    }

    private void UpdateButton_Click(object sender, EventArgs e)
    {
        string name = _selectedDocumentName; // użyj wybranej nazwy dokumentu z listy
        string xpath = _xpathTextBoxNewContent.Text;
        string newValue = _newValueTextBox.Text;
        try
        {
            _xmlProcessor.UpdateXmlDocument(name, xpath, newValue);
            _deleteButton.Visible = false; // Ukrycie przycisku, jeśli nie wybrano dokumentu
            infoLabel.Visible = true;
            _displayContentTextBox.Visible = false;
            xpathLabel2.Visible = false;
            _xpathTextBoxNewContent.Visible = false;
            newValueLabel.Visible = false;
            _newValueTextBox.Visible = false;
            _updateButton.Visible = false;
            _xpathTextBoxNewContent.Text = "";
            _newValueTextBox.Text = "";
            LoadDocumentList();
            MessageBox.Show("Dokument XML został pomyślnie zaktualizowany.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd podczas aktualizacji dokumentu XML: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
