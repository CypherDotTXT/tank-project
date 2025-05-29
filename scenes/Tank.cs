using Godot;
using System;

public partial class Tank : CharacterBody2D
{
	[Export] public float speed { get; set; } = 200f; // movement speed
	[Export] public float rotationSpeed { get; set; } = 2f; // rotation speed
	[Export] public PackedScene BulletScene { get; set; }
	private AnimationPlayer _animationPlayer;
	private Node2D weapon;
	private Marker2D muzzle;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		weapon = GetNode<Node2D>("Weapon");
		muzzle = weapon.GetNode<Marker2D>("Muzzle");
	}
	public void tankWeaponMovement()
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		Vector2 turnTurret = mousePosition - weapon.GlobalPosition;
		weapon.Rotation = turnTurret.Angle() - Rotation;
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Vector2.Zero;

		// Get input for movement
		if (Input.IsActionPressed("turn_right"))
			Rotation += rotationSpeed * (float)delta;
		if (Input.IsActionPressed("turn_left"))
			Rotation -= rotationSpeed * (float)delta;
		if (Input.IsActionPressed("move_forward"))
			velocity = Transform.X * speed;
		if (Input.IsActionPressed("move_backward"))
			velocity = -Transform.X * speed;

		// Normalize velocity to prevent faster diagonal movement
		velocity = velocity.Normalized() * speed;

		// Play animation based on movement
		if (velocity.Length() > 0)
		{
			_animationPlayer.Play("move");
		}
		else
		{
			_animationPlayer.Play("idle");
		}

		//Get input for shooting
		if (Input.IsActionJustPressed("weapon_fire"))
			Shoot();

		// Move the player
		Velocity = velocity;
		MoveAndSlide();
		tankWeaponMovement();
	}
	public void Shoot()
	{
		if (BulletScene == null)
			return;

		Bullet bullet = BulletScene.Instantiate<Bullet>();
		GetTree().CurrentScene.AddChild(bullet);

		// Set bullet position to muzzle of weapon
		bullet.GlobalPosition = muzzle.GlobalPosition;
		//bullet direction
		Vector2 direction = (GetGlobalMousePosition() - muzzle.GlobalPosition).Normalized();
		bullet.Init(direction);
	}
}
