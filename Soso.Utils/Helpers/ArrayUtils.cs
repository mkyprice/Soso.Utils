using System;

namespace Soso.Utils.Helpers;

public static class ArrayUtils
{
    public static T[] RemoveAt<T>(this T[] array, int index)
    {
        if (index < 0 || index >= array.Length)
        {
            throw new ArgumentOutOfRangeException($"{nameof(RemoveAt)} Index {index} is out of range");
        }

        T[] copy = new T[array.Length - 1];
        for (int i = 0; i < index; i++)
        {
            copy[i] = array[i];
        }
        for (int i = index; i < array.Length - 1; i++)
        {
            copy[i] = array[i + 1];
        }
        return copy;
    }
    
    public static void RemoveAt<T>(ref T[] array, int index)
    {
        if (index < 0 || index >= array.Length)
        {
            throw new ArgumentOutOfRangeException($"{nameof(RemoveAt)} Index {index} is out of range");
        }

        for (int i = index; i < array.Length - 1; i++)
        {
            array[i] = array[i + 1];
        }
        Array.Resize(ref array, array.Length - 1);
    }

    public static T[] InsertAt<T>(this T[] array, T item, int index)
    {
        if (index < 0 || index > array.Length)
        {
            throw new ArgumentOutOfRangeException($"{nameof(RemoveAt)} Index {index} is out of range");
        }
        
        T[] copy = new T[array.Length + 1];
        for (int i = 0; i < index; i++)
        {
            copy[i] = array[i];
        }
        
        copy[index] = item;
        
        for (int i = index + 1; i < copy.Length; i++)
        {
            copy[i] = array[i - 1];
        }
        return copy;
    }

    public static void InsertAt<T>(ref T[] array, T item, int index)
    {
        if (index < 0 || index > array.Length)
        {
            throw new ArgumentOutOfRangeException($"{nameof(RemoveAt)} Index {index} is out of range");
        }
        
        Array.Resize(ref array, array.Length + 1);
        
        for (int i = array.Length - 1; i >= Math.Max(1, index); i--)
        {
            array[i] = array[i - 1];
        }
        array[index] = item;
    }
}