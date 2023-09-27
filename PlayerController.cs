using Godot;
using System;
using System.Threading.Tasks;

public partial class PlayerController : CharacterBody2D
{
    public float moveSpeed = 150f;
    public float jumpVelocity = 500f; //Y axis in Godot is in negative direction. It's downward.
    public double jumpTimer = 0;


    /// <summary>
    /// Get gravity from project settings to keep everything in synced.
    /// </summary>
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();


    public override void _Ready()
    {
    }


    public override void _PhysicsProcess(double delta)
    {
        jumpTimer += delta;
        Vector2 velocity = Velocity;
        //GD.Print(jumpTimer);


        //apply gravity only if in the air
        if (!this.IsOnFloor())
            velocity.Y += gravity * (float)delta;

        //This code below makes the character go back to the floor instantly. Can be used for some combos.
        if (Input.IsKeyPressed(Key.Down) && !IsOnFloor())
        //This way it count even if you're holding the key. So this is not the proper way.
        //TODO: Make this code to read the key for once.
        {
            velocity.Y = jumpVelocity * 5;
        }

        if (Input.IsKeyPressed(Key.Up) && IsOnFloor())
        {
            velocity.Y = -jumpVelocity;
        }

        if (Input.IsKeyPressed(Key.Up) && !IsOnFloor() && jumpTimer > 1.0) 
        //Jump or whatever timer! 
        //I can give specific time zone for this to make combo movements controls more strictly. 
        //(Player should press the specified button at the right time!) 
        //Can show a button for a limited time in the screen to make player press the button!
        {
            velocity.Y = -jumpVelocity;
            jumpTimer = 0.0;
        }


        velocity.X = 0;
        if (Input.IsKeyPressed(Key.Left))
            velocity.X = -moveSpeed;
        if (Input.IsKeyPressed(Key.Right))
            velocity.X = moveSpeed;

        Velocity = velocity;
        this.MoveAndSlide();
    }
}
