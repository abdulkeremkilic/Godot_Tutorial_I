using Godot;

public partial class PlayerController : CharacterBody2D
{
    [Export] public float moveSpeed { get; set; } = 150f;
    [Export] public float jumpVelocity { get; set; } = 500f; //Y axis in Godot is in negative direction. It's downward.
    private double jumpTimer = 0;
    private AnimatedSprite2D sprite;
    /// <summary>
    /// Get gravity from project settings to keep everything in synced.
    /// </summary>
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
        velocity.X = 0; //to stop character moving constanly even if you don't press any button in X axis.

        playIdleAnimation();
        velocity = this.applyGravity(delta, velocity);
        velocity = this.move(velocity);

        Velocity = velocity;
        this.MoveAndSlide();

    }

    private Vector2 move(Vector2 velocity)
    {
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

        if (Input.IsKeyPressed(Key.Right))
        {
            velocity.X = moveSpeed;
            sprite.FlipH = false;
            sprite.Play("walking");
        }
        else if (velocity.X == 0)
        {
            this.playIdleAnimation();
        }

        if (Input.IsKeyPressed(Key.Left))
        {
            velocity.X = -moveSpeed;
            sprite.FlipH = true;
            sprite.Play("walking");

        }
        else if (velocity.X == 0)

        {
            this.playIdleAnimation();
        }

        return velocity;
    }

    private Vector2 applyGravity(double delta, Vector2 velocity)
    {
        //apply gravity only if in the air
        if (!this.IsOnFloor())
            velocity.Y += gravity * (float)delta;
        return velocity;
    }

    private void playIdleAnimation()
    {
        if (!Input.IsAnythingPressed() && this.IsOnFloor())
        {
            sprite.Play("idle");
        }
    }



}

