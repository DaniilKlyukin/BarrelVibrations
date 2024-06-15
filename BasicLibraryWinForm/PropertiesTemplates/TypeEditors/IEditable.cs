namespace BasicLibraryWinForm.PropertiesTemplates.TypeEditors
{
    public interface IEditable<T>
    {
        public T GetValues();

        public void SetValues(T values);
    }
}
