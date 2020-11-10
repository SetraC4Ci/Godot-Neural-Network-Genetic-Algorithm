using NeuralNet.Functional;
using System;

namespace NeuralNet
{
	namespace NNet
	{
		public interface INNBase
		{
			float[] Forward(float[] x);
			void Mutate(float mutationRate);
			void CopyWeights(INNBase source);
		}
	}
}
