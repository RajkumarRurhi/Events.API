namespace Events.API.Helpers
{
    public interface IPropertyChecker
    {
        bool HasProperties<T>(string fields);
    }
}