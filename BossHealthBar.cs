using Godot;
using System;

public partial class BossHealthBar : ProgressBar
{
    BossHealthBar bossHealthBar ;
    Boss boss ;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		boss = GetNode<Boss>("/root/World/boss") ;
	}


	public override void _Process(double delta)
	{
		this.MaxValue = boss.maxHealth ;

		this.Value = boss.health;

        if (this.Value <= 0)
        {
            bossHealthBar.Visible = false ;
        }
    }
}
