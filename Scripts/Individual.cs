using NeuralNet.NNet;
using System;

namespace GeneticAlgorithm
{
    public class Individual : IComparable<Individual>
    {
        public INNBase Model {get; private set;}
        public float Fitness {get; private set;} = 0f;
        public Individual(INNBase model) => Model = model;
        public Individual()
        {
            
        }
        public void ReplaceModel(INNBase newModel) => Model = newModel;
        public void SetFitness(float fit) => Fitness = fit;
        public void Mutate(float mutationRate) => Model.Mutate(mutationRate);
        public Individual CreateChild(float _mutationRate)
        {
            Individual child = new Individual(this.Model);
            child.Mutate(_mutationRate);
            return child;
        }
        public int CompareTo(Individual other)
        {
            if (other == null || Fitness > other.Fitness)
            {
                return 1;
            } 		
            else if (Fitness < other.Fitness)
            {
                return -1;
            }
            else
            {
                return 0;   
            }
        }
    }

}