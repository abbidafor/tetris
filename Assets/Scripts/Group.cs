using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Group : MonoBehaviour
{
  // time of the last fall, used to auto fall after
  // time parametrized by 'level'
  private float lastFall;

  // last key pressed time, to handle long press behaviour
  private float lastKeyDown;
  private float timeKeyPressed;

  // public void AlignCenter()
  // {
  //   transform.position += transform.position - Utils.Center(gameObject);
  // }


  bool isValidGridPos()
  {
    foreach (Transform child in transform)
    {
      Vector2 v = Grid.roundVector2(child.position);

      // Not inside Border?
      if (!Grid.insideBorder(v))
        return false;

      // Block in grid cell (and not part of same group)?
      if (Grid.grid[(int)v.x, (int)v.y] != null &&
          Grid.grid[(int)v.x, (int)v.y].parent != transform)
        return false;
    }
    return true;
  }


  // update grid
  void updateGrid()
  {
    // Remove old children from grid
    for (int y = 0; y < Playfield.h; ++y) {
      for (int x = 0; x < Playfield.w; ++x)
      {
        if (Playfield.grid[x, y] != null &&
            Playfield.grid[x, y].parent == transform) {
          Playfield.grid[x, y] = null;
        }
      }
    }

    insertOnGrid();
  }

  void insertOnGrid()
  {
    // Add new children to grid
    foreach (Transform child in transform)
    {
      Vector2 v = Playfield.roundVec2(child.position);
      Playfield.grid[(int)v.x, (int)v.y] = child;
    }
  }

  void gameOver()
  {
    Debug.Log("GAME OVER");
    while (!isValidGridPos())
      {
        //Debug.LogFormat("Updating last group...: {0}", transform.position);
        transform.position += new Vector3(0, 1, 0);
      }
    updateGrid(); // to not overleap invalid groups
    enabled = false; // disable script when dies
    //UIController.gameOver(); // active Game Over panel
    //HighScore.Set(ScoreManager.score); // set highscore
    //Music.stopMusic(); // stop Music
  }


// Use this for initialization
void Start()
  {
    lastFall = Time.time;
    lastKeyDown = Time.time;
    timeKeyPressed = Time.time;
    if (isValidGridPos()) {
      insertOnGrid();
    }
    else {
      Debug.Log("KILLED ON START");
      gameOver();
    }
  }

  void tryChangePos(Vector3 v) {
    // modify position
    transform.position += v;

    // See if valid
    if (isValidGridPos()) {
      updateGrid();
    }
    else {
      transform.position -= v;
    }
  }

  void fallGroup() {
    // modify
    transform.position += new Vector3(0, -1, 0);

    if (isValidGridPos()) {
      // It's valid. Update grid... again
      updateGrid();
    }
    else{
      // it's not valid. revert
      transform.position += new Vector3(0, 1, 0);

      // Clear filled horzontal lines
      Grid.deleteFullRows();

      FindObjectOfType<Spawner>().spawnNext();

      // Disable script
      enabled = false;
    }

    lastFall = Time.time;
    
  }

  // getKey if is pressed now on longer pressed by 0.5 seconds
  // if that true apply the key each 0.5f while is pressed
  bool getKey(KeyCode key) {
    bool keyDown = Input.GetKeyDown(key);
    bool pressed = Input.GetKey(key) && Time.time - lastKeyDown > 0.5f && Time.time - timeKeyPressed > 0.05f;

    if (keyDown) {
      lastKeyDown = Time.time;
    }
    if (pressed) {
      timeKeyPressed = Time.time;
    }

    return keyDown || pressed;
  }
  

  // Update is called once per frame
  void Update()
  {
    // if (UIController.isPaused) {
    //   return; // don't do anything
    // }
    if (getKey(KeyCode.LeftArrow)) {
      tryChangePos(new Vector3(-1, 0, 0));
    }
    else if (getKey(KeyCode.RightArrow)) { // move right
      tryChangePos( new Vector3(1, 0, 0));
    }
    else if (getKey(KeyCode.UpArrow) && gameObject.tag != "Cube") { // rotate
      transform.Rotate(0, 0, -90);

      // see if valid
      if (isValidGridPos()) {
        // it's valid. update grid
        updateGrid();
      }
      else {
        // it's not valid. revert
        transform.Rotate(0, 0, 90);
      }
    }
    else if (getKey(KeyCode.DownArrow) || (Time.time - lastFall) >= (float)1 / Mathf.Sqrt(LevelManager.level)) {
      fallGroup();
    } 
    else if (Input.GetKeyDown(keyCode.Space)) {
      while (enabled) { // fall until the bottom
        fallGroup();
      }
    }

    // // Move Left
    // if (Input.GetKeyDown(KeyCode.LeftArrow))
    // {
    //   // Modify position
    //   transform.position += new Vector3(-1, 0, 0);

    //   // See if it's valid
    //   if (isValidGridPos())
    //     // It's valid. Update grid
    //     updateGrid();
    //   else
    //     // It's not valid. revert.
    //     transform.position += new Vector3(1, 0, 0);
    // }

    // // Move Right
    // if (Input.GetKeyDown(KeyCode.RightArrow))
    // {
    //   // Modify position
    //   transform.position += new Vector3(1, 0, 0);

    //   // See if it's valid
    //   if (isValidGridPos())
    //     // It's valid. Update grid
    //     updateGrid();
    //   else
    //     // It's not valid. revert.
    //     transform.position += new Vector3(-1, 0, 0);
    // }

    // // Rotate
    // else if (Input.GetKeyDown(KeyCode.UpArrow))
    // {
    //   transform.Rotate(0, 0, -90);

    //   // See if valid
    //   if (isValidGridPos())
    //     // It's valid. Update grid
    //     updateGrid();
    //   else
    //     // It's not valid. revert
    //     transform.Rotate(0, 0, 90);
    // }

    // // Move downwards and Fall
    // else if (Input.GetKeyDown(KeyCode.DownArrow) ||
    //         Time.time - lastFall >=1)
    // {
    //   // Modify position
    //   transform.position += new Vector3(0, -1, 0);

    //   // See if valid
    //   if (isValidGridPos())
    //   {
    //     // It's valid. Update grid
    //     updateGrid();
    //   }
    //   else
    //   {
    //     // It's not valid. revert
    //     transform.position += new Vector3(0, 1, 0);

    //     // Clear filled horizontal lines
    //     Playfield.deleteFullRows();

    //     // Spawn next Group
    //     FindObjectOfType<Spawner>().spawnNext();

    //     // Disable script
    //     enabled = false;
    //   }

    //   lastFall = Time.time;
    // }
  }
}
