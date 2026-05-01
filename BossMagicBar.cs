using Godot;
using System;

public partial class BossMagicBar : ProgressBar
{
    Boss boss ;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		boss = GetNode<Boss>("/root/World/boss") ;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (!boss.die)
		{
			this.MaxValue = boss.magic_time ;
		    this.Value = boss.time_until_magic ;
		}
    }
}
