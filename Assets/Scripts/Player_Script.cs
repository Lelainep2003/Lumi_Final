using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Player_Script : MonoBehaviour {
   public int speed; // the speed in seconds the player will move 
   public int score; // this is the player's score
   public int courage; // this is the number of lives the player has
   public static int finalScore;
   public AudioSource happyBarkSource;
   public AudioSource whineBarkSource;
   private int lastBarkScore = 0;
   public Sprite happySprite;
   public Sprite seriousSprite;
   private SpriteRenderer spriteRenderer; 
   private int runDirection = 1; // 1 = right, -1 = left
   private bool isRunningOff = false; // Flag to determine if player is running off screen
   private float currentSpeed = 22f; // Declares a private float variable to store the current speed, initially set to 0
   private float acceleration = 30f; // how fast player accelerates
   public TextMeshProUGUI courageText;
   public TextMeshProUGUI scoreText;

// --- INSTANTILIZATION --- 
   void Start() {
      speed = 20; // the speed in seconds the player will move 
      score = 0; // this is the player's score
      courage = 3; // this is the number of lives the player has
      spriteRenderer = GetComponent<SpriteRenderer>();
      spriteRenderer.sprite = happySprite; // Start with happy
      UpdateCourageUI();
      UpdateScoreUI();
      
   }

// --- UPDATE ---
   void Update() {
//  GAME OVER MOVEMENT (when out of courage)
      if (isRunningOff)
      {
         currentSpeed += acceleration * Time.deltaTime;
         transform.Translate(Vector3.right * runDirection * currentSpeed * Time.deltaTime);

         // Load Game Over scene when fully off screen
         if (Mathf.Abs(transform.position.x) > 39f)
         {
            SceneManager.LoadScene("GameOver");
         }

         return; // Skip normal movement when running off
      }
//  PLAYER MOVEMENT
      float moveInput = Input.GetAxisRaw("Horizontal");
      Debug.Log(moveInput); 

      transform.Translate(Vector3.right * moveInput * Time.deltaTime * speed);
      
 //  Flip Lumi left/right based on input
      if (moveInput < 0)
      {
         transform.localScale = new Vector3(1, 1, 1); // Face right
      }
      else if (moveInput > 0)
      {
         transform.localScale = new Vector3(-1, 1, 1); // Face left (flipped)
      }      
// SCREEN WRAPPING
      if (transform.position.x < -39f) {
         transform.position = new Vector3(39f, transform.position.y, 0f);
      }
      else if (transform.position.x > 39f)
      {
         transform.position = new Vector3(-39f, transform.position.y, 0f);
      }     
   }
   
// --END OF UPDATE--    

   IEnumerator Jump(float jumpHeight = 1.5f, float jumpDuration = 0.3f)
   {
      Vector3 originalPos = transform.localPosition;
      Vector3 targetPos = new Vector3(originalPos.x, originalPos.y + jumpHeight, originalPos.z);

      float elapsed = 0f;

      // Jump up
      while (elapsed < jumpDuration / 2f)
      {
         transform.localPosition = Vector3.Lerp(originalPos, targetPos, (elapsed / (jumpDuration / 2f)));
         elapsed += Time.deltaTime;
         yield return null;
      }

      // Jump down
      elapsed = 0f;
      while (elapsed < jumpDuration / 2f)
      {
         transform.localPosition = Vector3.Lerp(targetPos, originalPos, (elapsed / (jumpDuration / 2f)));
         elapsed += Time.deltaTime;
         yield return null;
      }

      transform.localPosition = originalPos;
   }
// score points will be called from the objects script when a collision occurs 
   void ScorePoints(int pointsToScore)
   {
      // Add points to the score
      score += pointsToScore;
      // Only bark every 3 bones after reaching at least 5
      if (score >= 5 && score / 3 > lastBarkScore / 3 && !happyBarkSource.isPlaying)
      {
         StartCoroutine(Jump()); 
         happyBarkSource.Play();
      }
      UpdateScoreUI(); // Updates score UI
   }

//LoseCourage will be called from the object script when a collision occurs between a falling object and a player
  public void LoseCourage() {
// Stop losing courage after Game Over starts
      if (isRunningOff || courage < 1)
         return;

      courage--; // subtract one courage
      UpdateCourageUI(); // lower courage on UI
      Debug.Log("Lumi lost courage! Current Courage: " + courage);
// Play whine bark every time she gets hit
      if (!whineBarkSource.isPlaying)
      {
         whineBarkSource.Play();
      }
// Change to serious sprite briefly
      spriteRenderer.sprite = seriousSprite;
      CancelInvoke("RevertToHappy"); // Make sure it doesn't stack
      Invoke("RevertToHappy", 0.5f); // Back to happy after 0.5 seconds
      
      if (courage < 1) {
         Debug.Log("Game Over");
         GameOver();
      }
   }
   // Change to happy sprite if serious
   void RevertToHappy() {
      spriteRenderer.sprite = happySprite;
   }
//Restore Courage
   public void RestoreCourage(int amount)
   {
      courage += amount;
      UpdateCourageUI();
      Debug.Log("Courage restored! Current Courage: " + courage);
   }
   public void UpdateCourageUI()
   {
      courageText.text = "Courage: " + courage;
   }
   void UpdateScoreUI()
   {
      scoreText.text = "Score: " + score;
   }
   
//If this function is called, the player character loses. The game goes to a 'Game Over' screen.
   public void GameOver() {
      
// Checks for courage = 0 then starts running off the screen 
      if (courage <= 0) {
         CancelInvoke("RevertToHappy"); // prevent reverting
         if (!whineBarkSource.isPlaying)
            whineBarkSource.Play();
         spriteRenderer.sprite = seriousSprite; // ← Make her serious

         finalScore = score; // Store score in static variable
         isRunningOff = true;
// Stop player movement 
         currentSpeed = 0f;  
         
// Set her running direction based on where she is closest
         if (transform.position.x < 0){
            runDirection = -1;  // run left
            transform.localScale = new Vector3(1, 1, 1); // face left
         }
         else {
            runDirection = 1; // run right
            transform.localScale = new Vector3(-1, 1, 1); // face left
         }
      }
   }
}


/*
//SCREEN BLOCKING
//checks if player is hitting the left side of the screen
      if (transform.position.x < -39f) {
         //moves player back to the edge of the screen
         transform.position = new Vector3(-39f, transform.position.y, 0f);
      }
//checks if player is hitting the left side of the screen
      if (transform.position.x > 39f) {
         //moves player back to the edge of the screen
         transform.position = new Vector3( 39f, transform.position.y, 0f);
      }
*/  
