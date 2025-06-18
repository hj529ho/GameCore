using UnityEngine;

public interface ISubject 
{
    
}

public interface IObserver<T> where T : class
{
    public void OnNotify(T value);
}
