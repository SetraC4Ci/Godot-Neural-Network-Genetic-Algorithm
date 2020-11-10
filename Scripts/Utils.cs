using System.Collections.Generic;
//using Godot;

namespace NeuralNet
{
	namespace Utility
	{
		public static class Utils
		{
			public static float[] ZerosArray(float[] array)
			{
				float[] a = new float[array.Length - 1];
				for (int i = 0; i < array.Length; i++)
				{
					a[i] = 0;
				}
				return a;
			}
			public static float[] FZerosArray(float[,] array)
			{
				float[] a = new float[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					a[i] = 0;
				}
				return a;
			}
	
			public static int[] ZerosArray(int[] array)
			{
				int[] a = new int[array.Length - 1];
				for (int i = 0; i < array.Length; i++)
				{
					a[i] = 0;
				}
				return a;
			}
			public static float[] FlattenArray(float[,] array)
			{
				List<float> retarr = new List<float>();
				foreach (float item in array)
				{
					retarr.Add(item);
				}
				return retarr.ToArray();
			}        
			public static float[] ConcatArray(float[] a, float[] b)
			{
				//GD.Print("A:" + a.Length.ToString());
				//GD.Print("B:" + b.Length.ToString());
				
				
				float[] c = new float[a.Length + b.Length];
				//GD.Print("B:" + c.Length.ToString());
				for (int i = 0; i < a.Length; i++)
				{
					//GD.Print("iteration : " + i.ToString());
					c[i] = a[i];
				}
				for (int i = 0; i < b.Length; i++)
				{
					int iteration = i+a.Length;
					//GD.Print("iteration : " + iteration.ToString());
					c[iteration] = b[i];
				}
				return c;
			}
		}   
	} 
}
