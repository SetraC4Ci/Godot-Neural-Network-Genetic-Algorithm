using System.Collections.Generic;
using Godot;
using System;

namespace NeuralNet
{
	using Functional;
	namespace NNet
	{
		public class Neuron
		{
			
			protected float[] weights;
			protected float output;
			protected float bias;

			public Neuron(int n_inputs, Random rand, float _bias = 0)
			{
				List<float> listWeights = new List<float>();
				for (int i = 0; i < n_inputs; i++)
				{
					float weight = (float) rand.NextDouble(-1f, 1f);
					//Godot.GD.Print(weight.ToString());
					listWeights.Add(weight);
				}
				weights = listWeights.ToArray();
				this.bias = _bias;
			}

			public virtual float Activate(float[] inputs, IActivation act) 
			{
				float preactivation = 0f;
				for (int i = 0; i < inputs.Length; i++)
				{
					preactivation += inputs[i] * weights[i];
				}
				preactivation += bias;
				output = act.Activate(preactivation);
				return output;
			}

			public void Mutate(float mutationProb, Random rand)
			{
				for (int i = 0; i < weights.Length; i++)
				{
					float randfChance = (float) rand.NextDouble();
					if(randfChance <= mutationProb)
					{
						float oldweight = weights[i];
						weights[i] = (float) rand.NextDouble(-1f, 1f);
						//SetWeight(i, (float) rand.NextDouble(-1f, 1f));
					}
				}				
			}
			public void SetWeight(int index, float value) => weights[index] = value;
			public float GetWeight(int index) => weights[index];

			public void CopyWeights(Neuron source)
			{
				for (int i = 0; i < weights.Length; i++)
				{
					weights[i] = source.weights[i];
				}
			}
		}
	}
}
