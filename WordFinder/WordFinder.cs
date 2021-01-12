using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WordFinder
{
    public class WordFinder
    {
        private readonly IEnumerable<string> _matrix;
        private readonly IEnumerable<string> _transposedMatrix;

        public WordFinder(IEnumerable<string> matrix)
        {
            // Validate that the matrix is 64x64. If not the object can't be instatiated.
            if (matrix.Count() != 64) throw new Exception("Matrix should have 64 rows.");
            if (matrix.Any(x => x.Length != 64)) throw new Exception("Matrix should have 64 columns.");

            _matrix = matrix;

            // The matrix needs to be transposed in the constructor so this very demanding task is done only once.
            // Having the matrix transposed as a separate enumerable increases the finder performance.
            _transposedMatrix = TransposeMatrix(_matrix);
        }

        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {   
            // This dictionary keeps track of how many times a word has been found.
            var wordDictionary = new Dictionary<string, int>();

            foreach (var word in wordstream)
            {
                // Count the number of times the word appears in both the matrix and the transposed matrix.
                // Regex was chosen as the preferred way to find the words due to better performance.
                var wordCount = _matrix.Sum(x => Regex.Matches(x, word).Count) + _transposedMatrix.Sum(x => Regex.Matches(x, word).Count);

                // A word shouldn't appear twice in the results.
                if (!wordDictionary.ContainsKey(word))
                {
                    wordDictionary.Add(word, wordCount);
                }
            }

            // Return the top ten most found words in the matrix.
            return wordDictionary.OrderByDescending(x => x.Value).Take(10).Select(x => x.Key);
        }

        // This is the most performant way I could find to transpose the matrix while still having easy to read code.
        private IEnumerable<string> TransposeMatrix(IEnumerable<string> matrix)
        {
            var transposedMatrix = new List<string>();
            var enumerators = matrix.Select(e => e.GetEnumerator()).ToArray();
            try
            {
                while (enumerators.All(e => e.MoveNext()))
                {
                    transposedMatrix.Add(new string(enumerators.Select(e => e.Current).ToArray()));
                }
            }
            finally
            {
                Array.ForEach(enumerators, e => e.Dispose());
            }

            return transposedMatrix;
        }

        // This is another way to transpose the matrix which I like because I find it more readable. But in the end I'm not using it because it's not as performant.
        private IEnumerable<string> TransposeMatrixOtherMethod(IEnumerable<string> matrix)
        {
            return _matrix
                .SelectMany(row => row.Select((item, index) => new { item, index }))
                .GroupBy(i => i.index, i => i.item)
                .Select(g => new string(g.ToArray()));
        }
    }
}
