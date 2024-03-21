using Sandbox;

public sealed class LookAtTag : Component
{
	[Property] public string tag { get; set; }
	GameObject Target;
	protected override void OnStart()
	{
		IEnumerable<GameObject> SceneObjects = Scene.GetAllObjects(true);
		foreach(GameObject go in SceneObjects)
		{
			if(go.Tags.Has(tag))
			{
				Target = go;
			}
		}
	}

	protected override void OnUpdate()
	{
		Transform.Rotation = Rotation.LookAt(Target.Transform.Position-Transform.Position);
	}
}