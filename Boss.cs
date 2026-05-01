using System.Data.Common;
using Godot;

public partial class Boss : CharacterBody2D, IDamageable
{
    public float time_until_magic = 0 ;

    public float maxHealth = 300 ;

	public float health = 300 ;

	public float magic_time = 1 ;

	int fire_round = 0 ;

	float summon_time = 1 ;

	float time_until_endSummon = 0 ;

	float time_until_die = 0 ;

	float dying_time = 1 ;

	public float angleStep = 20 ;

	public int bulletCount = 4 ;

	float shooting_amount = 5 ;

	float shooting_delay = 1 ;

	float time_until_shoot = 1 ;

	int shootedbullet = 0 ;

	float distanceTravelled = 0 ;

	float range = 100 ;

	float speed = 50 ;

	public bool boss_exist = true ;

	bool first_time_shooting = true ;

	bool addScore = true ;

	public bool die = false ;

	bool moveRight = true ;

	[Export] public PackedScene explosionscene ;

	[Export] public PackedScene enemyBulletScene ;

	AnimationPlayer animationplayer ;

	EnemySpawner enemyspawner ;

	Sprite2D sprite2D ;

	[Export] PackedScene enemyscene ;

	[Export] PackedScene enemy2scene ;

	[Export] PackedScene enemy3scene ;

	Player player ;

    public override void _Ready()
    {
		explosionscene = GD.Load<PackedScene>("res://explosion.tscn") ;

		enemyBulletScene = GD.Load<PackedScene>("res://enemy_bullet.tscn") ;

		enemyscene = GD.Load<PackedScene>("res://enemy.tscn") ;

		enemy2scene = GD.Load<PackedScene>("res://Characters/Enemies/enemy_2.tscn") ;

		enemy3scene = GD.Load<PackedScene>("res://enemy_3.tscn") ;

        animationplayer = GetNode<AnimationPlayer>("AnimationPlayer") ;

		enemyspawner = GetNode<EnemySpawner>("/root/World/EnemySpawner") ;

		player = GetNode<Player>("/root/World/Player") ;

		sprite2D = GetNode<Sprite2D>("Sprite2D") ;
    }

	private async void shoot()
	{
		for (int wave = 0 ; wave < shooting_amount ; wave++)
		{
			float startAngleDegree = (player.GlobalPosition - GlobalPosition).Angle() ;

		    float angleStepRad = Mathf.DegToRad(angleStep) ;

		    for (int i = -bulletCount ; i <= bulletCount ; i++)
		    {
			    EnemyBullet enemyBullet = enemyBulletScene.Instantiate<EnemyBullet>() ;

			    float currentRadian = startAngleDegree + (i * angleStepRad) ;

			    enemyBullet.Rotation = currentRadian ;

			    enemyBullet.Direction = Godot.Vector2.FromAngle(currentRadian) ; // call a vector that length is 1

			    enemyBullet.GlobalPosition = GlobalPosition ;

			    GetTree().CurrentScene.AddChild(enemyBullet) ;

				enemyBullet.IsShootedByBoss = true ;
		    }

			await ToSignal(GetTree().CreateTimer(0.1f),SceneTreeTimer.SignalName.Timeout) ;
	    }
	}

    private void spawning()
	{
		for (int i = 0 ; i < enemyspawner.spawn_points.Length ; i++)
		{
			RandomNumberGenerator rng = new RandomNumberGenerator() ;

			Godot.Vector2 location = enemyspawner.spawn_points[i].GlobalPosition ;

            if (rng.Randi() % 3 == 0)
			{
				Enemy enemy = enemyscene.Instantiate<Enemy>() ;
		        enemy.GlobalPosition = location ;
		        GetTree().CurrentScene.AddChild(enemy) ;
			}

			else if (rng.Randi() % 3 == 1)
			{
				Enemy2 enemy2 = enemy2scene.Instantiate<Enemy2>() ;
				enemy2.GlobalPosition = location ;
				GetTree().CurrentScene.AddChild(enemy2) ;
			}

			else
			{
				Enemy3 enemy3 = enemy3scene.Instantiate<Enemy3>() ;
				enemy3.GlobalPosition = location ;
				GetTree().CurrentScene.AddChild(enemy3) ;			
			}
		}
	}

    private void explosion()
	{
		Explosion explosion_1 = explosionscene.Instantiate<Explosion>() ;

		Explosion explosion_2 = explosionscene.Instantiate<Explosion>() ;

		Explosion explosion_3 = explosionscene.Instantiate<Explosion>() ;

		if (!player.die)
		{
		    Godot.Vector2 pos_1 = player.GlobalPosition ;

			Godot.Vector2 pos_2 = player.GlobalPosition ;

			Godot.Vector2 pos_3 = player.GlobalPosition ;

			pos_1.Y -= 20 ;

			pos_2.Y -= 20 ;

			pos_3.Y -= 20 ;

			pos_1.X -= 80 ;

			pos_3.X += 80 ;

			explosion_1.GlobalPosition = pos_1;

			explosion_2.GlobalPosition = pos_2;

			explosion_3.GlobalPosition = pos_3 ;

			GetTree().CurrentScene.AddChild(explosion_1) ;

			GetTree().CurrentScene.AddChild(explosion_2) ;

			GetTree().CurrentScene.AddChild(explosion_3) ;
		}
	}
    public override void _Process(double delta)
    {
		if (health > 0)
		{
			if (enemyspawner.boss_summoning && time_until_endSummon < 1/(summon_time * 2))
		    {
			    animationplayer.Play("summon") ;

			    time_until_endSummon += (float) delta ;
		    }

		    else if (enemyspawner.boss_summoning && time_until_endSummon >= 1/(summon_time * 2))
		    {
			    enemyspawner.boss_summoning = false ;

			    animationplayer.Play("attack") ;

			    time_until_endSummon  = 0 ;
		    }

		    else if (!enemyspawner.boss_summoning)
		    {
			    animationplayer.Play("attack") ;
		    }
		}

		else
		{
			if (time_until_die <= dying_time)
			{
				time_until_die += (float) delta ;

				animationplayer.Play("die") ;
			}

			else
		    {
				if (addScore)
				{
					Enemy.score += 10 ;

					addScore = false ;
				}
				die = true ;
			}
		}


		if (time_until_magic < magic_time)
		{
			time_until_magic += (float) delta ;
		}

		else if (fire_round % 3 == 0 && time_until_magic >= magic_time && !die)
		{
			spawning() ;

			time_until_magic = 0 ;

			fire_round ++ ;
		}

		else if (fire_round % 3 == 1 && time_until_magic >= magic_time && !die)
		{
			explosion() ;

			time_until_magic = 0 ;

			fire_round ++ ;
		}

		else if (fire_round % 3 == 2 && time_until_magic >= magic_time && !die)
		{
			shoot() ;

			time_until_magic = 0 ;

            fire_round ++ ;
	    }

		if (die)
		{
			sprite2D.Visible = false ;
		}
    }

    public override void _PhysicsProcess(double delta)
    {
        if (distanceTravelled >= range)
		{
			moveRight = false ;
		}

		else if (distanceTravelled <= 0)
		{
			moveRight = true ;
		}

		if (moveRight && health > 0)
		{
            Godot.Vector2 velocity = Velocity ;

            velocity.X = speed ;

            Velocity = velocity ;

            Position += Velocity * (float) delta ;

			distanceTravelled += speed * (float) delta ;
		}

		else if (!moveRight && health > 0)
		{
			Godot.Vector2 velocity = Velocity ;

			velocity.X = -speed ;

			Velocity = velocity ;

			Position += Velocity * (float) delta ;

			distanceTravelled -= speed * (float) delta ;
		}
    }

	public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            GD.Print(Name, " died");
            QueueFree();
        }
    }
}
