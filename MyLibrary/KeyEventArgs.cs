namespace MyLibrary;

public class KeyEventArgs<T> : EventArgs
{
    public T Key { get; }
    
    public KeyEventArgs(T key)
    {
        Key = key;
    }
}