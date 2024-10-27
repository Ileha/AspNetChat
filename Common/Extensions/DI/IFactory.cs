namespace Common.Extensions.DI
{
    public interface IFactory<out T>
    {
        T Create();
    }

    public interface IFactory<in P0, out T>
    {
        T Create(P0 param0);
    }

    public interface IFactory<in P0, in P1, out T>
    {
        T Create(P0 param0, P1 param1);
    }

    public interface IFactory<in P0, in P1, in P2, out T>
    {
        T Create(P0 param0, P1 param1, P2 param2);
    }

    public interface IFactory<in P0, in P1, in P2, in P3, out T>
    {
        T Create(P0 param0, P1 param1, P2 param2, P3 param3);
    }

    public interface IFactory<in P0, in P1, in P2, in P3, in P4, out T>
    {
        T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4);
    }

    public interface IFactory<in P0, in P1, in P2, in P3, in P4, in P5, out T>
    {
        T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5);
    }

    public interface IFactory<in P0, in P1, in P2, in P3, in P4, in P5, in P6, out T>
    {
        T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6);
    }

    public interface IFactory<in P0, in P1, in P2, in P3, in P4, in P5, in P6, in P7, out T>
    {
        T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7);
    }

    public interface IFactory<in P0, in P1, in P2, in P3, in P4, in P5, in P6, in P7, in P8, out T>
    {
        T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7, P8 param8);
    }

    public interface IFactory<in P0, in P1, in P2, in P3, in P4, in P5, in P6, in P7, in P8, in P9, out T>
    {
        T Create(P0 param0, P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6, P7 param7, P8 param8, P9 param9);
    }

}