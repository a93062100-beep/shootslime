using Godot;
using System;

public partial class Bullet : Area2D
{
    float speed = 200 ;

    float Range = 200 ;

    float distanceTravelled = 0 ;

    public Vector2 dir ;


    public override void _PhysicsProcess(double delta)
    {
        Vector2 position = Position ;

        position.Y -= speed * (float) delta ;

        Position = position ;

        distanceTravelled += speed *(float) delta ;
        
        if (distanceTravelled > Range)
        {
            QueueFree() ;
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is IDamageable damageable && body.IsInGroup("enemy"))
        {
            damageable.TakeDamage(1);
            
            QueueFree();

            GD.Print("enemy attacked") ;
        }
    }

}
