using Godot;
using System;
using System.Numerics;

public partial class Enemy2 : CharacterBody2D,IDamageable
{
	public const float speed = 60;

	public float health = 3 ;

	float attack_speed = 1 ;

	float time_until_attack = 1 ;

	float fire_rate = 1 ;

	float time_until_fire = 1 ;

	float attack_time = 1 ;

	float time_until_endAttack = 0 ;

    float dying_time = 1 ;

	float time_until_die = 0 ;

	bool attacking = false ;

	bool addScore = true ;

	bool within_attack_range = false ;

	Player player ;

	[Export] public PackedScene explosionscene ;

	Enemy enemy ;

	World world ;

	Explosion explose ;

	AnimationPlayer animationplayer ;

	public void TakeDamage(float amount)
	{
		health -= amount ;
	}
    public override void _Ready()
    {
        player = GetNode<Player>("/root/World/Player") ;

		explosionscene = GD.Load<PackedScene>("res://explosion.tscn") ;

		world = GetNode<World>("/root/World") ;

		animationplayer = GetNode<AnimationPlayer>("AnimationPlayer") ;
	}
    
	public void explosions()
	{
		Explosion explosion = explosionscene.Instantiate<Explosion>() ;

		Godot.Vector2 pos = player.GlobalPosition ;

		pos.Y -= 20 ;

		explosion.GlobalPosition = pos ;

		GetTree().CurrentScene.AddChild(explosion) ;
	}

    public override void _Process(double delta)
    {
        if (within_attack_range && time_until_attack > attack_speed)
		{
			player.health -- ;

			GameManager.Instance.DamagePlayer() ;

			GD.Print("attack player") ;

			world.player_is_hurt = true ;

			time_until_attack = 0 ;
		}

		else
		{
			time_until_attack += (float) delta ;
		}

		Godot.Vector2 pos = Position ;

		Godot.Vector2 player_pos = player.Position ;

		if (time_until_fire > 2 / fire_rate && !player.die && pos.Y <= player_pos.Y && health > 0)
		{
			explosions() ;

			attacking = true ;

			time_until_fire = 0 ;
		}

		else
		{
			time_until_fire += (float) delta ;
		}

		if (attacking && time_until_endAttack <= 1 / (attack_time * 2) && health > 0)
		{
			animationplayer.Play("attack") ;

			time_until_attack += (float) delta ;
		}

		else if (attacking && time_until_endAttack > 1 / (attack_time * 2) && health > 0)
		{
			attacking = false ;

			time_until_endAttack = 0 ;
		}

		if (pos.Y > player_pos.Y + 100)
		{
			QueueFree() ;
		}
    }


	public override void _PhysicsProcess(double delta)
	{
		if (health > 0)
		{
			Godot.Vector2 velocity = Velocity;

		    velocity.Y = speed ;

		    Velocity = velocity ;

		    Position += Velocity * (float) delta ;

			if (!attacking)
			{
				animationplayer.Play("move") ;      
			}
		}

		else
		{
			if (addScore)
			{
				Enemy.score ++ ;
			}

			addScore = false ;

			if (time_until_die <= dying_time / 2)
			{
				animationplayer.Play("die") ;

				time_until_die += (float) delta ;
			}

			else
			{
				QueueFree();
			}
		}
	}

	public void OnBodyEnter(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			within_attack_range = true ;
		}
	}

	public void OnBodyExit(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			within_attack_range = false ;
		}
	}
}
