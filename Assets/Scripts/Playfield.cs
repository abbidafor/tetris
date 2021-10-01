// using UnityEngine;
// using System.Collections;

// public class Playfield : MonoBehaviour
// {
//   // Round 
//   public static Vector2 roundVec2(Vector2 v)
//   {
//     return new Vector2(Mathf.Round(v.x),
//                        Mathf.Round(v.y));

//   }

//   // Inside border 
//   public static bool insideBorder(Vector2 pos)
//     {
//       return ((int)pos.x >= 0 &&
//               (int)pos.x < w &&
//               (int)pos.y >= 0);
//     }

//   // Delete row 
//   public static void deleteRow(int y)
//     {
//         for (int x = 0; x < w; ++x)
//         {
//           Destroy(grid[x, y].gameObject);
//           grid[x, y] = null;
//         }
//     }

//   // Decrease row
//   public static void decreaseRow(int y)
//     {
//         for (int x = 0; x < w; ++x)
//         {
//             if (grid[x, y] != null)
//             {
//                 // Move one towards bottom
//                 grid[x, y - 1] = grid[x, y];
//                 grid[x, y] = null;

//                 // Update block position
//                 grid[x, y - 1].position += new Vector3(0, -1, 0);
//             }
//         }
//     }

//   // Decrease rows above
//   public static void decreaseRowsAbove(int y)
//     {
//         for (int i = y; i < h; ++i)
//         {
//             decreaseRow(i);
//         }
//     }

//   // Is row full
//   public static bool isRowFull(int y)
//     {
//         for (int x = 0; x < w; ++x)
//         {
//             if (grid[x, y] == null)
//             {
//                 return false;
//             }
//         }
//         return true;
//     }

//   // Delete full rows
//   public static void deleteFullRows()
//     {
//         for (int y = 0; y < h; ++y)
//         {
//             if (isRowFull(y))
//             {
//                 deleteRow(y);
//                 decreaseRowsAbove(y + 1);
//                 y--;
//             }
//         }
//     }
// } 
