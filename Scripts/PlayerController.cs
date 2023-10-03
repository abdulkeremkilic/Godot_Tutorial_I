using System;
using Godot;

public partial class PlayerController : CharacterBody2D
{
    [Export] public float moveSpeed { get; set; } = 150f;
    [Export] public float jumpVelocity { get; set; } = 500f; //Y axis in Godot is in negative direction. It's downward.
    private double jumpTimer = 0;
    private AnimatedSprite2D sprite;
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();


    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite");
    }

    public override void _Process(double delta)
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        velocity.X = 0; 
        //to stop character moving constanly even if you don't press any button in X axis. 
        // Make this by timer so player can take advantage of acceleration. 

        playIdleAnimation();
        velocity = this.applyGravity(delta, velocity);
        velocity = this.move(velocity);



        Velocity = velocity;
        this.MoveAndSlide();
    }

    private Vector2 move(Vector2 velocity)
    {

        #region (crouch)
        if (Input.IsKeyPressed(Key.Down) && !IsOnFloor())
        //TODO: Make this code to read the key for once.
        {
            velocity.Y = jumpVelocity * 2;
            sprite.Play("fall");
        }
        else if (Input.IsKeyPressed(Key.Down) && IsOnFloor())
        {
            sprite.Play("crouch");
        }
        #endregion

        if(this.isNextToWall() && velocity.Y > 0)
        {
            sprite.FlipH = true;
            sprite.Play("wall_slide");
        }
       


        #region (jump)
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
        #endregion


        #region (move_right)
        if (Input.IsKeyPressed(Key.Right))
        {
            velocity.X = moveSpeed;
            sprite.FlipH = false;
            sprite.Play("walking");
        } 
        #endregion


        #region (move_left) 
        if (Input.IsKeyPressed(Key.Left))
        {
            velocity.X = -moveSpeed;
            sprite.FlipH = true;
            sprite.Play("walking");
        }
        #endregion

        return velocity;
    }

    private Vector2 applyGravity(double delta, Vector2 velocity)
    {
        //apply gravity only if in the air
        if (!this.IsOnFloor()) 
        {
            velocity.Y += gravity * (float)delta;
            sprite.Play("fall");
        }
        return velocity;
    }

    private void playIdleAnimation()
    {
        if (!Input.IsAnythingPressed() && this.IsOnFloor() && Velocity.X == 0)
        {
            sprite.Play("idle");
        }
    }


    private Boolean isNextToWall() 
    {
        return this.isNextToLeftWall() || this.isNextToRightWall();
    }

    private Boolean isNextToRightWall()
    {
        RayCast2D nextToRightWall = GetNode<RayCast2D>("rightWall");
        return nextToRightWall.IsColliding();
    }

    private Boolean isNextToLeftWall()
    {
        RayCast2D nextToLeftWall = GetNode<RayCast2D>("leftWall");
        return nextToLeftWall.IsColliding();
    }
}

