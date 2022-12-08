using System;

public class HelperRandomNumbers
{
    private int[] _randomNumbers;

    public HelperRandomNumbers(int countNumbers)
    {
        if(countNumbers > 0)
        {
            _randomNumbers = new int[countNumbers];

            FillArray(_randomNumbers);

            ShuffleArray(_randomNumbers);
        }
        else
        {
            throw new ArgumentException("Checked value, that you pass an argument!");
        }
    }

    public int this[int i]
    {
        get
        {
            return _randomNumbers[i];
        }
    }

    private void ShuffleArray(int[] array)
    {
        int tmp;

        for(int i = 0; i < array.Length; i++)
        {
            tmp = array[i]; 

            int r = UnityEngine.Random.Range(i, array.Length);

            array[i] = array[r];
            array[r] = tmp;
        }
    }

    private void FillArray(int[] array)
    {
        int i = 0;

        while(i < array.Length)
        {
            array[i] = i;
            i++;
        }
    }
}