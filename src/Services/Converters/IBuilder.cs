namespace Presentation.Converters
{
    public interface IBuilder<out T>
    {
        T Build();
    }
}