using System.Collections.Generic;

namespace NeuralNet
{
	using Functional;
	namespace NNet
	{
		public class Sequential : INNBase
		{
			public List<INNBase> NeuralNetworks {get; private set;}
			//private readonly List<IActivation> Activations;
			public Sequential()
			{
				NeuralNetworks = new List<INNBase>();
				//Activations = new List<IActivation>();
			}
			public void Add(INNBase newNN)
			{
				NeuralNetworks.Add(newNN);
				//Activations.Add(act);
			}
			public float[] Forward(float[] x)
			{
				float[] output = NeuralNetworks[0].Forward(x);
				for (int i = 1; i < NeuralNetworks.Count; i++)
				{
					List<float> lo = new List<float>(output);
					// output = NeuralNetworks[i].Forward(lo.ToArray(), Activations[i]); 
					lo = new List<float>(NeuralNetworks[i].Forward(lo.ToArray()));
					output = lo.ToArray();                   
				}
				return output;
			}

			public int CountLayer() => NeuralNetworks.Count;
			//public INNBase GetLayer(int index) => NeuralNetworks[index];
			public void SetLayer(int index, INNBase layer) => NeuralNetworks[index] = layer;

			public void Mutate(float mutationRate)
			{
				
				for (int i = 0; i < NeuralNetworks.Count; i++)
				{
					NeuralNetworks[i].Mutate(mutationRate);
				}
			}

			public void CopyWeights(INNBase source)
			{
				Sequential src = (Sequential) source;
				for (int i = 0; i < NeuralNetworks.Count; i++)
				{
					NeuralNetworks[i].CopyWeights(src.NeuralNetworks[i]);
				}
			}

		}
	}
}
