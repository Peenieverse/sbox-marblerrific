/// <summary>
/// TODO: Cleanup this mess
/// </summary>
public sealed class MarbleScript : Component
{
	[Property] private CameraComponent cameraComponent { get; set; }
	[Property] private GameObject forwardAxis { get; set; }
	[Property] private GameObject Camera { get; set; }
	[Property] private GameObject ActualCamera { get; set; }
	[Property] private Rigidbody rb { get; set; }
	[Property] private float Acceleration { get; set; }
	[Property] private float wormSpeed { get; set; }
	[Property] private float BrakeAcceleration { get; set; }
	[Property] private float JumpForce { get; set; }
	[Property] private float rayDis { get; set; }
	[Property] private bool AirControl { get; set; }
	[Property] private float raySize { get; set; }
	[Property] private float mouseSensitivity { get; set; } = 100f;
	[Property] private float forceMultForBetterConfig { get; set; } = 10000000f;
	[Property] private float cameraReturnLerp { get; set; } = 100f;
	[Property] private float Zrespwawn { get; set; } = -1000f;
	[Property] private float fovVelocityMult { get; set; } = 0.1f;
	[Property] private float fovVelocityLerp { get; set; } = 0.2f;
	public float mouseSensMult = 1f;
	float xRotation = 0f;
	float yRotation = 0f;
	[Property] private Vector2 CamClamp { get; set; }
	Vector3 cameraPosStart;
	float CamRayDis;
	GameObject respawnPoint;
	float startFov;
	[Property] public float wormMode { get; set; }
	[Property] public float wormSpeedLerp { get; set; }
	protected override void OnAwake()
	{
		cameraComponent.GameObject.Parent.SetParent( null );
		startFov = cameraComponent.FieldOfView;
		IEnumerable<GameObject> balls = Scene.GetAllObjects( true );
		foreach ( GameObject go in balls )
		{

			if ( go.Tags.Has( "respawnpoint" ) )
			{
				respawnPoint = go;
				break;
			}
		}
		cameraPosStart = ActualCamera.Transform.LocalPosition;
		CamRayDis = Vector3.DistanceBetween( ActualCamera.Transform.Position, Camera.Transform.Position );
	}
	public void Respawn( Vector3 point )
	{
		Transform.Position = point;
		rb.Velocity = Vector3.Zero;
	}
	Vector3 wormVel;
	protected override void OnUpdate()
	{
		//MOVEMENT SHIT
		var tr = Scene.Trace.Ray( forwardAxis.Transform.Position, forwardAxis.Transform.Position + (Vector3.Down * rayDis) ).IgnoreGameObject( GameObject ).Radius( raySize ).Run();
		if ( tr.Hit || AirControl )
		{
			Vector3 cameraForwardFlat = Camera.Transform.World.Forward;
			cameraForwardFlat.z = 0;
			Angles newRotation = Rotation.LookAt( cameraForwardFlat );
			forwardAxis.Transform.Rotation = newRotation;
			if ( Input.Pressed( "Jump" ) && tr.Hit ) rb.ApplyForce( forceMultForBetterConfig * Vector3.Up * JumpForce );
			if ( wormMode <= 0 )
			{
				rb.ApplyForce( forceMultForBetterConfig * Time.Delta * Acceleration * ((cameraForwardFlat * Input.AnalogMove.x) + (forwardAxis.Transform.World.Right * -Input.AnalogMove.y)) );
				Vector3 velocityExludingZ = rb.Velocity;
				velocityExludingZ.z = 0;
				if ( Input.Down( "Run" ) ) rb.ApplyForce( forceMultForBetterConfig * -velocityExludingZ * BrakeAcceleration * Time.Delta );
			}
			else
			{

				wormVel = Vector3.Lerp( wormVel, wormSpeed * ((cameraForwardFlat * Input.AnalogMove.x) + (forwardAxis.Transform.World.Right * -Input.AnalogMove.y)), Time.Delta * wormSpeedLerp );
				rb.Velocity = new Vector3( wormVel.x, wormVel.y, rb.Velocity.z );
			}
		}
		if ( wormMode > 0 ) wormMode -= Time.Delta;
		if ( Transform.Position.z <= Zrespwawn ) Respawn( respawnPoint.Transform.Position );



		//CAMERA SHIT
		Vector3 dir = (ActualCamera.Transform.Position - Camera.Transform.Position).Normal;
		var trt = Scene.Trace.Ray( Camera.Transform.Position, Camera.Transform.Position + (dir * CamRayDis) ).IgnoreGameObject( GameObject ).Run();
		if ( trt.Hit ) ActualCamera.Transform.Position = trt.HitPosition - (dir * 100f);
		else ActualCamera.Transform.LocalPosition = Vector3.Lerp( ActualCamera.Transform.LocalPosition, cameraPosStart, cameraReturnLerp * Time.Delta );
		float mouseX = Input.AnalogLook.yaw * mouseSensitivity * mouseSensMult * Time.Delta;
		float mouseY = Input.AnalogLook.pitch * mouseSensitivity * mouseSensMult * Time.Delta;
		xRotation += mouseY;
		xRotation = MathX.Clamp( xRotation, CamClamp.x, CamClamp.y );
		yRotation += mouseX;
		Camera.Transform.LocalRotation = new Angles( xRotation, yRotation, 0 );
		Camera.Transform.Position = Transform.Position;
		//FOV
		cameraComponent.FieldOfView = MathX.Lerp( cameraComponent.FieldOfView, startFov + ((rb.Velocity.Abs().z + rb.Velocity.Abs().y + rb.Velocity.Abs().z) * fovVelocityMult), Time.Delta * fovVelocityLerp );
	}
}
