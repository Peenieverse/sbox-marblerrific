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

	[Category( "Component & Object References" ), Property] private CameraComponent cameraComponent { get; set; }
	[Category( "Component & Object References" ), Property] private GameObject forwardAxis { get; set; }
	[Category( "Component & Object References" ), Property] private GameObject Camera { get; set; }
	[Category( "Component & Object References" ), Property] private GameObject ActualCamera { get; set; }
	[Category( "Component & Object References" ), Property] private Rigidbody rb { get; set; }

	[Category( "Movement Stats Normal" ), Property] private float Acceleration { get; set; }
	[Category( "Movement Stats Normal" ), Property] private float BrakeAcceleration { get; set; }
	[Category( "Movement Stats Normal" ), Property] private float JumpForce { get; set; }
	[Category( "Movement Stats Normal" ), Property] private float rayDis { get; set; }
	[Category( "Movement Stats Normal" ), Property] private float forceMultForBetterConfig { get; set; } = 10000000f;
	[Category( "Movement Stats Normal" ), Property] private float Zrespwawn { get; set; } = -1000f;

	[Category( "Movement Stats Worm" ), Property] private float wormSpeedLerp { get; set; }
	[Category( "Movement Stats Worm" ), Property] private float wormSpeed { get; set; }

	[Category( "Camera Settings" ), Property] private float fovVelocityMult { get; set; } = 0.1f;
	[Category( "Camera Settings" ), Property] private float fovVelocityLerp { get; set; } = 0.2f;
	[Category( "Camera Settings" ), Property] private float maxFov { get; set; } = 200f;
	[Category( "Camera Settings" ), Property] private Vector2 CamClamp { get; set; }
	[Category( "Camera Settings" ), Property] private float cameraReturnLerp { get; set; } = 100f;
	[Category( "Camera Settings" ), Property] private float raySize { get; set; }
	[Category( "Camera Settings" ), Property] private float mouseSensitivity { get; set; } = 100f;

	[Property] public float wormMode { get; set; }

	bool IsOnGround
	{
		get
		{
			return Scene.Trace.Ray( forwardAxis.Transform.Position, forwardAxis.Transform.Position + (Vector3.Down * rayDis) ).IgnoreGameObject( GameObject ).Radius( raySize ).Run().Hit;
		}
	}

	float xRotation = 0f;
	float yRotation = 0f;
	Vector3 cameraPosStart;
	float CamRayDis;
	GameObject respawnPoint;
	float startFov;
	Vector3 wormVel;
	Vector3 grav;

	protected override void OnAwake()
	{
		grav = Scene.PhysicsWorld.Gravity;
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

	void Jump()
	{
		rb.ApplyForce( forceMultForBetterConfig * Vector3.Up * JumpForce );
	}

	void CameraControl()
	{
		Vector3 dir = (ActualCamera.Transform.Position - Camera.Transform.Position).Normal;

		// clip prevention
		var tr = Scene.Trace.Ray( Camera.Transform.Position, Camera.Transform.Position + (dir * CamRayDis) ).IgnoreGameObject( GameObject ).Run();

		if ( tr.Hit )
			ActualCamera.Transform.Position = tr.HitPosition - (dir * 100f);
		else
			ActualCamera.Transform.LocalPosition = Vector3.Lerp( ActualCamera.Transform.LocalPosition, cameraPosStart, cameraReturnLerp * Time.Delta );

		// Mouse Move Stuff
		float mouseX = Input.AnalogLook.yaw * mouseSensitivity * Time.Delta;
		float mouseY = Input.AnalogLook.pitch * mouseSensitivity * Time.Delta;

		xRotation += mouseY;
		xRotation = MathX.Clamp( xRotation, CamClamp.x, CamClamp.y );
		yRotation += mouseX;

		// SetRotation And Position
		Camera.Transform.LocalRotation = new Angles( xRotation, yRotation, 0 );
		Camera.Transform.Position = Transform.Position;

		// FOV
		cameraComponent.FieldOfView = MathX.Lerp( cameraComponent.FieldOfView, MathX.Clamp(startFov + ((rb.Velocity.Abs().z + rb.Velocity.Abs().y + rb.Velocity.Abs().z)* fovVelocityMult), 0, maxFov ) , Time.Delta * fovVelocityLerp );
	}

	void wormMovement( Vector3 cameraForwardFlat )
	{
		wormVel = Vector3.Lerp( wormVel, wormSpeed * ((cameraForwardFlat * Input.AnalogMove.x) + (forwardAxis.Transform.World.Right * -Input.AnalogMove.y)), Time.Delta * wormSpeedLerp );
		rb.Velocity = new Vector3( wormVel.x, wormVel.y, rb.Velocity.z );
	}

	void normalMovement( Vector3 cameraForwardFlat, bool AirControl )
	{
		if ( IsOnGround || AirControl )
		{
			rb.ApplyForce( forceMultForBetterConfig * Time.Delta * Acceleration * ((cameraForwardFlat * Input.AnalogMove.x) + (forwardAxis.Transform.World.Right * -Input.AnalogMove.y)) );
			Vector3 velocityExludingZ = rb.Velocity;
			velocityExludingZ.z = 0;
			if ( Input.Down( "Run" ) ) rb.ApplyForce( forceMultForBetterConfig * -velocityExludingZ * BrakeAcceleration * Time.Delta );
		}
	}

	protected override void OnUpdate()
	{
		CameraControl();

		// TROLLFACEINREALLIFE: Setting the Vector Directions and Forward Axis 
		Vector3 cameraForwardFlat = Camera.Transform.World.Forward.WithZ( 0 );
		Angles newRotation = Rotation.LookAt( cameraForwardFlat );
		forwardAxis.Transform.Rotation = newRotation;

		if ( CurrentMode != MoveMode.Gravity )
		{
			Scene.PhysicsWorld.Gravity = grav;
		}

		if ( CurrentMode == MoveMode.Normal )
		{
			if ( Input.Pressed( "Jump" ) && IsOnGround )
				Jump();

			normalMovement( cameraForwardFlat, false );
		}

		else if ( CurrentMode == MoveMode.Worm )
		{
			if ( Input.Pressed( "Jump" ) && IsOnGround )
				Jump();

			wormMovement( cameraForwardFlat );
		}
		else if ( CurrentMode == MoveMode.Gravity )
		{
			if ( Input.Pressed( "Jump" ) )
				Scene.PhysicsWorld.Gravity *= -1;

			normalMovement( cameraForwardFlat, true );
		}
	}
}
