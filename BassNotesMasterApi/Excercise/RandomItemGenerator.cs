using System;

namespace BassNotesMasterApi.Excercise
{
    public class RandomItemGenerator
    {
        private readonly Random _random = new Random();
        private int _lastIndex = -1;

        private int TryNext(int collectionSize)
        {
            return _random.Next(0, collectionSize);
        }

        public int Next(int collectionSize)
        {
            int next;
            do
            {
                next = TryNext(collectionSize);
            } while (next.Equals(_lastIndex));
            return _lastIndex = next;
        }
    }
}