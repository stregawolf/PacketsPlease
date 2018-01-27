using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffle<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T[] MakeShuffledArray(int size)
    {
        T[] array = new T[size];
        for(int i = 0; i < size; i++)
        {
            array[i] = i;
        }

        Shuffle<T>(array);

        return array;
    }

    public static void Shuffle<T>(T[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            // Use Next on random instance with an argument.
            // ... The argument is an exclusive bound.
            //     So we will not go past the end of the array.
            int r = i + _random.Next(n - i);
            T t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
    }
}
