using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NHunspell;
using NUnit.Framework;

namespace hunspell_tr.Test
{
    [TestFixture]
    class SpellingTests
    {
        private Hunspell hunspell = HunspellHelper.HunspellTr;

        [TestCase("deneme", ExpectedResult = true)]
        public bool CorrectWords(string word)
        {
            return HunspellHelper.HunspellTr.Spell(word);
        }

    }
}
