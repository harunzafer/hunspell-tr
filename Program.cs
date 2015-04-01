using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using hunspell_tr.Test;

namespace hunspell_tr
{
    internal static class Program
    {
        /// <summary>
        /// 
        /// </summary>
        private const string DataDir = @"..\..\..\data\";

        private const string DictDirectoryPath = @"..\..\..\dict\";
        private const string WordListPath = DataDir + "words.txt";
        private const string StemMapPath = DataDir + "stemMap.txt";
        private const string NewWordsListPath = DataDir + "wordsToAdd.txt";

        private const string version = "1.1-2014.12.11";

        [STAThread]
        private static void Main()
        {
            Process(
                addNewWords: false,
                createStemMapFile: false,
                generateDictionary: true,
                runTests: true);
        }

        private static void Process(
            bool addNewWords = false,
            bool createStemMapFile = false,
            bool generateDictionary = false,
            bool runTests = false)
        {
            var sw = new Stopwatch();
            sw.Start();

            if (addNewWords)
            {
                if (AddNewWordsToWordList())
                {
                    Console.WriteLine(@"New Word List Constructed: " + sw.Elapsed);    
                }
                else
                {
                    Console.WriteLine(@"undefined words in word list. Aborting...");
                    return;
                }
            }
            
            //Analyze and stem all the words from start or just use the created stems
            var wordStemMap = createStemMapFile ? CreateStemMapFile() : Stems.FromDictionary(StemMapPath);
            Console.WriteLine(@"Stem Map Constructed: " + sw.Elapsed);

            if (generateDictionary)
            {
                DictionaryGenerator.Generate(wordStemMap, DictDirectoryPath + "tr-TR", langCode:"tr-TR");
                Console.WriteLine(@"Dictionary Generated: " + sw.Elapsed);
            }

            if (runTests)
            {
                HunspellHelper.RunTests();
            }
        }

        private static IDictionary<string, string> CreateStemMapFile()
        {
            var words = File.ReadAllLines(WordListPath, Encoding.UTF8);
            var map = Stems.Stem(words);
            var list = map.Select(x => x.Key + "\t" + x.Value);
            File.WriteAllLines(StemMapPath, list, Encoding.UTF8);
            return map;
        }

        private static bool AddNewWordsToWordList()
        {
            var words = File.ReadAllLines(NewWordsListPath, Encoding.UTF8);
            
            if (Nuve.AreAllWordsValid(words) == false)
            {
                Console.WriteLine(@"Warning: Not all words in the new word list are correct");
                return false;
            }

            var unigrams = File.ReadAllLines(WordListPath, Encoding.UTF8).ToList();
            unigrams.AddRange(words);
            unigrams.Sort();

            File.WriteAllLines(WordListPath, unigrams.Distinct(), Encoding.UTF8);

            return true;
        }

        private static void EliminateWordsContainingNumber()
        {
            var unigrams = File.ReadAllLines(WordListPath, Encoding.UTF8).ToList();
            unigrams.RemoveAll(x => x.IndexOfAny("1234567890".ToCharArray()) > -1);
            File.WriteAllLines(WordListPath, unigrams.Distinct(), Encoding.UTF8);
        }

        private static void GenerateDistinctWordList(string input, string output)
        {
            var unigrams = Unigrams.Read(DataDir + input);
            var words = unigrams as string[] ?? unigrams.ToArray();
            File.WriteAllLines(DataDir + output, words, Encoding.UTF8);
        }

        private static void GenerateTestDictionary()
        {
            var map = Stems.FromDictionary(DataDir + "stemMapTest.txt");
            DictionaryGenerator.Generate(map, DictDirectoryPath + @"test", "test");
        }

        private static void SortAndEliminateDuplicates(string path)
        {
            var lines = File.ReadAllLines(path).ToList();
            lines.Sort();
            File.WriteAllLines(path, lines.Distinct());
        }
    }
}