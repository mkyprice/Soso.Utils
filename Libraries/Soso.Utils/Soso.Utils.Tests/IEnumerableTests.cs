using System;
using System.Collections.Generic;
using Soso.Utils;

namespace Soso.Utils.Tests
{
    // ReSharper disable once InconsistentNaming
    public class IEnumerableTests
    {
        private List<int> _intList;
        [SetUp]
        public void Setup()
        {
            _intList = BuildList(1_000_000, i => i + 1);
        }
        
        [Test]
        public void Test_ToArray()
        {
            IEnumerable<int> enumerable = RunEnumerable<int>(_intList.Count, i => _intList[i]);
            
            int[] toArray = enumerable.ToArray();

            Assert.That(toArray.Length, Is.EqualTo(_intList.Count));
            for (int i = 0; i < _intList.Count; i++)
            {
                Assert.That(toArray[i], Is.EqualTo(_intList[i]), $"Expected {_intList[i]}, got {toArray[i]} at index {i}");
            }
        }

        [Test]
        public void Test_ToList()
        {
            IEnumerable<int> enumerable = RunEnumerable<int>(_intList.Count, i => _intList[i]);
            
            List<int> toList = IEnumerable.ToList(enumerable);

            Assert.That(toList.Count, Is.EqualTo(_intList.Count));
            for (int i = 0; i < _intList.Count; i++)
            {
                Assert.That(toList[i], Is.EqualTo(_intList[i]), $"Expected {_intList[i]}, got {toList[i]} at index {i}");
            }
        }


        [Test]
        public void Test_First()
        {
            int first = RunEnumerable(_intList.Count, i => _intList[i]).First();
            
            Assert.That(first, Is.EqualTo(_intList[0]));
        }
        [Test]
        public void Test_FirstEmpty()
        {
            List<int> emptyList = new List<int>();
            Assert.Catch(() => RunEnumerable(emptyList.Count, i => emptyList[i]).First());
        }


        [Test]
        public void Test_FirstOrDefault()
        {
            int first = RunEnumerable(_intList.Count, i => _intList[i]).FirstOrDefault();
            
            Assert.That(first, Is.EqualTo(_intList[0]));
        }
        [Test]
        public void Test_FirstOrDefaultEmpty()
        {
            List<int> emptyList = new List<int>();
            int first = RunEnumerable(emptyList.Count, i => emptyList[i]).FirstOrDefault();
            
            Assert.That(first, Is.EqualTo(0));
        }

        [Test]
        public void Test_Select()
        {
            const string findMe = "select";
            string[] test = { "one", "two", "three", "four", "five", findMe };
            bool[] expected = { false, false, false, false, false, true };
            bool[] selected = test.Select(i => i == findMe).ToArray();

            for (int i = 0; i < selected.Length; i++)
            {
                Assert.That(selected[i], Is.EqualTo(expected[i]));
            }
        }

        [Test]
        public void Test_SelectNotFound()
        {
            const string findMe = "select";
            string[] test = { "one", "two", "three", "four", "five" };
            bool[] expected = { false, false, false, false, false, false };
            bool[] selected = test.Select(i => i == findMe).ToArray();
            
            for (int i = 0; i < selected.Length; i++)
            {
                Assert.That(selected[i], Is.EqualTo(expected[i]));
            }
        }

        [Test]
        public void Test_Where()
        {
            const string findMe = "select";
            string[] test = { "one", "two", "three", "four", "five", findMe };
            string[] whered = test.Where(i => i == findMe).ToArray();
            
            Assert.That(whered.Length, Is.EqualTo(1));
            Assert.That(whered[0], Is.EqualTo(findMe));
        }

        [Test]
        public void Test_WhereNotFound()
        {
            const string findMe = "select";
            string[] test = { "one", "two", "three", "four", "five" };
            string[] whered = test.Where(i => i == findMe).ToArray();
            
            Assert.That(whered.Length, Is.EqualTo(0));
        }
        

        public List<T> BuildList<T>(int length, Func<int, T> builder)
        {
            List<T> utilsTests = new List<T>();
            for (int i = 0; i < length; i++)
            {
                utilsTests.Add(builder(i));
            }
            return utilsTests;
        }
        
        private IEnumerable<T> RunEnumerable<T>(int count, Func<int, T> setter)
        {
            for (int i = 0; i < count; i++)
            {
                yield return setter(i);
            }
        }
    }
}