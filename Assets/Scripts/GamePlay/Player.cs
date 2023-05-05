using UnityEngine;
using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour, IPunObservable
{
	public PhotonView view;
	public TextMeshPro NickNameText;
	public TextMeshPro DamageText;
	public Text Health;
	public int gainDamage;
	public string damageTo;

	public static Player instance;

	public int health;

	public enum ProjectAxis { onlyX = 0, xAndY = 1 };
	public ProjectAxis projectAxis = ProjectAxis.onlyX;
	public float speed = 150;
	public int extraJumps = 1;
	public float addForce = 7;
	public bool lookAtCursor;
	public KeyCode leftButton = KeyCode.A;
	public KeyCode rightButton = KeyCode.D;
	public KeyCode upButton = KeyCode.W;
	public KeyCode downButton = KeyCode.S;
	public KeyCode addForceButton = KeyCode.Space;
	public bool isFacingRight = true;
	private Vector3 direction;
	private float vertical;
	private float horizontal;
	private Rigidbody2D body;
	private float rotationY;
	private bool jump;

	Vector3 pos;
	Camera main;
	Animator animator;
	SpriteRenderer spriteRenderer;

	private bool faceLeft;

	public bool dead;

	Results Table;
	Vector3 defaultScale;
	string lastShooterName;



	void Start()
	{
		instance = this;
		defaultScale = transform.localScale;
		Table = GameObject.Find("Table").GetComponent<Results>();
		Health = GameObject.Find("Health").GetComponent<Text>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		health = 100;
		animator = GetComponent<Animator>();
		main = FindObjectOfType<Camera>();
		body = GetComponent<Rigidbody2D>();
		body.fixedAngle = true;

		if (projectAxis == ProjectAxis.xAndY)
		{
			body.gravityScale = 0;
			body.drag = 10;
		}


		view = GetComponent<PhotonView>();
		Debug.Log(view.Owner.NickName);
		NickNameText.SetText(view.Owner.NickName);
		Table.NewPlayerJoined(view.Owner.NickName);

		if (!view.IsMine)
		{
			NickNameText.color = Color.green;
		}
	}

	public void CheckForHP()
    {
		if (health <= 0)
			StartCoroutine(Respawn());

    }

	public IEnumerator Respawn()
    {
		if (view.IsMine)
		{
			Table.RPCDead(view.Owner.NickName);
			Table.RPCKill(lastShooterName);
			Debug.Log("last shooter was = " + lastShooterName);
		}
		GameManager.instance.canShoot = false;
		LeanTween.scale(gameObject, Vector3.zero, 0.2f);
		body.Sleep();
		yield return new WaitForSeconds(2f);
		body.WakeUp();
		GameManager.instance.canShoot = true;
		LeanTween.scale(gameObject, defaultScale, 0.2f);
		view.RPC("ChangePos", RpcTarget.All);
		health = 100;
		UpdateHealth();
	}

	[PunRPC]
	public void ChangePos()
    {
		transform.position = SpawnPoint.instance.RandomSpawnPoint();
    }

	
	public void GetDamage(int damage, string shooterName)
    {
		view.RPC("getDamage", RpcTarget.All, damage, shooterName);
	}

	public void UpdateHealth()
    {
		if(view.IsMine)
		{
			Health.text = "" + health;
		}
	}

	[PunRPC]
	public IEnumerator getDamage(int damage, string shooterName)
	{
		lastShooterName = shooterName;
		health -= damage;
		UpdateHealth();
		CheckForHP();
		DamageText.text = "" + damage;
		LeanTween.scale(DamageText.gameObject, Vector3.one, 0.1f);
		spriteRenderer.color = Color.red;
		yield return new WaitForSeconds(0.2f);
		spriteRenderer.color = Color.white;
		LeanTween.scale(DamageText.gameObject, Vector3.zero, 0.2f);
	}


	void OnCollisionStay2D(Collision2D coll)
	{
		if (coll.transform.tag == "Ground")
		{
			body.drag = 10;
			jump = true;
		}
	}

	void OnCollisionExit2D(Collision2D coll)
	{
		if (coll.transform.tag == "Ground")
		{
			body.drag = 0;
			jump = false;
		}
	}

	void Update()
	{
		if (!view.IsMine)
			return;


		if (GameManager.instance.chat.textMessage.text != "")
			return;



		Flip();
		pos = main.WorldToScreenPoint(transform.position);

		body.AddForce(direction * body.mass * speed);

		if (Mathf.Abs(body.velocity.x) > speed / 100f)
		{
			body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * speed / 100f, body.velocity.y);
			animator.SetTrigger("Move");

		}

		if (projectAxis == ProjectAxis.xAndY)
		{
			if (Mathf.Abs(body.velocity.y) > speed / 100f)
			{
				body.velocity = new Vector2(body.velocity.x, Mathf.Sign(body.velocity.y) * speed / 100f);
				animator.SetTrigger("Move");
			}
		}
		else
		{
			if (Input.GetKey(addForceButton) && jump)
			{
				body.velocity = new Vector2(0, addForce);
			}
		}

		if (body.velocity.x < 0.5f && body.velocity.x > -0.5f)
			animator.SetTrigger("Idle");

		//================

		if (lookAtCursor)
		{
			Vector3 lookPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
			lookPos = lookPos - transform.position;
			float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}

		if (Input.GetKey(upButton)) vertical = 1;
		else if (Input.GetKey(downButton)) vertical = -1; else vertical = 0;

		if (Input.GetKey(leftButton)) horizontal = -1;
		else if (Input.GetKey(rightButton)) horizontal = 1; else horizontal = 0;

		if (projectAxis == ProjectAxis.onlyX)
		{
			direction = new Vector2(horizontal, 0);
		}
		else
		{
			if (Input.GetKeyDown(addForceButton)) speed += addForce; else if (Input.GetKeyUp(addForceButton)) speed -= addForce;
			direction = new Vector2(horizontal, vertical);
		}

		if (horizontal > 0 && !isFacingRight) Flip(); else if (horizontal < 0 && isFacingRight) Flip();
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(faceLeft);
		}
		else
		{
			faceLeft = (bool)stream.ReceiveNext();
		}
	}


	void Flip()
	{
        if (Input.mousePosition.x < pos.x)
		{
			transform.localRotation = Quaternion.Euler(0, 180, 0);
			faceLeft = true;
		}

		if (Input.mousePosition.x > pos.x)
		{
			transform.localRotation = Quaternion.Euler(0, 0, 0);
			faceLeft = false;
		}

		NickNameText.transform.localRotation = transform.localRotation;
		DamageText.transform.localRotation = transform.localRotation;
	}
}


		