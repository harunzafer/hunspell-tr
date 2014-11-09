using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NHunspell;

namespace hunspell_tr.Test
{
    internal class HunspellHelper
    {
        private const string BaseDictPath = @"C:\Users\hrzafer\Desktop\workspace\hunspell-tr\dictionaries\";
        private const string BaseTestPath = @"C:\Users\hrzafer\Desktop\workspace\hunspell-tr\test\";
        private const string BaseDataPath = @"C:\Users\hrzafer\Desktop\workspace\hunspell-tr\test\";

        public static Hunspell HunspellTr = Create("hunspell-tr", "tr-TR");
        public static Hunspell TrSpell = Create("tr-spell", "tr-TR");
        public static Hunspell EnGb = Create("en-GB", "en-GB");
        public static Hunspell EnUs = Create("en-US", "en-US");

        public static Hunspell Create(string dictPath, string langCode)
        {
            var path = BaseDictPath + dictPath + "/" + langCode;
            return new Hunspell(path + ".aff", path + ".dic");
        }

        public static IEnumerable<string> GetCorrectWords()
        {
            return File.ReadAllLines(BaseTestPath + "hepsi-dogru.txt");
        }

        public static IEnumerable<string> GetStems(string word)
        {
            return HunspellTr.Stem(word);
        }

        public static void GetInfo(string word)
        {
            Console.WriteLine("Spelled correct:" + HunspellTr.Spell(word));
            var stems = HunspellTr.Stem(word);
            Console.WriteLine("Stems:");
            foreach (var stem in stems)
            {
                Console.WriteLine(stem);
            }

            var suggestions = HunspellTr.Suggest(word);
            Console.WriteLine("Suggestions:");
            foreach (var suggestion in suggestions)
            {
                Console.WriteLine(suggestion);
            }

            Console.WriteLine("Analyses:");
            var analyses = HunspellTr.Analyze(word);
            foreach (var analysis in analyses)
            {
                Console.WriteLine(analysis);
            }

        }

        public static IEnumerable<string> GetWrongWords()
        {
            return File.ReadAllLines(BaseTestPath + "hepsi-yanlis.txt");
        }

        /// <summary>
        /// Returns valid words that not recognized by the spellchecker
        /// </summary>
        /// <param name="hunspell">spellchecker</param>
        /// <param name="words"></param>
        /// <param name="correct"></param>
        /// <returns></returns>
        public static void TestWords(Hunspell hunspell, IEnumerable<string> words, bool correct)
        {
            var failed = correct ? 
                words.Where(word => !hunspell.Spell(word)).ToList() : 
                words.Where(word => hunspell.Spell(word)).ToList();
            
            var msg = correct ? "correct" : "wrong";

            if (failed.Any())
            {
                Console.WriteLine("Test for " + msg + " words failed for " + failed.Count + " words");
                foreach (var word in failed)
                {
                    Console.WriteLine(word);
                }
            }
            else
            {
                Console.WriteLine("Test for " + msg + " words passed:");
            }
        }

        public delegate IEnumerable<string> SpellcheckerTest(Hunspell hunspell, IEnumerable<string> words, bool correct);

        public static void RunTests()
        {
            Console.WriteLine("Hunspell-tr--------------------------: ");
            TestWords(HunspellTr, GetCorrectWords(), correct: true);
            TestWords(HunspellTr, GetWrongWords(), correct: false);

            Console.WriteLine("\nTr-spell---------------------------: ");
            TestWords(TrSpell, GetCorrectWords(), correct: true);
            TestWords(TrSpell, GetWrongWords(), correct: false);
        }
    }
}

