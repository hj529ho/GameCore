using System;

public interface IRef
{
    int Count { get; }

    event Action OnAcquired;
    event Action OnReleased;

    void Acquire();
    void Release();
}

/// <summary>
/// 간단한 레퍼런스 카운팅
/// </summary>
public class Ref : IRef
{
    private int _count = 0;

    public event Action OnAcquired = delegate { };
    public event Action OnReleased = delegate { };

    public int Count => _count;
    public void Acquire()
    {
        _count++;
        if (_count == 1)
        {
            OnAcquired();
        }
    }
    public void Release()
    {
        _count--;
        if (_count == 0)
        {
            OnReleased();
        }
    }
}

/// <summary>
/// 간단한 스레드 세이프 레퍼런스 카운팅
/// </summary>
public class ConcurrentRef : IRef
{
    readonly object _lock;
    readonly IRef _ref;

    public ConcurrentRef(IRef @ref = null)
    {
        _lock = new object();
        _ref = @ref ?? new Ref();
    }

    public int Count => _ref.Count;
    public event Action OnAcquired
    {
        add => _ref.OnAcquired += value;
        remove => _ref.OnAcquired -= value;
    }
    public event Action OnReleased
    {
        add => _ref.OnReleased += value;
        remove => _ref.OnReleased -= value;
    }

    public void Acquire()
    {
        lock (_lock)
        {
            _ref.Acquire();
        }
    }
    public void Release()
    {
        lock (_lock)
        {
            _ref.Release();
        }
    }
}