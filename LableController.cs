using Godot;
using System;

public partial class LableController : Label
{
	int upKeyPressed = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
        if (upKeyPressed >= 15)
        {
            upKeyPressed = 0;
			this.toogleVisibility();
        }
    }

	public void toogleVisibility() {
		this.Visible = !this.Visible;
	}

}
