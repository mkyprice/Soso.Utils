using System;
using Soso.Utils.Random;

namespace Soso.Utils.Tests;

public class RandomTests
{
    public static int TestIterations = 1000;
    
    [Test]
    public void TestSeed()
    {
        int seed = Environment.TickCount;
        
        SosoRandom r1 = new SosoRandom(seed);
        SosoRandom r2 = new SosoRandom(seed);

        for (int i = 0; i < TestIterations; i++)
        {
            Assert.That(r2.Next(0, int.MaxValue), Is.EqualTo(r1.Next(0, int.MaxValue)));
        }
    }
    
    [Test]
    [TestCase(-100, 100)]
    public void TestRangeInt(int min, int max)
    {
        SosoRandom r1 = new SosoRandom();

        for (int i = 0; i < TestIterations; i++)
        {
            int next = r1.Next(min, max);
            Assert.That(next, Is.InRange(min, max));
        }
    }
    
    [Test]
    [TestCase(-100, 100)]
    [TestCase(100, 1000)]
    [TestCase(0, 1)]
    public void TestRangeFloat(float min, float max)
    {
        SosoRandom r1 = new SosoRandom();

        for (int i = 0; i < TestIterations; i++)
        {
            float next = r1.Next(min, max);
            Assert.That(next, Is.InRange(min, max));
        }
    }
    
    [Test]
    [TestCase(-100, 100)]
    [TestCase(100, 1000)]
    [TestCase(0, 1)]
    public void TestRangeDouble(double min, double max)
    {
        SosoRandom r1 = new SosoRandom();

        for (int i = 0; i < TestIterations; i++)
        {
            double next = r1.Next(min, max);
            Assert.That(next, Is.InRange(min, max));
        }
    }
}