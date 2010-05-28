namespace Commons.Ebml
{
    public interface IDocumentRecognizer
    {
        ElementPrototype getElements();
        Element CreateElement(ElementPrototype type);
        Element CreateElement(ElementId type);
    }
}