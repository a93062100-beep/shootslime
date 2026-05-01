using Godot;
using System;

public partial class ProgressBar3 : HealthBar
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
		this.MaxValue = 100 ;

		this.Value = boss.health ;

		var fillstyle = GetThemeStylebox("fill") as StyleBoxFlat ;

		var backgroundstyle = GetThemeStylebox("background") as StyleBoxFlat ;

		backgroundstyle.BgColor = Colors.Gray ;

		backgroundstyle.BorderWidthLeft = 2 ;

		backgroundstyle.BorderWidthRight = 2 ;

		backgroundstyle.BorderWidthTop = 2 ;

		backgroundstyle.BorderWidthBottom = 2 ;

		fillstyle.BgColor = Colors.Red ;
	}
}
