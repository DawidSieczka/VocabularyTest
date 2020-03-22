using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Security.AccessControl;
using System.Threading;

namespace VocabularyTest
{
    static class Program
    {
        public static List<Vocabulary> _dictionary = new List<Vocabulary>();
        static string directoryPath = @"C:\Users\dawid\Desktop\VocabularyTest\";
        static int points;

        static void Main(string[] args)
        {
            string[] filePaths = Directory.GetFiles(directoryPath, "*.csv");
            DisplayFiles(filePaths);

            Console.WriteLine("\n\n\nChoice file:");
            
            string filePath = GetChosenFilePath(filePaths, GetSelectionInput());
            Console.Clear();

            FileMapping(filePath);
            
            Console.WriteLine("0 - exit \n1 - eng-pol  \n2 - pol-eng");
            Shuffle(_dictionary);
            Examinate(GetSelectionInput());

            Console.ReadLine();
        }

        private static string GetChosenFilePath(string[] filePaths,int choice)
        {
            string filePath = " ";
            
            try
            {
               filePath = filePaths[choice];
            }
            catch
            {
                Console.WriteLine("Wrong choice");
            }
            return filePath;

        }

        private static void DisplayFiles(string[] filePaths)
        {
            int number = 0;

            foreach (var item in filePaths)
            {
                Console.WriteLine(number++ +": "+item.Replace(directoryPath, "").Replace(".csv", ""));
            }
        }

        static void FileMapping(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                {


                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine().Trim();

                        var result = line.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                        string[] words = result[0].Split(';');

                        var _meaning = new Vocabulary()
                        {
                            wordA = words[0],
                            wordB = words[1]
                        };
                        _dictionary.Add(_meaning);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Reading file failed.\n{ex.Message}");
                Console.ReadLine();
                Environment.Exit(0);
            }
            
        }
        static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        static int GetSelectionInput()
        {
            string input = Console.ReadLine();
            int.TryParse(input, out int choice);

            if (choice == null)
                return 0;
            else
                return choice;
        }

        private static void Examinate(int choice)
        {
            Console.Clear();

            switch (choice)
            {
                case 1:
                    for (int i = 0; i < _dictionary.Count; i++)
                    {
                        Console.WriteLine(_dictionary[i].wordA);
                        string input = Console.ReadLine();
                        Console.Write(_dictionary[i].wordB);
                        Rate(input, _dictionary[i].wordB);
                        Console.ReadLine();
                        Console.WriteLine("------------------\n");
                    }
                    break;
                case 2:
                    for (int i = 0; i < _dictionary.Count; i++)
                    {
                        Console.WriteLine(_dictionary[i].wordB);
                        string input = Console.ReadLine();
                        Console.Write(_dictionary[i].wordA);
                        Rate(input, _dictionary[i].wordA);
                        Console.ReadLine();
                        Console.WriteLine("------------------\n");
                    }
                    break;

                default:
                    Console.WriteLine("Wrong choice");
                    break;
            }
        }
        static void Rate(string input, string correctAnswer)
        {
            input = input.ToLower();
            correctAnswer = correctAnswer.ToLower();
            if (string.Compare(input, correctAnswer) == 0)
                points++;
            Console.WriteLine($"\n\n{points}/{_dictionary.Count}");
        }
    }
    public class Vocabulary 
    {
        public string wordA { get; set; }
        public string wordB { get; set; }
    }
}
