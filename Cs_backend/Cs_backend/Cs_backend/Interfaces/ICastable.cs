namespace Cs_backend.Interfaces;

public interface ICastable<out T>
{
    public T Cast();
}