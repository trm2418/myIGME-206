using System;

namespace NumberSortv1
{
    class Program
    {
        delegate int SortingFunction(int[] array);
        static void Main(string[] args)
        {
            // declare the unsorted and sorted arrays
            int[] aUnsorted;
            int[] aSorted;

        // a label to allow us to easily loop back to the start if there are input issues
        start:
            Console.WriteLine("Enter a list of space-separated numbers");

            // read the space-separated string of numbers
            string sNumbers = Console.ReadLine();

            // split the string into the an array of strings which are the individual numbers
            string[] sNumber = sNumbers.Split(' ');

            // initialize the size of the unsorted array to 0
            int nUnsortedLength = 0;

            // a double used for parsing the current array element
            int nThisNumber;

            // iterate through the array of number strings
            foreach (string sThisNumber in sNumber)
            {
                // if the length of this string is 0 (ie. they typed 2 spaces in a row)
                if (sThisNumber.Length == 0)
                {
                    // skip it
                    continue;
                }

                try
                {
                    // try to parse the current string into a double
                    nThisNumber = int.Parse(sThisNumber);

                    // if it's successful, increment the number of unsorted numbers
                    ++nUnsortedLength;
                }
                catch
                {
                    // if an exception occurs
                    // indicate which number is invalid
                    Console.WriteLine($"Number #{nUnsortedLength + 1} is not a valid number.");

                    // loop back to the start
                    goto start;
                }
            }

            // now we know how many unsorted numbers there are
            // allocate the size of the unsorted array
            aUnsorted = new int[nUnsortedLength];

            // reset nUnsortedLength back to 0 to use as the index to store the numbers in the unsorted array
            nUnsortedLength = 0;
            foreach (string sThisNumber in sNumber)
            {
                // still skip the blank strings
                if (sThisNumber.Length == 0)
                {
                    continue;
                }

                // parse it into a double (we know they are all valid now)
                nThisNumber = int.Parse(sThisNumber);

                // store the value into the array
                aUnsorted[nUnsortedLength] = nThisNumber;

                // increment the array index
                nUnsortedLength++;
            }

            // allocate the size of the sorted array
            aSorted = new int[nUnsortedLength];

            // start the sorted length at 0 to use as sorted index element
            int nSortedLength = 0;

            Console.Write("Sort in <a>scending or <d>escending order: ");
            string sAscDesc = Console.ReadLine();


            ///************************************************************
            /// delegate steps
            /// 1. define the delegate method data type
            ///         delegate double MathFunction(double n1, double n2);
            /// 2. allocate the delegate method variable
            ///         MathFunction processDivMult;
            /// 3. point the variable to the method that it should call
            ///         processDivMult = new MathFunction(Multiply);
            /// 4. call the delegate method
            ///         nAnswer = processDivMult(n1, n2);
            ///************************************************************


            // 1. Create FindHighestValue() based on FindLowestValue()
            // 2. define the delegate method data type 
            //    called SortingFunction with the correct method signature

            // 3. declare a delegate method variable of type SortingFunction

            SortingFunction sortingFunction;



            if (sAscDesc.ToLower().StartsWith("a"))
            {
                // 4. if ascending, then set the delegate variable to FindLowestValue
                sortingFunction = new SortingFunction(FindLowestValue);
            }
            else
            {
                // 5. else set the delegate variable to FindHighestValue
                sortingFunction = new SortingFunction(FindHighestValue);
            }

            // while there are unsorted values to sort
            while (aUnsorted.Length > 0)
            {
                // 6. call the delegate method
                aSorted[nSortedLength] = sortingFunction(aUnsorted);

                RemoveUnsortedValue(aSorted[nSortedLength], ref aUnsorted);

                ++nSortedLength;
            }

            // write the sorted array of numbers
            Console.WriteLine("The sorted list is: ");
            foreach (double thisNum in aSorted)
            {
                Console.Write($"{thisNum} ");
            }

            Console.WriteLine();
        }

        // find the lowest value in the array of doubles
        static int FindLowestValue(int[] array)
        {
            // define return value
            int returnVal;

            // initialize to the first element in the array
            // (we must initialize to an array element)
            returnVal = array[0];

            // loop through the array
            foreach (int thisNum in array)
            {
                // if the current value is less than the saved lowest value
                if (thisNum < returnVal)
                {
                    // save this as the lowest value
                    returnVal = thisNum;
                }
            }

            // return the lowest value
            return (returnVal);
        }

        // find the highest value in the array of doubles
        static int FindHighestValue(int[] array)
        {
            // define return value
            int returnVal;

            // initialize to the first element in the array
            // (we must initialize to an array element)
            returnVal = array[0];

            // loop through the array
            foreach (int thisNum in array)
            {
                // if the current value is greater than the saved highest value
                if (thisNum > returnVal)
                {
                    // save this as the highest value
                    returnVal = thisNum;
                }
            }

            // return the highest value
            return (returnVal);
        }

        // remove the first instance of a value from an array
        static void RemoveUnsortedValue(int removeValue, ref int[] array)
        {
            // allocate a new array to hold 1 less value than the source array
            int[] newArray = new int[array.Length - 1];

            // we need a separate counter to index into the new array, 
            // since we are skipping a value in the source array
            int dest = 0;

            // the same value may occur multiple times in the array, so skip subsequent occurrences
            bool bAlreadyRemoved = false;

            // iterate through the source array
            foreach (int srcNumber in array)
            {
                // if this is the number to be removed and we didn't remove it yet
                if (srcNumber == removeValue && !bAlreadyRemoved)
                {
                    // set the flag that it was removed
                    bAlreadyRemoved = true;

                    // and skip it (ie. do not add it to the new array)
                    continue;
                }

                // insert the source number into the new array
                newArray[dest] = srcNumber;

                // increment the new array index to insert the next number
                ++dest;
            }

            // set the ref array equal to the new array, which has the target number removed
            // this changes the variable in the calling function (aUnsorted in this case)
            array = newArray;
        }
    }
}
