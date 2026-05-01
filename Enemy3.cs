using Godot;
using System;

public partial class Enemy3 : CharacterBody2D , IDamageable
{
	public float speed = 300;

	public float health = 3 ;

    float attack_speed = 1;

	float time_until_attack = 1 ;

	float dying_time = 1 ;

    float time_until_die = 0 ;

    bool within_attack_range = false ;

    bool addScore = true ;

	Sprite2D sprite2D ;

    public AnimationPlayer animationplayer ;

	AudioStreamPlayer2D slimeImpactSound ;

    Player player ;

	World world ;

    public void TakeDamage(float amount)
    {
        health -= amount ;
    }

	public override void _Ready()
	{
		sprite2D = GetNode<Sprite2D>("Sprite2D") ;

		player = GetNode<Player>("/root/World/Player") ;

        animationplayer = GetNode<AnimationPlayer>("AnimationPlayer") ;

		slimeImpactSound = GetNode<AudioStreamPlayer2D>("slimeImpactSound") ;

        world = GetNode<World>("/root/World") ;

	}

    public override void _Process(double delta)
    {
        if (!player.die)
        {
            Vector2 pos = Position ;

            Vector2 player_pos = player.Position ;

            if (pos.Y > player_pos.Y + 100)
            {
                QueueFree();
            }

			else if (pos.Y <= player_pos.Y)
			{
				slimeImpactSound.Play() ;
			}
        }
        if (within_attack_range && time_until_attack > 1 / attack_speed) 
        {
            GD.Print("Attack player") ;

            time_until_attack = 0 ;

            if (player.health > 0)
            {
                player.health -- ;

				GameManager.Instance.DamagePlayer() ;

                world.player_is_hurt = true ;
            }
		}

		else
        {
            time_until_attack += (float) delta ;
        }
    }

	public override void _PhysicsProcess(double delta)
	{
		if (health > 0)
        {
            Vector2 velocity = Velocity ;

            velocity.Y = speed ;

            Velocity = velocity ;

            Position += Velocity * (float) delta ;

            animationplayer.Play("attack") ;
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
                QueueFree() ;
            }
        }
	}

	public void OnBodyEnter (Node2D body)
    { 
        if (body.IsInGroup("player") && health > 0)
        {
            within_attack_range = true ;
        }
    }
     

    public void OnBodyExit (Node2D body)
    {
        if (body.IsInGroup("player"))
        {
            within_attack_range = false ;
        }
    }
}
