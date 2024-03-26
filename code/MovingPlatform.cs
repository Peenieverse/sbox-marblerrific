using Sandbox;

public sealed class MovingPlatform : Component
{
	[Property] private bool Reverse {get;set;}
	[Property] private float Speed {get;set;}
	List<Vector3> posPoints;
	List<Angles> rotPoints;
	int targetIndex;
	int lastIndex;
	int direction;
	protected override void OnStart()
	{
		direction = 1;
		posPoints = new List<Vector3>();
		rotPoints = new List<Angles>();
		for(int i = 0; i < GameObject.Children.Count; i++)
		{
			posPoints.Add(GameObject.Children[i].Transform.Position);
			rotPoints.Add(GameObject.Children[i].Transform.Rotation.Angles());
		}
	}
	float t;
	protected override void OnPreRender()
	{
		if(t*Speed>=1)
		{
			t=0;
			lastIndex = targetIndex;
			if(targetIndex+1 >= posPoints.Count)
			{
				if(Reverse) direction = -1;
				else targetIndex = -1;
			}
			else if (targetIndex-1 < 0)
			{
				if(Reverse) direction = 1;
			}
			targetIndex+=direction;
		}

		Transform.Position = Vector3.Lerp(posPoints[lastIndex],posPoints[targetIndex],t*Speed);

		t+=Time.Delta;
	}
}