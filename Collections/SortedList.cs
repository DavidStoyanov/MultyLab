using System.Collections;

namespace Collections
{
    public class SortedList<E> : IEnumerable<E> where E : IComparable<E>
    {
        private Comparer<E> Comparer;
        protected List<E> Elements;


        public SortedList()
        {
            Elements = new List<E>();
            InitiateComparer(null);
        }

        public SortedList(int capacity)
        {
            Elements = new List<E>(capacity);
            InitiateComparer(null);
        }

        public SortedList(Comparer<E> comparer)
        {
            Elements = new List<E>();
            InitiateComparer(comparer);
        }

        public SortedList(Comparer<E> comparer, int capacity)
        {
            Elements = new List<E>(capacity);
            InitiateComparer(comparer);
        }


        public void Add(E element)
        { 
            int index = FindIndexOfElement(element);
            Elements.Insert(index, element);
        }

        public E? PeekFirst()
        {
            if (!Elements.Any()) return default;

            return Elements[0];
        }

        public E? PeekLast()
        {
            if (!Elements.Any()) return default;

            return Elements[Count() - 1];
        }

        public E RemoveFirst()
        {
            IsCollectionEmptyCheckAndThrow();

            E element = Elements[0];
            Elements.RemoveAt(0);

            return element;
        }

        public E RemoveLast()
        {
            IsCollectionEmptyCheckAndThrow();

            E element = Elements[Count() - 1];
            Elements.RemoveAt(Count() - 1);

            return element;
        }

        public bool Remove(E element)
        {
            if (!Elements.Any()) return false;

            // find the index of first element with same value
            int index = FindIndexOfElement(element);

            if (index >= Elements.Count) return false;

            // find the exact same element index
            index = FindIndexElementByNearIndex(element, index);

            E foundElement = Elements[index];

            if (!foundElement.Equals(element)) return false;

            Elements.RemoveAt(index);

            return true;
        }

        private int FindIndexElementByNearIndex(E element, int index)
        {
            int indexInner = index + 1;
            while (indexInner >= 0)
            {
                indexInner--;
                E foundElement = Elements[indexInner];
                if (foundElement.Equals(element)) return indexInner;
                if (foundElement.CompareTo(element) != 0) break;
            }
            indexInner = index;
            while (indexInner < Elements.Count - 1)
            {
                indexInner++;
                E foundElement = Elements[indexInner];
                if (foundElement.Equals(element)) return indexInner;
                if (foundElement.CompareTo(element) != 0) break;
            }
            return index;
        }


        public int Count()
        {
            return Elements.Count;
        }

        public bool Any()
        {
            return Elements.Any();
        }

        public void Clear()
        {
            Elements.Clear();
        }

        public bool Contains(E element)
        {
            return Elements.Contains(element);
        }

        public void CopyTo(int index, E[] array)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                array[index + i] = Elements[i];
            }
        }

        public void CopyTo(int index, E[] array, E element)
        {
            int foundIndex = FindIndexOfElement(element);
            for (int i = 0; i < foundIndex; i++)
            {
                array[index + i] = Elements[i];
            }
        }

        public E[] GetAllBefore(E element)
        {
            if (!Elements.Any()) return Array.Empty<E>();

            int foundIndex = FindIndexOfElement(element);

            E[] array = new E[foundIndex];

            for (int i = 0; i < foundIndex; i++)
            {
                array[i] = Elements[i];
            }

            return array;
        }

        public E[] GetAllAfter(E element)
        {
            if (!Elements.Any()) return Array.Empty<E>();

            int foundIndex = FindIndexOfElement(element);
            int length = Elements.Count - foundIndex;

            E[] array = new E[length];

            for (int i = foundIndex; i < Elements.Count; i++)
            {
                array[Elements.Count - i - 1] = Elements[i];
            }

            return array;
        }

        public IEnumerator<E> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        private int FindIndexOfElement(E element)
        {
            return FindIndex(element, 0, Elements.Count);
        }

        private int FindIndex(E element, int start, int end)
        {
            // Search O(log n)
            int difference = end - start;
            int half = difference / 2;

            if (half == 0)
            {
                if (Elements.Any())
                {
                    if (Comparer.Compare(Elements[start], element) >= 0)
                    {
                        return start;
                    }  
                }

                return end;
            }

            int midIndex = start + half;
            E midElement = Elements[midIndex];
            if (Comparer.Compare(element, midElement) > 0)
            {
                // # element is greater
                start = midIndex;
                return FindIndex(element, start, end);
            }
            else if (Comparer.Compare(element, midElement) < 0)
            {
                // # element is lower
                end = midIndex;
                return FindIndex(element, start, end);
            }
            else
            {
                // # element is equal
                return midIndex;
            }
        }

        private void IsCollectionEmptyCheckAndThrow()
        {
            if (!Elements.Any())
            {
                string excMsg = "Remove element on empty collection";
                throw new EmptyCollectionException(excMsg);
            }
        }

        private void InitiateComparer(Comparer<E>? comparer)
        {
            Comparer<E> AscendingComparer = Comparer<E>.Create((a, b) => a.CompareTo(b));
            if (comparer == null) comparer = AscendingComparer;
            Comparer = comparer;
        }


    }
}