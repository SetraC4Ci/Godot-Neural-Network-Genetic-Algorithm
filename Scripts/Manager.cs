using Godot;
using NeuralNet.NNet;
using NeuralNet.Functional;
using System.Collections.Generic;
using GeneticAlgorithm;

public class Manager : Node
{
	const string AgentPath = "res://Agent.tscn";
	[Export] private uint NPopulation = 240;
	private PackedScene AI_Scene;
	public List<Individual> Population {get; private set;}

	private Spatial spawnPoint;
	private bool spawnAgent = true;
	bool started = false;
	private Node mainScene;
	private Timer timer;
	private List<AIPlayerController> AgentList;
	private float lastBestScore;
	private int Generation = 1;
	private bool showTheBest = false;

	public override void _Ready()
	{
		AI_Scene = (PackedScene) GD.Load(AgentPath);
		Population = new List<Individual>();
		CreateNewPopulation(NPopulation);
		spawnPoint = (Spatial) GetNode("SpawnPoint");
		AgentList = new List<AIPlayerController>();
		mainScene = GetParent();
		timer = (Timer) mainScene.GetNode("Timer");
		NPopulation = 240;
		timer.WaitTime = 12;
	}

	public override void _Process(float delta)
	{
		Spawn(onlyTheBest : false);
		StartTimer();
		if (started == true && timer.TimeLeft <= 0)
		{
			DeleteAll();
			CreateNewGeneration();
			GD.Print("Spawning new Generation");
			spawnAgent = true;
			Spawn(onlyTheBest : false);
			started = false;
			StartTimer();
		}
		if(Input.IsActionJustPressed("show_best"))
		{
			ShowAll();
		}
	}

	public void CreateNewPopulation(uint nPopulation)
	{
		
		for (int i = 0; i < nPopulation; i++)
		{
			// Sequential model = new Sequential();
			// model.Add(new Layer(8, 16, Activations.Tanh));
			// model.Add(new Layer(16, 8, Activations.Tanh));
			// model.Add(new Layer(8, 4, Activations.Tanh));
			// model.Add(new Layer(4, 2, Activations.Tanh));
			// model.Mutate(2f);
			// Individual individual = new Individual(model);
			// individual.Mutate(.5f);
			// Population.Add(individual);
			RNN model = new RNN(8, 3, new uint[]{16, 8, 4, 2}, Activations.Tanh);
			Individual individual = new Individual(model);
			Population.Add(individual);
		}
	}

	public void DeleteAll()
	{
		for (int i = 0; i < NPopulation; i++)
		{
			AIPlayerController AgentA = (AIPlayerController) mainScene.GetNode(Generation.ToString() + "Agent" + i.ToString());
			AgentA.Free();
		}
		AgentList = new List<AIPlayerController>();
		AgentList.Clear();
	}

	public void Spawn(bool onlyTheBest = true)
	{
		if (spawnAgent)
		{
			int i = 0;
			// spawn one agent every 1 frame
			while (i < NPopulation)
			{
				AgentList.Add((AIPlayerController) AI_Scene.Instance());
				mainScene.AddChild(AgentList[i]);

				// setting the name of the agent 
				// then we can get it simply by name after
				AgentList[i].Name = Generation.ToString() + "Agent" +i.ToString();

				// setting the gene of the agent 
				// according to index in population
				AgentList[i].SetGenes(Population[i]);
				AgentList[i].Gene.SetFitness(0);
				AgentList[i].index = i;

				// Set The position of the Agent
				AgentList[i].GlobalTransform = spawnPoint.GlobalTransform;
				i++;
			}
			if(onlyTheBest)
			{
				for (int j = 1; j < AgentList.Count; j++)
				{
					AgentList[j].Visible = false;
				}
			}
			spawnAgent = false;	
		}			
	}

	public void StartTimer()
	{
		if(started == false)
		{
			timer.Start();
		}
		started = true;
	}

	public void CreateNewGeneration()
	{
		Population.Sort();
		Population.Reverse();

		GD.Print("Fittest : " + Population[0].Fitness);
		GD.Print("Baddest : " + Population[Population.Count - 1].Fitness);

		List<Individual> newPopulation = new List<Individual>();

		int bestNumber = (int) NPopulation / 8;

		// retaining the best in population for the next generation
		// the weights of the best to the bad NN
		for (int i = bestNumber; i < Population.Count; i++)
		{
			//GD.Print("Copying weights");
			Population[i].Model.CopyWeights(Population[i-bestNumber].Model);
		}
		// then mutate the bad NN
		for (int i = bestNumber; i < Population.Count; i++)
		{
			// GD.Print("Mutating");
			Population[i].Model.Mutate(.01f);
		}
		// if no evolution
		// mutate the 1/3 of the best
		if(Population[0].Fitness < (lastBestScore + 2f))
		{
			GD.Print("noEvo; Mutating");
			for (int i = bestNumber/3; i < bestNumber; i++)
			{
				Population[i].Model.Mutate(.01f);
			}
		}
		lastBestScore = Population[0].Fitness;	

		// reset fitness
		for (int i = 0; i < Population.Count; i++)
		{
			Population[i].SetFitness(0);
		}	

		// incrementing generation number
		Generation++;
	}
	
	public void ShowAll()
	{
		showTheBest = !showTheBest;
		if(showTheBest)
		{
			for (int j = 1; j < AgentList.Count; j++)
			{
				AgentList[j].Visible = false;
			}
		}
		else
		{
			for (int j = 0; j < AgentList.Count; j++)
			{
				AgentList[j].Visible = true;
			}
		}
	}

}
