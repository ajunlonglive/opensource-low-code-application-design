namespace Domain.Abstractions
{
    public interface IBuilder<out T>
    {
        T Build();
    }
}