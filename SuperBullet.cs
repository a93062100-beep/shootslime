using Godot;
using System;

public partial class SuperBullet : Area2D
{
	float speed = 300 ;

    float Range = 400 ;

    float distanceTravelled = 0 ;

	bool attacking = false ;

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        Godot.Vector2 position = Position ;

        position.Y -= speed * (float) delta ;

        Position = position ;

        distanceTravelled += speed *(float) delta ;
        
        if (distanceTravelled > Range)
        {
            QueueFree() ;
		}
    }

    private void OnBodyEnter(Node2D body)
    {
        if (body is IDamageable damageable && body.IsInGroup("enemy"))
        {
            damageable.TakeDamage(1) ;

            GD.Print("attack enemy") ;
        }
	}
}
