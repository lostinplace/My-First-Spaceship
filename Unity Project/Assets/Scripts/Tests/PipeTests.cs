using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

namespace Tests
{
    public class PipeTests
    {
        
        [Test]
        public void PipeTestsSimplePasses()
        {
            var go = new GameObject("MyGameObject");
            go.AddComponent<Pipe>();
            var pipe = go.GetComponent<Pipe>();

            Assert.False(pipe.isBroken);
        }

        [Test]
        public void CalculateDamageSeemsReasonable()
        {
            var go = new GameObject("MyGameObject");
            go.AddComponent<Pipe>();
            var pipe = go.GetComponent<Pipe>();
            var results = Enumerable.Repeat(0, 100).Select(_ => Pipe.GetDamageForHeat(10)).ToList();
            var greaterThan10 = results.Where(x => x > 10).Count();
            


            Assert.Less(greaterThan10, 10);


            results = Enumerable.Repeat(0, 100).Select(_ => Pipe.GetDamageForHeat(100)).ToList();
            greaterThan10 = results.Where(x => x > 10).Count();

            Assert.Less(10, greaterThan10);

        }

    }
}
