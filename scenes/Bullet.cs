using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export] public float speed = 600f;
	public Vector2 Velocity = Vector2.Zero;

	public override void _PhysicsProcess(double delta)
	{
		Position += Velocity * (float)delta;

		if (!GetViewportRect().HasPoint(GlobalPosition))
			QueueFree();
	}
	public void Init(Vector2 direction)
	{
		Velocity = direction.Normalized() * speed;
		Rotation = direction.Angle();
	}
}
