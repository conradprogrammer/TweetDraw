using System;

namespace TweetDraw.Helpers
{
    internal class Utils
    {
        static public int[] RandomeIndexes(int size)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            int[] indexes = new int[size];
            for (int i = 0; i < size; i++) indexes[i] = -1;
            int _temp = 0;
            int _swapIndex = 0;
            for (int i = 0; i < size; i++)
            {
                _swapIndex = random.Next(size);
                _temp = (indexes[i] == -1) ? i : indexes[i];
                indexes[i] = (indexes[_swapIndex] == -1) ? _swapIndex : indexes[_swapIndex];
                indexes[_swapIndex] = _temp;
            }
            return indexes;
        }
    }
}
