namespace ProjectGame.Core.Pooling
{
    public interface IPool<T>
    {
        bool IsReady { get; }
        T Get();
        void Release(T item);
    }
}