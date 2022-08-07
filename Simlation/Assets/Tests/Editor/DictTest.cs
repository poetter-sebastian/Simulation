using System.Linq;
using NUnit.Framework;
using UnityEngine;
using World.Structure;

namespace Tests.Editor
{
    public class DictTest
    {
        [Test]
        public void IntDictAdd()
        {
            var dict = new SymmetricIntDict<int>
            {
                { (0, 1), 1 },
                { (1, 0), 0 },
                { (1, 1), 2 },
            };
            Assert.AreEqual(2, dict.Count);
            
            var dict2 = new SymmetricIntDict<int>
            {
                { 0, 1, 1 },
                { 1, 0, 0 },
                { 1, 1, 2 },
            };
            Assert.True(dict.Count == dict2.Count && !dict.Except(dict2).Any());
        }
        
        [Test]
        public void IntDictGet()
        {
            var dict = new SymmetricIntDict<int>
            {
                { (0, 1), 1 },
                { (1, 0), 0 },
                { (1, 1), 2 },
            };
            Assert.AreEqual(1, dict[(0,1)]);
        }
        
        [Test]
        public void IntDictSimpleGet()
        {
            var dict = new SymmetricIntDict<int>
            {
                { (0, 1), 1 },
                { (1, 0), 0 },
                { (1, 1), 2 },
            };
            Assert.AreEqual(1, dict[0,1]);
        }
        
        [Test]
        public void DictAdd()
        {
            var dict = new SymmetricVectorDict<int>
            {
                { (Vector3.zero, Vector3.up), 1 },
                { (Vector3.zero, Vector3.up), 0 },
                { (Vector3.zero, Vector3.right), 2 },
            };
            Assert.AreEqual(2, dict.Count);
            
            var dict2 = new SymmetricVectorDict<int>
            {
                { Vector3.zero, Vector3.up, 1 },
                { Vector3.zero, Vector3.up, 0 },
                { Vector3.zero, Vector3.right, 2 },
            };
            Assert.True(dict.Count == dict2.Count && !dict.Except(dict2).Any());
        }
        
        [Test]
        public void DictGet()
        {
            var dict = new SymmetricVectorDict<int>
            {
                { (Vector3.zero, Vector3.up), 1 },
                { (Vector3.zero, Vector3.left), 0 },
                { (Vector3.zero, Vector3.up), 2 },
            };
            Assert.AreEqual(1, dict[(Vector3.zero, Vector3.up)]);
        }
        
        [Test]
        public void DictSimpleGet()
        {
            var dict = new SymmetricVectorDict<int>
            {
                { (Vector3.zero, Vector3.up), 1 },
                { (Vector3.zero, Vector3.left), 0 },
                { (Vector3.zero, Vector3.up), 2 },
            };
            Assert.AreEqual(1, dict[Vector3.zero, Vector3.up]);
        }
    }
}