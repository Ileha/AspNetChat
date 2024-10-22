namespace AspNetChat.Extensions.DI
{
    public interface IFactory<T>
    {
        T Create();
    }

    public interface IFactory<P0, T>
    {
        T Create(P0 param0);
    }

    public interface IFactory<P0, P1, T>
    {
        T Create(P0 param0, P1 param1);
    }

    public interface IFactory<P0, P1, P2, T>
    {
        T Create(P0 param0, P1 param1, P2 param2);
    }

    public interface IFactory<P0, P1, P2, P3, T>
    {
        T Create(P0 param0, P1 param1, P2 param2, P3 param3);
    }

    public interface IFactory<P0, P1, P2, P3, P4, T>
    {
        T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4);
    }

    public interface IFactory<P0, P1, P2, P3, P4, P5, T>
    {
        T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5);
    }

    public interface IFactory<P0, P1, P2, P3, P4, P5, P6, T>
    {
        T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6);
    }

    public interface IFactory<P0, P1, P2, P3, P4, P5, P6, P7, T>
    {
        T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7);
    }

    public interface IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, T>
    {
        T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7, P8 param8);
    }

    public interface IFactory<P0, P1, P2, P3, P4, P5, P6, P7, P8, P9, T>
    {
        T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7, P8 param8, P9 param9);
    }

}