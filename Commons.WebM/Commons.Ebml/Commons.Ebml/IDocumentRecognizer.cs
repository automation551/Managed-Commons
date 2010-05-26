namespace Commons.Ebml
{
    public interface IDocumentRecognizer
    {
        public ElementPrototype getElements();
        public Element CreateElement(ElementPrototype type);
        public Element CreateElement(ElementId type);
    }
}