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
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        const string DataDir = @"C:\Users\hrzafer\Desktop\workspace\hunspell-tr\data\";
        const string DictDir = @"C:\Users\hrzafer\Desktop\workspace\hunspell-tr\dictionaries\";
        private const string CompleteWordListPath = DataDir + "complete_word_list.txt";
        private const string CompleteStemMapPath = DataDir + "complete_stem_map.txt";
        private const string NewWordListPath = DataDir + "new_word_list.txt";
        
        [STAThread]
        static void Main()
        {            
            
            Process(addNewWords:false,
                createStemMapFile:false,
                generateDictionary:false);

            //HunspellHelper.RunTests();
            
            //SortAndEliminateDuplicates(@"C:\Users\hrzafer\Desktop\workspace\hunspell-tr\test\hepsi-yanlis.txt");
            //var stems = HunspellHelper.GetStems("saatım");
            //foreach (var stem in stems)
            //{
            //    Console.WriteLine(stem);
            //}
            //HunspellHelper.GetInfo("cenke");
            //GenerateTestDictionary();
            
            //Yap();

            //var unigrams = Unigrams.Read(@"C:\Users\hrzafer\Desktop\workspace\words.txt");
            //var words = unigrams as string[] ?? unigrams.ToArray();
            //File.WriteAllLines(@"C:\Users\hrzafer\Desktop\workspace\words.txt", words, new UTF8Encoding(false));

        }

        public static void Yap()
        {
            var lines = File.ReadAllLines(@"C:\Users\hrzafer\Desktop\workspace\Damla\code\zemberek3-exsample\unigrams.txt");
            var stems = new Dictionary<string, int>();
            foreach (var line in lines)
            {
                
                var row = line.Split('\t');
                var word = row[0];
                var freq = Int32.Parse(row[1]);
                string stem;
                if (Stems.TryStem(word, out stem))
                {
                    if (stems.ContainsKey(stem))
                    {
                        stems[stem] += freq;
                    }
                    else
                    {
                        stems.Add(stem, freq);
                    }
                }                
            }
            var list = stems.ToList();
            list.Sort((first, next) => next.Value.CompareTo(first.Value));

            var text = list.Select(x => x.Key + "\t" + x.Value);
            
            File.WriteAllLines(@"C:\Users\hrzafer\Desktop\workspace\stems2.txt", text);
        }

        public static void Process(bool addNewWords=false, 
            bool createStemMapFile=false,
            bool generateDictionary=false)
        {
            var sw = new Stopwatch();
            sw.Start();
            if (addNewWords)
            {
                AddNewWordsToCompleteWordList();
                Console.WriteLine("New Word List Constructed: " + sw.Elapsed);
            }

            var map = createStemMapFile ? CreateStemMapFile() : Stems.FromDictionary(CompleteStemMapPath);                     
            Console.WriteLine("Stem Map Constructed: " + sw.Elapsed);

            if (generateDictionary)
            {
                DictionaryGenerator.Generate(map, DictDir + @"hunspell-tr", "tr-TR");
                Console.WriteLine("Dictionary Generated: " + sw.Elapsed);    
            }
            
            HunspellHelper.RunTests();
        }

        public static string SubstringJava(this string s, int start, int end)
        {
            return s.Substring(start, end - start);
        }

        public static IDictionary<string, string> CreateStemMapFile()
        {
            var words = File.ReadAllLines(CompleteWordListPath, Encoding.UTF8);
            var map = Stems.Stem(words);
            var list = map.Select(x => x.Key + "\t" + x.Value);
            File.WriteAllLines(CompleteStemMapPath, list, Encoding.UTF8);
            return map;
        }

        public static void AddNewWordsToCompleteWordList()
        {
            var words = File.ReadAllLines(NewWordListPath, Encoding.UTF8);
            var unigrams = File.ReadAllLines(CompleteWordListPath, Encoding.UTF8).ToList();
            unigrams.AddRange(words);
            unigrams.Sort();
            File.WriteAllLines(CompleteWordListPath, unigrams.Distinct(), Encoding.UTF8);
        }

        public static void EliminateWordsContainingNumber()
        {
            var unigrams = File.ReadAllLines(CompleteWordListPath, Encoding.UTF8).ToList();
            unigrams.RemoveAll(x => x.IndexOfAny("1234567890".ToCharArray()) > -1);
            File.WriteAllLines(CompleteWordListPath, unigrams.Distinct(), Encoding.UTF8);
        }

        public static void GenerateDistinctWordList(string input, string output)
        {
            var unigrams = Unigrams.Read(DataDir + input);
            var words = unigrams as string[] ?? unigrams.ToArray();
            File.WriteAllLines(DataDir + output, words, Encoding.UTF8);
            
        }

        public static void GenerateTestDictionary()
        {
            var map = Stems.FromDictionary(DataDir + "test_stem_map.txt");
            DictionaryGenerator.Generate(map, DictDir + @"test", "test");
        }

        public static void SortAndEliminateDuplicates(string path)
        {
            var lines = File.ReadAllLines(path).ToList();
            lines.Sort();
            File.WriteAllLines(path, lines.Distinct());
        }
    }
}
