using NUnit.Framework;
using System.Collections;
using System.Text;
using System.Linq;

namespace Collections.Tests
{
    public abstract class BaseSortedList
    {
        protected SortedList<int> list;

        protected BaseSortedList() { }

        [SetUp]
        protected void Setup()
        {
            list = new SortedList<int>();
        }


        protected static string Message(IEnumerable expected, IEnumerable actual)
        {
            string message = "Expected: {0}\n    Actual: {1}";
            return string.Format(message, CollectionToString(expected), CollectionToString(actual));
        }

        protected static string CollectionToString(IEnumerable collection)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in collection)
            {
                sb.Append(item.ToString());
                sb.Append(", ");
            }

            if (collection.GetEnumerator().MoveNext() == false)
            {
                return "[]";
            }

            return $"[{sb.Remove(sb.Length - 2, 1)}]";
        }
    }

    public class SortedListAddTests : BaseSortedList
    {

        [Test]
        public void Test1()
        {
            list.Add(0);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(10);
            list.Add(7);
            list.Add(8);
            list.Add(9);
            list.Add(10);
            list.Add(6);
            list.Add(4);
            list.Add(5);

            var expected = list.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(expected, list, Message(expected, list));
        }

        [Test]
        public void Test2()
        {
            list.Add(0);
            list.Add(0);
            list.Add(0);
            list.Add(0);
            list.Add(0);
            list.Add(-10);
            list.Add(4);
            list.Add(5);

            var expected = list.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(expected, list, Message(expected, list));
        }

        [Test]
        public void Test3()
        {
            list.Add(0);

            var expected = list.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(expected, list, Message(expected, list));
        }

        [Test]
        public void Test4()
        {
            var expected = list.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(expected, list, Message(expected, list));
        }

        [Test]
        public void Test5()
        {
            list.Add(345);
            list.Add(1555);
            list.Add(12);
            list.Add(-89);
            list.Add(100);
            list.Add(30);
            list.Add(31);
            list.Add(21);
            list.Add(-10);
            list.Add(58);
            list.Add(25);
            list.Add(21);
            list.Add(21);
            list.Add(25);
            list.Add(26);
            list.Add(19);
            list.Add(-100);
            list.Add(2000);
            list.Add(0);

            var expected = list.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(expected, list, Message(expected, list));
        }

        [Test]
        public void Test6()
        {
            list.Add(0);
            list.Add(-10);
            list.Add(4);
            list.Add(5);

            var expected = list.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(expected, list, Message(expected, list));
        }

        [Test]
        public void Test7()
        {
            list.Add(1);
            list.Add(2);
            list.Add(-10);
            list.Add(4);
            list.Add(5);

            var expected = list.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(expected, list, Message(expected, list));
        }

        [Test]
        public void Test8()
        {
            list.Add(0);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(10);
            list.Add(7);
            list.Add(8);
            list.Add(9);
            list.Add(10);
            list.Add(6);
            list.Add(4);
            list.Add(5);
            list.Add(-50);
            list.Add(-0);
            list.Add(-1);

            var expected = list.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(expected, list, Message(expected, list));
        }

        [Test]
        public void Test9()
        {
            list.Add(10);
            list.Add(100);
            list.Add(1000);

            var expected = list.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(expected, list, Message(expected, list));
        }

        [Test]
        public void Test10()
        {
            list.Add(-10);
            list.Add(-100);
            list.Add(-1000);
            list.Add(-1000);
            list.Add(-100);
            list.Add(-10);

            var expected = list.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(expected, list, Message(expected, list));
        }

    }

    public class SortedListCopyTo : BaseSortedList
    {

        [Test]
        public void Test1()
        {
            list.Add(11);
            list.Add(55);
            list.Add(0);
            list.Add(-99);
            list.Add(101);

            var expected = list.OrderBy(x => x).Take(3).ToList();
            var actual = list.LowerElementsToList(20);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test2()
        {
            list.Add(0);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(10);
            list.Add(7);
            list.Add(8);
            list.Add(9);
            list.Add(10);
            list.Add(6);
            list.Add(4);
            list.Add(5);

            var expected = list.OrderBy(x => x).ToList();
            var actual = list.LowerElementsToList(20);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test3()
        {
            list.Add(0);
            list.Add(-20);
            list.Add(25640);
            list.Add(289);

            var expected = list.Take(0).ToList();
            var actual = list.LowerElementsToList(-100);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test4()
        {
            list.Add(0);
            list.Add(-20);
            list.Add(25640);
            list.Add(289);

            var expected = list.OrderBy(x => x).Take(1).ToList();
            var actual = list.LowerElementsToList(-19);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test5()
        {
            list.Add(-90);

            var expected = list.OrderBy(x => x).Take(0).ToList();
            var actual = list.LowerElementsToList(-100);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test6()
        {
            list.Add(-90);

            var expected = list.OrderBy(x => x).Take(1).ToList();
            var actual = list.LowerElementsToList(0);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test7()
        {
            list.Add(-90);
            list.Add(404);
            list.Add(500);

            var expected = list.OrderBy(x => x).Take(2).ToList();
            var actual = list.LowerElementsToList(499);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }
    }
}