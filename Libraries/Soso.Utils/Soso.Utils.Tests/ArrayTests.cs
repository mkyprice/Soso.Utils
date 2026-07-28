using System;
using Soso.Utils.Helpers;
using Soso.Utils.Random;

namespace Soso.Utils.Tests;

public class ArrayTests
{
    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(4)]
    public void TestExtensionRemoveAt(int index)
    {
        int[] og = new int[5];
        int[] test = new int[5];
        for (int i = 0; i < og.Length; i++)
        {
            og[i] = i;
            test[i] = i;
        }

        test = test.RemoveAt(index);
        Assert.That(test.Length, Is.EqualTo(og.Length - 1));
        for (int i = 0; i < og.Length; i++)
        {
            if (i == index) continue;
            int testIndex = i < index ? i : i - 1;
            Assert.That(og[i], Is.EqualTo(test[testIndex]));
        }
    }
    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(4)]
    public void TestRemoveAt(int index)
    {
        int[] og = new int[5];
        int[] test = new int[5];
        for (int i = 0; i < og.Length; i++)
        {
            og[i] = i;
            test[i] = i;
        }

        ArrayUtils.RemoveAt(ref test, index);
        Assert.That(test.Length, Is.EqualTo(og.Length - 1));
        for (int i = 0; i < og.Length; i++)
        {
            if (i == index) continue;
            int testIndex = i < index ? i : i - 1;
            Assert.That(og[i], Is.EqualTo(test[testIndex]));
        }
    }
    
    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(5)]
    public void TestExtensionInsertAt(int index)
    {
        int insert = 999;
        
        const int LENGTH = 5;
        int[] og = new int[LENGTH];
        int[] test = new int[LENGTH];
        for (int i = 0; i < test.Length; i++)
        {
            og[i] = i;
            test[i] = i;
        }

        test = test.InsertAt(insert, index);
        
        Assert.That(test.Length, Is.EqualTo(LENGTH + 1));
        Assert.That(test[index], Is.EqualTo(insert));

        for (int i = 0; i < og.Length; i++)
        {
            int ogValue;
            int testValue;
            if (i < index)
            {
                ogValue = og[i];
                testValue = test[i];
            }
            else
            {
                ogValue = og[i];
                testValue = test[i + 1];
            }
            Assert.That(ogValue, Is.EqualTo(testValue));
        }
    }
    
    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(5)]
    public void TestInsertAt(int index)
    {
        int insert = 999;
        
        const int LENGTH = 5;
        int[] og = new int[LENGTH];
        int[] test = new int[LENGTH];
        for (int i = 0; i < test.Length; i++)
        {
            og[i] = i;
            test[i] = i;
        }

        ArrayUtils.InsertAt(ref test, insert, index);
        
        Assert.That(test.Length, Is.EqualTo(LENGTH + 1));
        Assert.That(test[index], Is.EqualTo(insert));

        for (int i = 0; i < og.Length; i++)
        {
            int ogValue;
            int testValue;
            if (i < index)
            {
                ogValue = og[i];
                testValue = test[i];
            }
            else
            {
                ogValue = og[i];
                testValue = test[i + 1];
            }
            Assert.That(ogValue, Is.EqualTo(testValue));
        }
    }
}