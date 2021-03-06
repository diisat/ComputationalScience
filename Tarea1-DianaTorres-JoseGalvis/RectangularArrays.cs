﻿//----------------------------------------------------------------------------------------
//	Copyright © 2007 - 2018 Tangible Software Solutions, Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class includes methods to convert Java rectangular arrays (jagged arrays
//	with inner arrays of the same length).
//----------------------------------------------------------------------------------------
internal static class RectangularArrays
{
    public static ArrayList[][] ReturnRectangularArrayListArray(int size1, int size2)
    {
        ArrayList[][] newArray = new ArrayList[size1][];
        for (int array1 = 0; array1 < size1; array1++)
        {
            newArray[array1] = new ArrayList[size2];
        }

        return newArray;
    }
}