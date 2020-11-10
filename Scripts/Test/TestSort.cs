using System.Collections.Generic;
using System;


public class ToSort : IComparable<ToSort>
{
    private readonly int variable;

    public static Random random;

    public ToSort()
    {
        variable = random.Next();
    }

    public int CompareTo(ToSort other)
	{
		if (other == null) return 1;
		
		if (variable > other.variable)
			return 1;
		else if (variable < other.variable)
			return -1;
		else
			return 0;
	}
}
public class TestSort
{
    List<ToSort> ListToSort = new List<ToSort>();

    List<ToSort> Sorted = new List<ToSort>();

    public TestSort()
    {
        for (int i = 0; i < 10; i++)
        {
            ListToSort.Add(new ToSort());
        }
        Sort();
    }

    public void Sort()
    {
        ListToSort.Sort();
    }
}