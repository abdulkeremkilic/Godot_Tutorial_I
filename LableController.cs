using Godot;
using System;

public partial class LableController : Label
{
	private int upKeyPressed = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Visible = false;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (Input.IsKeyPressed(Key.Up))
		{
			GD.Print(upKeyPressed);
			upKeyPressed += 1;
		}


		if (upKeyPressed >= 15 && this.onTimerTimeout())
		{
			GD.Print("UpKey pressed more than 15 times -> toogleVisibility(): true");
			this.toogleVisibility();
		}

	}

	public void toogleVisibility()
	{
		this.Visible = !this.Visible;
	}

	private Boolean onTimerTimeout()
	{ //timer node connected to lable1, and listens to this method when timer is out.
		this.resetKeyPressed();
		return true;
	}

	private void onTimerTimeoutLableCleaner()
	{ // another timer for clearing the lable off the screen it calles itself automatically once timer is out.
		if (this.Visible == true)
		{
			this.toogleVisibility();
			GD.Print("0.5 second is off: cleaning the screen");
		}
	}

	private void resetKeyPressed()
	{
		this.upKeyPressed = 0;
	}
}
