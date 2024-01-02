using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A stack based <see cref="Pool{T}"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Pool<T> : IDisposable
    where T : class
{
    private readonly List<T> list;

    private readonly Func<T> createFunc;

    private readonly Action<T> actionOnGet;

    private readonly Action<T> actionOnRelease;

    private readonly Action<T> actionOnDestroy;

    private readonly int maxSize;

    private T freshlyReleased;

    /// <summary>
    /// The total number of active and inactive <see cref="GameObject"/>s.
    /// </summary>
    public int CountAll { get; private set; }

    /// <summary>
    /// Number of <see cref="GameObject"/>s that have been created by the pool but are currently in use and have not yet been returned.
    /// </summary>
    public int CountActive => CountAll - CountInactive;

    /// <summary>
    /// Number of <see cref="GameObject"/>s that are currently available in the pool..
    /// </summary>
    public int CountInactive => list.Count + ((freshlyReleased != null) ? 1 : 0);

    /// <summary>
    /// Create new <see cref="Pool{T}"/> instance.
    /// </summary>
    /// <param name="createFunc">Used to create a new instance when the pool is empty. In most cases this will just be () => new T().</param>
    /// <param name="actionOnGet">Called when the instance is taken from the pool.</param>
    /// <param name="actionOnRelease">Called when the instance is returned to the pool. This can be used to clean up or disable the instance.</param>
    /// <param name="actionOnDestroy">Called when the element could not be returned to the pool due to the pool reaching the maximum size.</param>
    /// <param name="defaultCapacity">The default capacity the stack will be created with.</param>
    /// <param name="maxSize">The maximum size of the pool. When the pool reaches the max size then any further instances returned to the pool will be ignored and can be garbage collected. This can be used to prevent the pool growing to a very large size.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public Pool(
        Func<T> createFunc,
        Action<T> actionOnGet = null,
        Action<T> actionOnRelease = null,
        Action<T> actionOnDestroy = null,
        int defaultCapacity = 10,
        int maxSize = 1000
    )
    {
        if (createFunc == null)
        {
            throw new ArgumentNullException("createFunc");
        }

        if (maxSize <= 0)
        {
            throw new ArgumentException("Max Size must be greater than 0", "maxSize");
        }

        list = new List<T>(defaultCapacity);

        this.createFunc = createFunc;
        this.maxSize = maxSize;
        this.actionOnGet = actionOnGet;
        this.actionOnRelease = actionOnRelease;
        this.actionOnDestroy = actionOnDestroy;
    }

    /// <summary>
    /// Get an instance from the pool. If the pool is empty then a new instance will be created.
    /// </summary>
    /// <returns></returns>
    public T Get()
    {
        T value = null;

        if (freshlyReleased != null)
        {
            value = freshlyReleased;
            freshlyReleased = null;
        }
        else if (list.IsEmpty())
        {
            value = createFunc();
            CountAll += 1;
        }
        else
        {
            int index = list.Count - 1;
            value = list[index];
            list.RemoveAt(index);
        }

        actionOnGet?.Invoke(value);

        return value;
    }

    /// <summary>
    /// Returns the instance back to the pool.
    /// </summary>
    /// <param name="element"></param>
    public void Release(T element)
    {
        if (list.Count > 0 || freshlyReleased != null)
        {
            if (HasElement(element))
            {
                throw new InvalidOperationException(
                    "Trying to release an object that has already been released to the pool."
                );
            }
        }

        actionOnRelease?.Invoke(element);

        if (freshlyReleased == null)
        {
            freshlyReleased = element;
        }
        else if (CountInactive < maxSize)
        {
            list.Add(element);
        }
        else
        {
            actionOnDestroy?.Invoke(element);
            CountAll -= 1;
        }
    }

    /// <summary>
    /// Removes all pooled items. If the pool contains a destroy callback then it will be called for each item that is in the pool.
    /// </summary>
    public void Clear()
    {
        if (actionOnDestroy != null)
        {
            foreach (var item in list)
            {
                actionOnDestroy(item);
            }

            if (freshlyReleased != null)
            {
                actionOnDestroy(freshlyReleased);
            }
        }

        freshlyReleased = null;
        list.Clear();
        CountAll = 0;
    }

    /// <summary>
    /// Removes all pooled items. If the pool contains a destroy callback then it will be called for each item that is in the pool.
    /// </summary>
    public void Dispose()
    {
        Clear();
    }

    private bool HasElement(T element)
    {
        return freshlyReleased == element || list.Contains(element);
    }
}
