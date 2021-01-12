using System;
using System.IO;

namespace WordFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            // You can edit both text files to test different words and matrices.
            // I generated my matrix using this tool http://www.unit-conversion.info/texttools/random-string-generator/
            // Then I add the words I want to finde to the matrix and to the word stream.
            var wordMatrix = File.ReadAllLines("wordmatrix.txt");
            var wordStream = File.ReadAllLines("wordstream.txt");

            var wordFinder = new WordFinder(wordMatrix);
            var topFoundWords = wordFinder.Find(wordStream);

            // Write the result to the console for testing purposes.
            // The result should show the top ten most found words from the word stream inside the matrix.
            Console.Write(string.Join(Environment.NewLine, topFoundWords));
        }
    }
}
