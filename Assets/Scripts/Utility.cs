using System;
using System.Linq;

public static class Utility
{
    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        System.Random prng = new System.Random(seed);

        for (int i = 0; i < array.Length - 1; i++)
        {
            int rIndex = prng.Next(i, array.Length);

            T tempItem = array[rIndex];

            array[rIndex] = array[i];
            array[i] = tempItem;
        }

        return array;
    }

    public static T[] GetEnumValues<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
    }
}
