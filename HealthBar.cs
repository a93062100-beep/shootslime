using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	Player player ;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<Player>("/root/World/Player") ;
	}


	public override void _Process(double delta)
	{
		this.MaxValue = player.maxHealth ;

		this.Value = player.health;

		var fillstyle = GetThemeStylebox("fill") as StyleBoxFlat ;

		fillstyle.BgColor = Colors.Red ;
           
	}
}
