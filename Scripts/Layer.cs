using System.Collections.Generic;
//using Godot;
using System;

namespace NeuralNet
{
	using Functional;
	namespace NNet
	{
		public class Layer : INNBase
		{
			protected Neuron[] neurons;
			protected float[] outputs;
			protected uint nOutput;
			protected static Random random;
			protected IActivation activation;
			public Layer(uint n_inputs, uint n_outputs, IActivation act)
			{
				activation = act;
				random = new Random();
				List<Neuron> neuronsList = new List<Neuron>();
				for (int i = 0; i < n_outputs; i++)
				{
					neuronsList.Add(new Neuron((int) n_inputs, random, 0));
				}
				this.neurons = neuronsList.ToArray();
				this.outputs = new float[n_outputs];
			}
			public Neuron[] GetNeurons() => neurons;
			public uint GetNOutput() => nOutput;
			public float[] GetOutputs() => outputs;
			public Neuron GetNeuron(int index) => neurons[index];
			public int GetNeuronNumber() => neurons.Length;
			public void SetNeurons(Neuron[] _neurons) => this.neurons = _neurons;
			public void SetActivation(IActivation act) => this.activation = act;
			public void SetWeight(int neuronIndex, int weightIndex, float value) => this.neurons[neuronIndex].SetWeight(weightIndex, value);
			
			// Mutate All Neurons in the Layer
			public void Mutate(float mutationRate)
			{
				//Godot.GD.Print("CALLED MUTATION OF LAYER");
				for (int i = 0; i < neurons.Length; i++)
				{
					neurons[i].Mutate(mutationRate, random);
				}
			}
			public static void CopyLayer(Layer source, Layer destination) => destination.SetNeurons(source.GetNeurons());
			public float[] Forward(float[] x)
			{
				for (int i = 0; i < this.outputs.Length; i++)
				{
					this.outputs[i] = this.neurons[i].Activate(x, activation);
				}
				return this.outputs;
			}

			public void CopyWeights(INNBase source)
			{
				Layer src = (Layer) source;
				for (int i = 0; i < neurons.Length; i++)
				{
					neurons[i].CopyWeights(src.neurons[i]);
				}
			}
		}
	}
}
