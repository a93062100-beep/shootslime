using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	// Called when the node enters the scene tree for the first time.

	float spawn_rate = 1 ;

	float time_until_spawn = 0 ;

	public bool summon_boss = true ;

	public bool boss_summoning = false ;

	[Export] PackedScene enemyscene ;

	[Export] PackedScene enemy2scene ;

	[Export] PackedScene enemy3scene ;

	[Export] PackedScene bossscene ;

	[Export] public Node2D[] spawn_points ;

	Node2D bossSpawnPoint ;

	Player player ;

	Boss boss ;

	public override void _Ready()
	{
		enemyscene = GD.Load<PackedScene>("res://enemy.tscn") ;

		enemy2scene = GD.Load<PackedScene>("res://Characters/Enemies/enemy_2.tscn") ;

		enemy3scene = GD.Load<PackedScene>("res://enemy_3.tscn") ;

		bossscene = GD.Load<PackedScene>("res://boss.tscn") ;

		player = GetNode<Player>("/root/World/Player") ;

		bossSpawnPoint = GetNode<Node2D>("bossSpawnPoint") ;

		boss = GetNode<Boss>("/root/World/boss") ;
	}

	private void spawn()
	{
        RandomNumberGenerator rng = new RandomNumberGenerator() ;

		Vector2 location = spawn_points[rng.Randi() % spawn_points.Length].GlobalPosition ;
        
		RandomNumberGenerator rng2 = new RandomNumberGenerator() ;

		if (rng2.Randi() % 3 == 0)
		{
			Enemy enemy = enemyscene.Instantiate<Enemy>() ;
		    enemy.GlobalPosition = location ;
		    GetTree().CurrentScene.AddChild(enemy) ;
		}

		else if (rng2.Randi() % 3 == 1)
		{
			Enemy2 enemy2 = enemy2scene.Instantiate<Enemy2>() ;
			enemy2.GlobalPosition = location ;
			GetTree().CurrentScene.AddChild(enemy2) ;									
		}

		else
		{
			Enemy3 enemy3 = enemy3scene.Instantiate<Enemy3>() ;

			Vector2 player_pos = player.GlobalPosition;

			Vector2 randpos = location ;

			randpos.X = player_pos.X ;

            enemy3.GlobalPosition = randpos ;

			GetTree().CurrentScene.AddChild(enemy3) ;
		}
	}

	private void summonBoss()
	{
		Boss boss = bossscene.Instantiate<Boss>() ;
		boss.GlobalPosition = bossSpawnPoint.GlobalPosition ;
		GetTree().CurrentScene.AddChild(boss) ;
	}

	// Called every frame. 'delta' is they elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (time_until_spawn > 1 / spawn_rate)
		{
			spawn() ;

			time_until_spawn = 0 ;
		}

		else
		{
			time_until_spawn += (float) delta ;
		}

		if (summon_boss && Enemy.score == 1000)
		{
			summonBoss() ;

			boss.die = false ;

			boss_summoning = true ;

			summon_boss = false ;
		}
	}
}
