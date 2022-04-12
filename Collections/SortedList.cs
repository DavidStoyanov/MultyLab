using System.Collections;

namespace Collections
{
    public class SortedList<E> : IEnumerable<E> where E : IComparable<E>
    {
        private static readonly Comparer<E> AscendingComparer =
            Comparer<E>.Create((x, y) => x.CompareTo(y));

        protected List<E> elements;
        private readonly Comparer<E> comparer;

        public SortedList()
        {
            elements = new List<E>();
            comparer = AscendingComparer;
        }

        public SortedList(Comparer<E> comparer)
        {
            elements = new List<E>();
            this.comparer = comparer;
        }

        public void Add(E element)
        {
            int index = FindIndex(element, 0, elements.Count);
            elements.Insert(index, element);
        }

        public E? Peek()
        {
            if (!elements.Any())
                return default;

            return elements[Count() - 1];
        }

        public E RemoveLast()
        {
            if (!elements.Any())
                throw new InvalidOperationException("Remove element on empty collection");

            E element = elements[Count() - 1];
            elements.RemoveAt(Count() - 1);

            return element;
        }

        public bool Remove(E element)
        {
            // ?? WARNING TODO...
            // If element's value same for more than one element in the list
            // Should get the whole list of same elements and remove the correct element

            int index = FindIndex(element, 0, elements.Count);

            if (!elements.Any() || index >= elements.Count)
                return false;

            E foundElement = elements[index];

            if (!foundElement.Equals(element))
                return false;

            elements.RemoveAt(index);

            return true;
        }

        public int Count()
        {
            return elements.Count;
        }

        public bool Any()
        {
            return elements.Any();
        }

        public void Clear()
        {
            elements = new List<E>();
        }

        public bool Contains(E element)
        {
            return elements.Contains(element);
        }

        public void CopyTo(int index, E[] array, E element)
        {
            int foundIndex = FindIndex(element, 0, elements.Count);
            for (int i = 0; i < foundIndex; i++)
            {
                array[index + i] = elements[i];
            }
        }

        public IList<E> LowerElementsToList(E element)
        {
            IList<E> list = new List<E>();

            int foundIndex = FindIndex(element, 0, elements.Count);

            for (int i = 0; i < foundIndex; i++) list.Add(elements[i]);

            return list;
        }

        public IEnumerator<E> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        private int FindIndex(E element, int start, int end)
        {
            // Search O(log n)
            int difference = end - start;
            int half = difference / 2;

            if (half == 0)
            {
                if (elements.Any())
                {
                    E startElement = elements[start];
                    //if (startElement.CompareTo(element) >= 0)
                    if (comparer.Compare(startElement, element) >= 0)
                        return start;
                }

                return end;
            }

            int midIndex = start + half;
            E midElement = elements[midIndex];
            if (comparer.Compare(element, midElement) > 0)
            {
                // element is greater
                start = midIndex;
                return FindIndex(element, start, end);
            }
            else if (comparer.Compare(element, midElement) < 0)
            {
                // element is less
                end = midIndex;
                return FindIndex(element, start, end);
            }
            else
            {
                // element is equal
                return midIndex;
            }
        }


    }
}