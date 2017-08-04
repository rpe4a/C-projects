namespace TryTypeObject.project
{
    public interface IFactory<out T> where T : class, new()
    {
        T CreateInstance();
    }
}