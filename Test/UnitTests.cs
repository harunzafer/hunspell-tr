using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace hunspell_tr.Test
{
    [TestFixture]
    class UnitTests
    {
        [Test]
        public void Unigrams_Get()
        {
            IEnumerable<string> expected = new[] { "bu", "denemedir" };
            IEnumerable<string> actual = Unigrams.Get("bu 1 denemedir");       
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
