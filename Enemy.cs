using Godot;
using GodotPlugins.Game;
using System;
using System.Collections;
using System.ComponentModel;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

public partial class Enemy : CharacterBody2D, IDamageable
{
    [Export]PackedScene enemyBulletscene ;
    float speed = 60 ;

    public float health = 3 ;

    float attack_speed = 1;

    float fire_rate = 1 ;

    float time_until_fire = 0 ;

    public static int score = 0 ;

    float time_until_attack = 0 ;

    float attack_time = 1 ;

    float time_until_endAttack = 0 ;

    float dying_time = 1 ;

    float time_until_die = 0 ;

    bool within_attack_range = false ;

    bool attacking = false ;

    bool addScore = true ;

    Sprite2D sprite2D ;

    public AnimationPlayer animationplayer ;

    Player player ;

    World world ;

    public void TakeDamage(float amount)
    {
        health -= amount ;
    }

    public override void _Ready()
    {
        sprite2D = GetNode<Sprite2D>("Sprite2D") ;

        enemyBulletscene = GD.Load<PackedScene>("res://enemy_bullet.tscn") ;

        player = GetNode<Player>("/root/World/Player") ;

        animationplayer = GetNode<AnimationPlayer>("AnimationPlayer") ;

        world = GetNode<World>("/root/World") ;
    }
    public void enemy_shoot()
    {
        EnemyBullet enemyBullet = enemyBulletscene.Instantiate<EnemyBullet>() ;
        enemyBullet.GlobalPosition = GlobalPosition ;
        GetTree().CurrentScene.AddChild(enemyBullet) ;
        enemyBullet.IsShootedByBoss = false ;
    }
    
    public override void _Process(double delta)
    {
        if (!player.die)
        {
            Godot.Vector2 pos = Position ;

            Godot.Vector2 player_pos = player.Position ;

            if (pos.Y > player_pos.Y + 100)
            {
                QueueFree();
            }
        }
        if (within_attack_range && time_until_attack > 1 / attack_speed) 
        {
            GD.Print("Attack player") ;

            time_until_attack = 0 ;

            if (player.health > 0)
            {
                GameManager.Instance.DamagePlayer() ;

                player.health -- ;

                world.player_is_hurt = true ;
            }
        }

        else
        {
            time_until_attack += (float) delta ;
        }

        if (time_until_fire > 1 / fire_rate && health > 0)
        {
            enemy_shoot() ;

            attacking = true ;

            time_until_fire = 0 ;
        }

        else
        {
            time_until_fire += (float) delta ;
        }

        if (attacking && time_until_endAttack <= 1 / (2 * attack_time) && health > 0 )
        {
            animationplayer.Play("attack") ;

            time_until_endAttack += (float) delta ;
        }

        else if (attacking && time_until_endAttack > 1 / (2 * attack_time) && health > 0)
        {
            attacking = false ;

            time_until_endAttack = 0 ;
        }

    }

    public override void _PhysicsProcess(double delta)
    {

        if (health > 0)
        {
            Godot.Vector2 velocity = Velocity ;

            velocity.Y = speed ;

            Velocity = velocity ;

            Position += Velocity * (float) delta ;

            if (!attacking)
            {
                animationplayer.Play("walk") ;
            }
        }

        else
        {
            if (addScore)
            {
                score ++ ;
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

    public void OnAttackRangeBodyEnter (Node2D body)
    { 
        if (body.IsInGroup("player") && health > 0)
        {
            within_attack_range = true ;
        }
    }
     

    public void OnAttackRangeBodyExit (Node2D body)
    {
        if (body.IsInGroup("player"))
        {
            within_attack_range = false ;
        }
    }
  
}


