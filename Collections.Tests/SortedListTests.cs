using NUnit.Framework;
using System.Collections;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Collections.Tests
{
    public abstract class BaseSortedList
    {
        protected BaseSortedList() { }

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

            return $"[{sb.Remove(sb.Length - 2, 2)}]";
        }
    }

    public class SortedIntListBase : BaseSortedList
    {
        protected static readonly Comparer<int> AscendingComparer =
            Comparer<int>.Create((x, y) => x.CompareTo(y));
        protected static readonly Comparer<int> DesscendingComparer =
            Comparer<int>.Create((x, y) => y.CompareTo(x));

        protected SortedList<int> list;
    }


    public class SortedIntListAscendingAddTests : SortedIntListBase
    {
        [SetUp]
        protected void Setup()
        {
            list = new SortedList<int>(AscendingComparer);
        }

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

    public class SortedIntListAscendingGetListByElementTests : SortedIntListBase
    {
        [SetUp]
        protected void Setup()
        {
            list = new SortedList<int>(AscendingComparer);
        }

        [Test]
        public void Test1()
        {
            list.Add(11);
            list.Add(55);
            list.Add(0);
            list.Add(-99);
            list.Add(101);

            var expected = list.OrderBy(x => x).Take(3).ToList();
            var actual = list.GetAllBefore(20);

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
            var actual = list.GetAllBefore(20);

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
            var actual = list.GetAllBefore(-100);

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
            var actual = list.GetAllBefore(-19);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test5()
        {
            list.Add(-90);

            var expected = list.OrderBy(x => x).Take(0).ToList();
            var actual = list.GetAllBefore(-100);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test6()
        {
            list.Add(-90);

            var expected = list.OrderBy(x => x).Take(1).ToList();
            var actual = list.GetAllBefore(0);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test7()
        {
            list.Add(-90);
            list.Add(-91);

            var expected = list.Take(0).ToList();
            var actual = list.GetAllBefore(-100);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test8()
        {
            list.Add(-90);
            list.Add(-91);

            var expected = list.OrderBy(x => x).Take(2).ToList();
            var actual = list.GetAllBefore(0);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test9()
        {
            list.Add(-90);
            list.Add(404);
            list.Add(500);

            var expected = list.OrderBy(x => x).Take(2).ToList();
            var actual = list.GetAllBefore(499);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test10()
        {
            var expected = list.Take(0).ToList();
            var actual = list.GetAllBefore(-100);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }
    }

    public class SortedIntListDesscendingAddTests : SortedIntListBase
    {
        [SetUp]
        protected void Setup()
        {
            list = new SortedList<int>(DesscendingComparer);
        }

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

            var expected = list.OrderByDescending(x => x).ToList();
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

            var expected = list.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(expected, list, Message(expected, list));
        }

        [Test]
        public void Test3()
        {
            list.Add(0);

            var expected = list.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(expected, list, Message(expected, list));
        }

        [Test]
        public void Test4()
        {
            var expected = list.OrderByDescending(x => x).ToList();
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

            var expected = list.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(expected, list, Message(expected, list));
        }

        [Test]
        public void Test6()
        {
            list.Add(0);
            list.Add(-10);
            list.Add(4);
            list.Add(5);

            var expected = list.OrderByDescending(x => x).ToList();
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

            var expected = list.OrderByDescending(x => x).ToList();
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

            var expected = list.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(expected, list, Message(expected, list));
        }

        [Test]
        public void Test9()
        {
            list.Add(10);
            list.Add(100);
            list.Add(1000);

            var expected = list.OrderByDescending(x => x).ToList();
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

            var expected = list.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(expected, list, Message(expected, list));
        }

    }

    public class SortedIntListDesscendingGetListByElementTests : SortedIntListBase
    {
        [SetUp]
        protected void Setup()
        {
            list = new SortedList<int>(DesscendingComparer);
        }

        [Test]
        public void Test1()
        {
            list.Add(11);
            list.Add(55);
            list.Add(0);
            list.Add(-99);
            list.Add(101);

            var expected = list.OrderBy(x => x).Take(3).ToList();
            var actual = list.GetAllAfter(20);

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
            var actual = list.GetAllAfter(20);

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
            var actual = list.GetAllAfter(-100);

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
            var actual = list.GetAllAfter(-19);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test5()
        {
            list.Add(-90);

            var expected = list.Take(0).ToList();
            var actual = list.GetAllAfter(-100);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test6()
        {
            list.Add(-90);

            var expected = list.Take(1).ToList();
            var actual = list.GetAllAfter(0);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test7()
        {
            list.Add(-90);
            list.Add(-91);

            var expected = list.Take(0).ToList();
            var actual = list.GetAllAfter(-100);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test8()
        {
            list.Add(-90);
            list.Add(-91);

            var expected = list.OrderBy(x => x).Take(2).ToList();
            var actual = list.GetAllAfter(0);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test9()
        {
            list.Add(-90);
            list.Add(404);
            list.Add(500);

            var expected = list.OrderBy(x => x).Take(2).ToList();
            var actual = list.GetAllAfter(499);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

        [Test]
        public void Test10()
        {
            var expected = list.Take(0).ToList();
            var actual = list.GetAllAfter(-100);

            CollectionAssert.AreEqual(expected, actual, Message(expected, actual));
        }

    }
}