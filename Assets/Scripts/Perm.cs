using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DuplicatePerm
{
    public static List<List<float>> numList = new List<List<float>>();
    public static void test(int[] a)
    {
        numList.Clear();
        //int[] a = { 4,4,0,2 };
        perm(a, 0);
        for (int i = 0; i < numList.Count; i++)
        {
            string tmp = "";
            for (int j = 0; j < numList[i].Count; j++)
			{
                tmp += numList[i][j] + " ";
			}
        
            
        }
    }

    public static int perm(int[] a, int begin)
    {
        if (begin == a.Length)
        {
            List<float> tmp = new List<float>();
            for (int i = 0; i < a.Length; i++)
            {
                tmp.Add(a[i]);
            }
            numList.Add(tmp);

            return 1;
        }

        int count = 0;
        for (int i = begin; i < a.Length; i++)
        {
            if (isSwap(a, begin, i))
            {
                swap(a, begin, i);
                count += perm(a, begin + 1);
                swap(a, begin, i);
            }
        }
        return count;
    }

    public static void swap(int[] a, int begin, int end)
    {
        int temp = a[begin];
        a[begin] = a[end];
        a[end] = temp;
    }

    public static bool isSwap(int[] a, int begin, int end)
    {
        for (int i = end; i > begin; i--)
        {
            if (a[end] == a[i - 1])
            {
                return false;
            }
        }
        return true;
    }

}