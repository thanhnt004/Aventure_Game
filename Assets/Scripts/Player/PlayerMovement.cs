using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMovement : MonoBehaviour
{
    public PlayerAtribute Data;
    #region Variables
    public Rigidbody2D rb { get; private set; }
    //Các biến điều hiện, thể hiện trạng thái của nhân vật
	public bool IsFacingRight { get; private set; }//đang nhìn phải
	public bool IsJumping { get; private set; }//đang nhảy
	public bool IsWallJumping { get; private set; }//đang bật nhảy khỏi tường
	public bool IsSliding { get; private set; }//đang trượt

	//Các biến thời gian chỉ thời gian cuối của từng trạng thái
	public float LastOnGroundTime { get; private set; }
	public float LastOnWallTime { get; private set; }
	public float LastOnWallRightTime { get; private set; }
	public float LastOnWallLeftTime { get; private set; }
	
	//Trạng thái nhảy
	private bool _isJumpCut;//khi thả nút nhảy 
	private bool _isJumpFalling;//Khi nhân vật rơirơi 
	private float _wallJumpStartTime;//thời gian bắt đầu bật khỏi tường 
	private int _lastWallJumpDir;//Hướng nhảy 
	public float LastPressedJumpTime { get; private set; }// thời gina bấm nhảy

	private Vector2 _moveInput;//Hướng chuyển động của nhân vật
	//Audio
    private AudioManager audioManager;//Biến điều khiển audio
	//Các biến lưu trữ vùng kiểm tra trạng thái của nhân vật
    [Header("Checks")] 
	[SerializeField] private Transform _groundCheckPoint;
	[SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
	[Space(5)]
	[SerializeField] private Transform _frontWallCheckPoint;
	[SerializeField] private Transform _backWallCheckPoint;
	[SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);

    [Header("Layers & Tags")]
	[SerializeField] private LayerMask _groundLayer;
	[Header("Animator")]
	[SerializeField] private Animator animator;
	#endregion
    private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
	}

	private void Start()
	{
		SetGravityScale(Data.gravityScale);
		IsFacingRight = true;
	}
    void Update()
    {
        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;
		LastOnWallTime -= Time.deltaTime;
		LastOnWallRightTime -= Time.deltaTime;
		LastOnWallLeftTime -= Time.deltaTime;
		LastPressedJumpTime -= Time.deltaTime;
		#endregion
		//Kiểm tra trạng thái vị trí của nhân vật
        #region COLLISION CHECKS
		if (!IsJumping)
		{
			//kiểm tra mặt đất
			if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) ) 
			{
				LastOnGroundTime = Data.coyoteTime; 
            }
			//kiểm tra tường bên trái
			if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)
					|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)) && !IsWallJumping)
				LastOnWallRightTime = Data.coyoteTime;

			//Tường bên phải
			if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)
				|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)) && !IsWallJumping)
				LastOnWallLeftTime = Data.coyoteTime;

			LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
		}
		#endregion
		//Kiểm tra trạng thái nhảy 
		#region JUMP CHECKS
		if (IsJumping && rb.linearVelocityY < 0)
		{
			IsJumping = false;
			_isJumpFalling = true;
		}

		if (IsWallJumping && Time.time - _wallJumpStartTime > Data.wallJumpTime)
		{
			IsWallJumping = false;
		}

		if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
			_isJumpCut = false;
			_isJumpFalling = false;
		}

		//Nhảy 
		if (CanJump() && LastPressedJumpTime > 0)
		{
			IsJumping = true;
			IsWallJumping = false;
			_isJumpCut = false;
			_isJumpFalling = false;
			Jump();
			animator.SetTrigger("jump");
			audioManager.PlaySFX(audioManager.jump);
		}
		//Bật tường 
		else if (CanWallJump() && LastPressedJumpTime > 0)
		{
			IsWallJumping = true;
			IsJumping = false;
			_isJumpCut = false;
			_isJumpFalling = false;
			_wallJumpStartTime = Time.time;
			_lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;
			
			WallJump(_lastWallJumpDir);
			animator.SetTrigger("jump");
			audioManager.PlaySFX(audioManager.jump);
		}
		#endregion
		//Kiểm tra điều kiện trượt 
		#region SLIDE CHECKS
		if (CanSlide() && ((LastOnWallLeftTime > 0 && _moveInput.x < 0) || (LastOnWallRightTime > 0 && _moveInput.x > 0)))
		{
			IsSliding = true;
		}
		else
			IsSliding = false;
		#endregion
		//Set trọng lực trong các trạng thái khác nhau 
		#region GRAVITY
		if (IsSliding)
		{
			SetGravityScale(0);
		}
		else if (rb.linearVelocityY < 0 && _moveInput.y < 0)
		{
			//Tăng trọng lực khi nhấn nút xuống fastFallGravityMult
			SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
			//Giới hạn tốc độ rơi
			rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -Data.maxFastFallSpeed));
		}
		else if (_isJumpCut)
		{
			//Tăng trọng lực khi thả nút nhảy jumpCutGravityMult
			SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
			rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -Data.maxFallSpeed));
		}
		else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(rb.linearVelocityY) < Data.jumpHangTimeThreshold)
		{
			SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
		}
		else if (rb.linearVelocityY < 0)
		{
			//Tăng trọng lực khi rơi fallGravityMult
			SetGravityScale(Data.gravityScale * Data.fallGravityMult);
			rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -Data.maxFallSpeed));
		}
		else
		{
			//trongj lực trong trạng thái bình thường 
			SetGravityScale(Data.gravityScale);
		}
		#endregion
    }
    private void FixedUpdate()
	{
		//Xử lý trạng thái chạy 
		if (IsWallJumping)
			Run(Data.wallJumpRunLerp);
		else
			Run(1);

		//Xử lý trạng thái trượt 
		if (IsSliding)
			Slide();
    }
    #region INPUT HANDLER
	//Khi bấm và thả nút nhảy 
    public void Jumped(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
           LastPressedJumpTime = Data.jumpInputBufferTime;
        }
        else if(context.canceled)
        {
            if (CanJumpCut() || CanWallJumpCut())
			_isJumpCut = true;
        }
    }
	//Khi ấn nút di chuyển  
    public void Moved(InputAction.CallbackContext context)
    {
        _moveInput  = context.ReadValue<Vector2>();	
		if (_moveInput.x != 0)
		{
			CheckDirectionToFace(_moveInput.x > 0);
    	}
	}
    #endregion
    #region RUN METHODS
    private void Run(float lerpAmount)
	{
		//tính toán vận tốc và hướng 
		float targetSpeed = _moveInput.x * Data.runMaxSpeed;
		//Giảm sự chuyển động sử dụng lerp 
		targetSpeed = Mathf.Lerp(rb.linearVelocityX, targetSpeed, lerpAmount);

		#region Calculate AccelRate
		float accelRate;

		//Lấy gia tốc 
		if (LastOnGroundTime > 0)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
		#endregion

		#region Add Bonus Jump Apex Acceleration
		//Gia tốc khi nhảy 
		if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(rb.linearVelocityY) < Data.jumpHangTimeThreshold)
		{
			accelRate *= Data.jumpHangAccelerationMult;
			targetSpeed *= Data.jumpHangMaxSpeedMult;
		}
		#endregion

		//Duy trì động lựclực
		#region Conserve Momentum
		if(Data.doConserveMomentum && Mathf.Abs(rb.linearVelocityX) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.linearVelocityX) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
		{
			accelRate = 0; 
		}
		#endregion

		//Tính toán khoảng cách giữa vận tốc hiện tại và vận tốc cực đại 
		float speedDif = targetSpeed - rb.linearVelocityX;
		//Tính toán lực cần thiết 

		float movement = speedDif * accelRate;

		//Tác dụng lực lên nhân vật 
		rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
		if(LastOnGroundTime>0&&_moveInput.x!=0)
			{
				animator.SetBool("run",_moveInput.x!=0);
			}
		else if(LastOnGroundTime>0&&_moveInput.x==0)
			animator.SetBool("run",false);
		/*
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		*/
	}
    private void Turn()
	{
		Vector3 scale = transform.localScale; 
		scale.x *= -1;
		transform.localScale = scale;

		IsFacingRight = !IsFacingRight;
	}
    #endregion
    #region JUMP METHODS
    private void Jump()
	{
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;
		#region Perform Jump
		float force = Data.jumpForce;
		if (rb.linearVelocityY < 0)
			force -= rb.linearVelocityY;
		rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
		#endregion
	}

	private void WallJump(int dir)
	{
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;
		LastOnWallRightTime = 0;
		LastOnWallLeftTime = 0;

		#region Perform Wall Jump
		Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
		force.x *= dir; 

		if (Mathf.Sign(rb.linearVelocityX) != Mathf.Sign(force.x))
			force.x -= rb.linearVelocityX;

		if (rb.linearVelocityY < 0) 
			force.y -= rb.linearVelocityY;
		// if(( dir==1 &&_moveInput.x<0)||(dir == -1 && _moveInput.x>0 ))
		// 	Turn();
		rb.AddForce(force, ForceMode2D.Impulse);
		#endregion
	}
	#endregion

	#region OTHER MOVEMENT METHODS
	private void Slide()
	{
		if(rb.linearVelocityY > 0)
		{
		    rb.AddForce(-rb.linearVelocityY * Vector2.up,ForceMode2D.Impulse);
		}
		float speedDif = Data.slideSpeed - rb.linearVelocityY;	
		float movement = speedDif * Data.slideAccel;
		movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif)  * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

		rb.AddForce(movement * Vector2.up);
	}
    #endregion
    #region CHECK METHODS
    public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight)
			Turn();
	}
    private bool CanJump()
    {
		return LastOnGroundTime > 0 && !IsJumping;
    }

	private bool CanWallJump()
    {
		return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
			 (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
	}

	private bool CanJumpCut()
    {
		return IsJumping && rb.linearVelocityY > 0;
    }

	private bool CanWallJumpCut()
	{
		return IsWallJumping && rb.linearVelocityY > 0;
	}

	public bool CanSlide()
    {
		if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && LastOnGroundTime <= 0)
			return true;
		else
			return false;
	}
    #endregion
    void SetGravityScale(float gravityScale)
    {
        rb.gravityScale = gravityScale;
    }
    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
		Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
	}
    #endregion
	#region SpeepUp
	public void SpeepUp(float time, float amount)
	{
		StartCoroutine(BoostSpeed(time,amount));
	}
	private IEnumerator BoostSpeed(float time,float amount) 
	{ 
		Data.runMaxSpeed += amount;
		yield return new WaitForSeconds(time); 
		Data.runMaxSpeed -= amount;
	}
	#endregion
}