using System;

public interface IXmlProcessor
{
    void SaveXmlDocument(string name, string content);

    public void DeleteXmlDocument(string name);
    List<string> GetXmlDocumentNames();
    string SearchXmlDocuments(string xpath);
    void UpdateXmlDocument(string name, string xpath, string newValue);
}
