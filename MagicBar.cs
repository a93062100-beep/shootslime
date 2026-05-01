using Godot;
using System;

public partial class MagicBar : ProgressBar
{
    Player player ;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<Player>("/root/World/Player") ;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	    var fillstyle = GetThemeStylebox("fill") as StyleBoxFlat ;

		var backgroundstyle = GetThemeStylebox("background") as StyleBoxFlat ;

	    backgroundstyle.BgColor = Colors.Gray ;

		fillstyle.BgColor = Colors.Blue ;

		if (!player.die)
		{
			this.MaxValue = player.magic_time ;
		    this.Value = player.time_until_magic ;
		}
    }
}
