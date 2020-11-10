using Godot;
using GeneticAlgorithm;
using System;

public class AIPlayerController : KinematicBody
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export] private readonly float movement_speed = 10f;
	//private float acceleration = 5f;
	[Export] private readonly float rotate_speed = 3f;
	public Individual Gene {get; private set;}
	private Spatial ArrivePoint;	
	Node ManagerNode;
	public int index;

	float distance;
	float lastdistance;

	Manager manager;

	RayCast FRay;
	private float reward = 1f;
	private float penality = 4f;
	private float bestdistance;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ManagerNode = GetParent();
		manager = (Manager) ManagerNode.GetNode("Manager");
		ArrivePoint = (Spatial) manager.GetNode("ArrivePoint");		
		FRay = (RayCast) GetNode("FRay");
		bestdistance = Translation.DistanceTo(ArrivePoint.GlobalTransform.origin);
	}



//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{

		// GEnerating inputs
		// The agent coord
		float Agentx = GlobalTransform.origin.x;
		float Agenty = GlobalTransform.origin.y;
		float Agentz = GlobalTransform.origin.z;

		// the destination coord
		float Arrivex = ArrivePoint.GlobalTransform.origin.x;
		float Arrivey = ArrivePoint.GlobalTransform.origin.y;
		float Arrivez = ArrivePoint.GlobalTransform.origin.z;

		// distance between the agent and the objectif
		distance = Translation.DistanceTo(ArrivePoint.GlobalTransform.origin);

		float[] inputs = new float[] {Agentx, Agenty, Agentz, Arrivex, Arrivey, Arrivez, distance, Gene.Fitness};


		float[] output = Gene.Model.Forward(inputs);
		//float y_mov = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		float y_mov = output[0];
		//float rotate = Input.GetActionStrength("ui_left") - Input.GetActionStrength("ui_right");
		float rotate = output[1];
		
		RotateY(rotate * rotate_speed * delta);
		Vector3 direction = new Vector3(0,0,1).Rotated(new Vector3(0,1,0), Rotation.y);
		Vector3 motion = direction * y_mov * delta * movement_speed;
		
		MoveAndCollide(motion);

		if (distance >= lastdistance)
		{
			Gene.SetFitness(Gene.Fitness - penality);
		}
		if (distance > bestdistance)
		{
			Gene.SetFitness(Gene.Fitness - penality);
		}
		if (distance < lastdistance)
		{
			Gene.SetFitness(Gene.Fitness + reward);
		}
		if (distance < bestdistance)
		{
			Gene.SetFitness(Gene.Fitness + (float)(reward*4f));
			bestdistance = distance;
		}
		if (FRay.Enabled && FRay.IsColliding())
		{
			StaticBody CollidedObject = (StaticBody) FRay.GetCollider();
			if(CollidedObject.Name == "ArrivePoint")
			{
				GD.Print(Name + "Arrive to Destination");
				Gene.SetFitness(Gene.Fitness + 10000f);
			}
		}

		lastdistance = distance;
		// decrementing fitness every to force agent to go the fastest possible
		Gene.SetFitness(Gene.Fitness - 0.5f);
		// copying the fitness of the agent to the source in manager
		manager.Population[index].SetFitness(Gene.Fitness);
		//
		//GD.Print(this.Gene.Equals(manager.Population[index]));
		
	}

	public void Delete() => Free();

	public void SetIndex(int i) => index = i;

	public void SetGenes(Individual newGene) => Gene = newGene;

	public override string ToString() => "blablabla";

}
