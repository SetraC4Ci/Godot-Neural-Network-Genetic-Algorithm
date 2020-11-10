using System.Collections.Generic;
using Godot;

namespace NeuralNet
{
	using Functional;
	using Utility;
	namespace NNet
	{
		public class RNN : INNBase
		{
			private readonly uint n_memory;
			private float[,] mem_stack;
			private float[] flat_mem_stack;
			//private Layer h_layer;
			//private Layer o_layer;
			private readonly Sequential h_layers;
			private readonly IActivation activation; // output layer activation function
			private readonly uint n_outputs;
			public RNN(uint n_inputs, uint n_mem, uint[] shape, IActivation act)
			{
				n_memory = n_mem;
				n_outputs = shape[shape.Length - 1];
				mem_stack = new float[n_memory,n_outputs+n_inputs];
				flat_mem_stack = Utils.FZerosArray(mem_stack);
				activation = act;
				//IActivation Tanh = new Tanh();

				h_layers = new Sequential();
				h_layers.Add(new Layer((uint) (n_inputs + flat_mem_stack.Length), shape[0], Activations.Tanh));
				//h_layers[0] = new Layer((uint) (n_inputs + flat_mem_stack.Length), hidden_dim);
				for (int i = 1; i < shape.Length; i++)
				{
					//h_layers[i] = new Layer(hidden_dim, hidden_dim);
					h_layers.Add(new Layer(shape[i -1], shape[i], Activations.Tanh));
				}

				// Setting the output layer activation to the given activation
				
				Layer lastLayer = (Layer) h_layers.NeuralNetworks[h_layers.NeuralNetworks.Count -1];
				lastLayer.SetActivation(activation);
				h_layers.NeuralNetworks[h_layers.CountLayer() - 1] = lastLayer;
				
				this.n_outputs = shape[shape.Length -1];            
			}

			public float[] Forward(float[] x)
			{
				flat_mem_stack = Utils.FlattenArray(mem_stack);
				
				float[] inputPlusMemory = Utils.ConcatArray(x, flat_mem_stack);				
				
				float[] output = h_layers.Forward(inputPlusMemory);
				
				// setting the mem Stack
				float[,] mem2 = new float[n_memory,n_outputs+x.Length];
				
				for (int i = 0; i < n_memory; i++)
				{
					for (int j = 0; j < n_outputs+x.Length; j++)
					{
						try {
							mem2[i+1,j] = mem_stack[i,j];
						}
						catch(System.Exception){
							break;
						}
					}
				}
				float[] outputPlusInput = Utils.ConcatArray(x, output);
				for (int i = 0; i < outputPlusInput.Length; i++)
				{
					mem2[0,i] = outputPlusInput[i];
				}
				
				mem_stack = mem2;
				
				return output;
			}


			/******TODO

			public Layer GetHLayer(int LayerIndex) => h_layers[LayerIndex];
			public void SetHLayer(int LayerIndex, Layer set) => h_layers[LayerIndex] = set;

			public void MutateAllLayer(float mutationProb)
			{
				for (int i = 0; i < h_layers.Length; i++)
				{
					h_layers[i].MutateAllNeurons(mutationProb);
				}
			}
			*******/

			public void Mutate(float mutationRate)
			{
				for (int i = 0; i < h_layers.CountLayer(); i++)
				{
					h_layers.Mutate(mutationRate);
				}
			}

			public void CopyWeights(INNBase source)
			{
				RNN src = (RNN) source;
				h_layers.CopyWeights(src.h_layers);
			}
		}
	}
}
