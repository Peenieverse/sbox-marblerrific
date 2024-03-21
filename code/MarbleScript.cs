namespace Marblerrific;

public enum MoveMode
{
	Normal,
	Worm,
	Gravity
}

// Asphaltian: TODO, make this a bit more nicer
public sealed class MarbleScript : Component
{
	[Property] public MoveMode CurrentMode { get; set; }

	[Category( "References" ), Property] private CameraComponent CameraComponent { get; set; }
	[Category( "References" ), Property] private Rigidbody RigidBody { get; set; }
	[Category( "References" ), Property] public GameObject ForwardAxis { get; set; }
	[Category( "References" ), Property] public GameObject Camera { get; set; }
	[Category( "References" ), Property] public GameObject ActualCamera { get; set; }

	[Category( "Movement Stats (Normal)" ), Property] public float Acceleration { get; set; }
	[Category( "Movement Stats (Normal)" ), Property] public float BrakeAcceleration { get; set; }
	[Category( "Movement Stats (Normal)" ), Property] public float JumpForce { get; set; }
	[Category( "Movement Stats (Normal)" ), Property] public float RayDis { get; set; }
	[Category( "Movement Stats (Normal)" ), Property] public float ForceMultForBetterConfig { get; set; } = 10000000f;

	[Category( "Movement Stats (Worm)" ), Property] public float WormSpeedLerp { get; set; }
	[Category( "Movement Stats (Worm)" ), Property] public float WormSpeed { get; set; }

	[Category( "Camera Settings" ), Property] public float FovVelocityMult { get; set; } = 0.1f;
	[Category( "Camera Settings" ), Property] public float FovVelocityLerp { get; set; } = 0.2f;
	[Category( "Camera Settings" ), Property] public float MaxFov { get; set; } = 200f;
	[Category( "Camera Settings" ), Property] public Vector2 CamClamp { get; set; }
	[Category( "Camera Settings" ), Property] public float CameraReturnLerp { get; set; } = 100f;
	[Category( "Camera Settings" ), Property] public float RaySize { get; set; }
	[Category( "Camera Settings" ), Property] public float MouseSensitivity { get; set; } = 100f;

	private bool IsOnGround
	{
		get
		{
			return Scene.Trace.Ray( ForwardAxis.Transform.Position, ForwardAxis.Transform.Position + (Vector3.Down * RayDis) ).IgnoreGameObject( GameObject ).Radius( RaySize ).Run().Hit;
		}
	}

	private float xRotation = 0f;
	private float yRotation = 0f;
	private Vector3 cameraPosStart;
	private float camRayDis;
	private GameObject respawnPoint;
	private float startFov;
	private Vector3 wormVel;
	private Vector3 grav;

	protected override void OnAwake()
	{
		grav = Scene.PhysicsWorld.Gravity;
		CameraComponent.GameObject.Parent.SetParent( null );
		startFov = CameraComponent.FieldOfView;

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
		camRayDis = Vector3.DistanceBetween( ActualCamera.Transform.Position, Camera.Transform.Position );
	}

	public void Respawn( Vector3 point )
	{
		Transform.Position = point;
		RigidBody.Velocity = Vector3.Zero;
	}

	private void Jump()
	{
		RigidBody.ApplyForce( ForceMultForBetterConfig * Vector3.Up * JumpForce );
	}

	private void CameraControl()
	{
		Vector3 dir = (ActualCamera.Transform.Position - Camera.Transform.Position).Normal;

		// clip prevention
		var tr = Scene.Trace.Ray( Camera.Transform.Position, Camera.Transform.Position + (dir * camRayDis) ).IgnoreGameObject( GameObject ).Run();
		if ( tr.Hit )
			ActualCamera.Transform.Position = tr.HitPosition - (dir * 100f);
		else
			ActualCamera.Transform.LocalPosition = Vector3.Lerp( ActualCamera.Transform.LocalPosition, cameraPosStart, CameraReturnLerp * Time.Delta );

		// Mouse Move Stuff
		float mouseX = Input.AnalogLook.yaw * MouseSensitivity * Time.Delta;
		float mouseY = Input.AnalogLook.pitch * MouseSensitivity * Time.Delta;

		xRotation += mouseY;
		xRotation = MathX.Clamp( xRotation, CamClamp.x, CamClamp.y );
		yRotation += mouseX;

		// SetRotation And Position
		Camera.Transform.LocalRotation = new Angles( xRotation, yRotation, 0 );
		Camera.Transform.Position = Transform.Position;

		// FOV
		CameraComponent.FieldOfView = MathX.Clamp( MathX.Lerp( CameraComponent.FieldOfView, startFov + ((RigidBody.Velocity.Abs().z + RigidBody.Velocity.Abs().y + RigidBody.Velocity.Abs().z) * FovVelocityMult), Time.Delta * FovVelocityLerp ), 0, MaxFov );
	}

	private void WormMovement( Vector3 cameraForwardFlat )
	{
		wormVel = Vector3.Lerp( wormVel, WormSpeed * ((cameraForwardFlat * Input.AnalogMove.x) + (ForwardAxis.Transform.World.Right * -Input.AnalogMove.y)), Time.Delta * WormSpeedLerp );
		RigidBody.Velocity = new Vector3( wormVel.x, wormVel.y, RigidBody.Velocity.z );
	}

	private void NormalMovement( Vector3 cameraForwardFlat, bool AirControl )
	{
		if ( IsOnGround || AirControl )
		{
			RigidBody.ApplyForce( ForceMultForBetterConfig * Time.Delta * Acceleration * ((cameraForwardFlat * Input.AnalogMove.x) + (ForwardAxis.Transform.World.Right * -Input.AnalogMove.y)) );

			if ( Input.Down( "Run" ) )
				RigidBody.ApplyForce( ForceMultForBetterConfig * -RigidBody.Velocity.WithZ( 0 ) * BrakeAcceleration * Time.Delta );
		}
	}

	protected override void OnUpdate()
	{
		CameraControl();

		// TROLLFACEINREALLIFE: Setting the Vector Directions and Forward Axis 
		Vector3 cameraForwardFlat = Camera.Transform.World.Forward.WithZ( 0 );
		ForwardAxis.Transform.Rotation = Rotation.LookAt( cameraForwardFlat );

		if ( CurrentMode != MoveMode.Gravity )
		{
			Scene.PhysicsWorld.Gravity = grav;
		}

		if ( CurrentMode == MoveMode.Normal )
		{
			if ( Input.Pressed( "Jump" ) && IsOnGround )
				Jump();

			NormalMovement( cameraForwardFlat, false );
		}
		else if ( CurrentMode == MoveMode.Worm )
		{
			if ( Input.Pressed( "Jump" ) && IsOnGround )
				Jump();

			WormMovement( cameraForwardFlat );
		}
		else if ( CurrentMode == MoveMode.Gravity )
		{
			if ( Input.Pressed( "Jump" ) )
				Scene.PhysicsWorld.Gravity *= -1;

			NormalMovement( cameraForwardFlat, true );
		}
	}
}
