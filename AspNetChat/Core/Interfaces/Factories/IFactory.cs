namespace AspNetChat.Core.Interfaces.Factories
{
    public interface IFactory<T>
    {
        T Create();
    }

    public interface IFactory<P1, T>
    {
        T Create(P1 param1);
    }
}
