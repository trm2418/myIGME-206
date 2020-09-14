using System;

namespace WordSorter
{
    // Class: Program
    // Author: David Schuh and Tyler Machanic
    // Purpose: Unit Test 1 Question 2
    // Restrictions: None
    class Program
    {
        // the definition of the delegate function data type
        delegate string sortingFunction(string[] a);

        // Method: Main
        // Purpose: Get user input, create sorted and unsorted arrays, call sorting methods and print sorted words
        //          Output: Sorted list of words
        // Restrictions: None
        static void Main(string[] args)
        {
            // declare the unsorted and sorted arrays
            string[] aUnsorted;
            string[] aSorted;

            // declare the delegate variable which will point to the function to be called
            sortingFunction findHiLow;

        // a label to allow us to easily loop back to the start if there are input issues
        // not needed since input does not need to be parsed into a double
        //start:
            Console.WriteLine("Enter a list of space-separated words");

            // read the space-separated string of words
            string sWords = Console.ReadLine();

            // split the string into the an array of strings which are the individual words
            string[] sWord = sWords.Split(' ');

            // initialize the size of the unsorted array to 0
            int nUnsortedLength = 0;

            // iterate through the array of words strings
            foreach (string sThisWord in sWord)
            {
                // if the length of this string is 0 (ie. they typed 2 spaces in a row)
                if (sThisWord.Length == 0)
                {
                    // skip it
                    continue;
                }

                ++nUnsortedLength;
            }

            // now we know how many unsorted words there are
            // allocate the size of the unsorted array
            aUnsorted = new string[nUnsortedLength];

            // reset nUnsortedLength back to 0 to use as the index to store the words in the unsorted array
            nUnsortedLength = 0;
            foreach (string sThisWord in sWord)
            {
                // still skip the blank strings
                if (sThisWord.Length == 0)
                {
                    continue;
                }

                // store the value into the array
                aUnsorted[nUnsortedLength] = sThisWord;

                // increment the array index
                nUnsortedLength++;
            }

            // allocate the size of the sorted array
            aSorted = new string[nUnsortedLength];

            // prompt for <a>scending or <d>escending
            Console.Write("Ascending or Descending? ");
            string sDirection = Console.ReadLine();

            if (sDirection.ToLower().StartsWith("a"))
            {
                findHiLow = new sortingFunction(FindLowestValue);
            }
            else
            {
                findHiLow = new sortingFunction(FindHighestValue);
            }

            // start the sorted length at 0 to use as sorted index element
            int nSortedLength = 0;

            // while there are unsorted values to sort
            while (aUnsorted.Length > 0)
            {
                // store the lowest or highest unsorted value as the next sorted value
                aSorted[nSortedLength] = findHiLow(aUnsorted);

                // remove the current sorted value
                RemoveUnsortedValue(aSorted[nSortedLength], ref aUnsorted);

                // increment the number of values in the sorted array
                ++nSortedLength;
            }

            // write the sorted array of words
            Console.WriteLine("The sorted list is: ");
            foreach (string thisWord in aSorted)
            {
                Console.Write($"{thisWord} ");
            }

            Console.WriteLine();
        }

        // Method: FindLowestValue
        // Purpose: Find the lowest value in the array of strings
        //          Output: Lowest value
        // Restrictions: None
        static string FindLowestValue(string[] array)
        {
            // define return value
            string returnVal;

            // initialize to the first element in the array
            // (we must initialize to an array element)
            returnVal = array[0];

            // loop through the array
            foreach (string thisWord in array)
            {
                // if the current value is less than the saved lowest value
                if (thisWord.CompareTo(returnVal) < 0)
                {
                    // save this as the lowest value
                    returnVal = thisWord;
                }
            }

            // return the lowest value
            return (returnVal);
        }

        // Method: FindHighestValue
        // Purpose: Find the highest value in the array of strings
        //          Output: Highest value
        // Restrictions: None
        static string FindHighestValue(string[] array)
        {
            // define return value
            string returnVal;

            // initialize to the first element in the array
            // (we must initialize to an array element)
            returnVal = array[0];

            // loop through the array
            foreach (string thisWord in array)
            {
                // if the current value is greater than the saved highest value
                if (thisWord.CompareTo(returnVal) > 0)
                {
                    // save this as the highest value
                    returnVal = thisWord;
                }
            }

            // return the highest value
            return (returnVal);
        }

        // Method: RemoveUnsortedValue
        // Purpose: Remove the first instance of a value from an array
        // Restrictions: None
        static void RemoveUnsortedValue(string removeValue, ref string[] array)
        {
            // allocate a new array to hold 1 less value than the source array
            string[] newArray = new string[array.Length - 1];

            // we need a separate counter to index into the new array, 
            // since we are skipping a value in the source array
            int dest = 0;

            // the same value may occur multiple times in the array, so skip subsequent occurrences
            bool bAlreadyRemoved = false;

            // iterate through the source array
            foreach (string srcWord in array)
            {
                // if this is the word to be removed and we didn't remove it yet
                if (srcWord == removeValue && !bAlreadyRemoved)
                {
                    // set the flag that it was removed
                    bAlreadyRemoved = true;

                    // and skip it (ie. do not add it to the new array)
                    continue;
                }

                // insert the source word into the new array
                newArray[dest] = srcWord;

                // increment the new array index to insert the next word
                ++dest;
            }

            // set the ref array equal to the new array, which has the target word removed
            // this changes the variable in the calling function (aUnsorted in this case)
            array = newArray;
        }
    }
}
