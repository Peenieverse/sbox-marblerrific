using Sandbox;

public sealed class PickupSpawner : Component
{
	[Property] private float RespawnTime {get;set;}
	[Property] private List<Vector3> spawnBox {get;set;}
	[Property] private GameObject Pickup {get;set;}

	int spawnCount;
	protected override void OnStart()
	{
		spawnCount = GameObject.Children.Count;
	}
	Vector3 RandomVector3(Vector3 a, Vector3 b)
	{
		Vector3 rt = new Vector3(0,0,0);
		rt.x = a.x+(b.x-a.x)*(Game.Random.Next(0,1000)/1000);
		rt.y = a.y+(b.y-a.y)*(Game.Random.Next(0,1000)/1000);
		rt.z = a.z+(b.z-a.z)*(Game.Random.Next(0,1000)/1000);
		return rt;
	}
	bool spawning;
	protected override async void OnUpdate()
	{
		if(GameObject.Children.Count<spawnCount && !spawning)
		{
			spawning = true;
			await Task.DelaySeconds(RespawnTime);
			GameObject newPickup = Pickup.Clone();
			newPickup.SetParent(GameObject);
			newPickup.Transform.LocalPosition = RandomVector3(spawnBox[0],spawnBox[1]);
			spawning=false;
		}
	}
}