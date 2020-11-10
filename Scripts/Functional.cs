using System;
namespace NeuralNet
{
	namespace Functional
	{
		public interface IActivation
		{
			float  Activate(float n);
		}
		public class Tanh : IActivation
		{
			public float Activate(float n){
				return (float) Math.Tanh(n);
			}
		}

		public class Sigmoid : IActivation
		{
			public float Activate(float n)
			{
				return 0f;
			}
		}
		public class Relu : IActivation
		{
			public float Activate(float n)
			{
				return 0f;
			}
		}

		public static class Activations
		{
			public static IActivation Tanh = new Tanh();
			public static IActivation Sigmoid = new Sigmoid();
			public static IActivation Relu = new Relu();
		}
	}
}
