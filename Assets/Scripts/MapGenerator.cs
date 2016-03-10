using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GenerationType { Walker, DelaunayTriangulation, ZeldaRooms }
public class MapGenerator : MonoBehaviour {

    public GenerationType generationType = GenerationType.Walker;
    public int LevelSize = 250;
    public float unitScale = 2f;
    //public List<Sprite> floorSprites;
    public List<GameObject> floorPrefabs;
    //public List<Sprite> wallSprites;
    public List<GameObject> wallPrefabs;
    public GameObject player;
    public List<GameObject> Enemies;

    List<FloorWalker> walkers = new List<FloorWalker>();
    public List<GameObject> placedFloors = new List<GameObject>();
    List<GameObject> placedWalls = new List<GameObject>();
    List<Vector2> spawnLocations = new List<Vector2>();
    float movesSinceLastWalkerSpawn = 0;
    float rotationdegrees = 0;

    int floorCount = 0;

    bool GenerationComplete = false;

	void Start () {
        WalkerStart();
	}
	
	void Update () {
        //if (!GenerationComplete)
            //WalkerUpdate();
	}

    void WalkerStart() {
        // Spawn the floorwalker and place the first floor
        walkers.Clear();
        walkers.Add(new FloorWalker(0, 0));
        SpawnFloor(walkers[0].location);

        while (!GenerationComplete) {
            WalkerUpdate();
        }
    }

    void WalkerUpdate() {
        // Loop through and move each walker
        for (int i = 0; i < walkers.Count; i++) {
            if (walkers[i].RotationChance(0.3f, 0.03f)) { // random this
                spawnLocations.Add(walkers[i].location);
            }
            walkers[i].Move(unitScale);
            SpawnFloor(walkers[i].location);

            // Chance to create new walker
            if (Random.Range(0f, 1f) < ((movesSinceLastWalkerSpawn / (LevelSize / 5.5)) * (movesSinceLastWalkerSpawn / (LevelSize / 5.5)))) {
                //Debug.Log("Spawning New Walker [" + floorCount + "]");
                walkers.Add(new FloorWalker(walkers[i].location));
                movesSinceLastWalkerSpawn = 0;
            } else {
                movesSinceLastWalkerSpawn++;
            }
        }

        // if there are more then 1 floorwalker there is an increasing chance to destroy a random one
        if (walkers.Count > 1) {
            if (Random.Range(0f, 1f) < (((movesSinceLastWalkerSpawn*walkers.Count) / (LevelSize / 5)) * ((movesSinceLastWalkerSpawn * walkers.Count) / (LevelSize / 5)))) {
                //Debug.Log("Destroying a random floorwalker [" + floorCount + "]");
                walkers.RemoveAt(Random.Range(0, walkers.Count));
            }
        }

        // check level size against current floor count if floor count is >= exit floor placement
        if (floorCount > LevelSize) {
            GenerationComplete = true;

            SpawnPlayer();
            SpawnEnemies();
            SpawnWalls();
        }
        // determine player spawn

        // spawn enemies away from player with no los
        // Enemies can spawn anywhere far enough from the Player as long as there are no chests there. We just check all the Floors and give them a random chance to spawn some Enemies depending on  the current difficulty. Enemy type and rarity depends on the area. For example: a tile in the desert can spawn either a Bandit, Maggot, Corpse, Scorpion or a group of unwise Bandits warming themselves around an explosive barrel.

        // spawn walls in a non-dumb way
        // 4 different tiles for each tile, 22% chance to use the second or third and 2% to use the fourth else use the first
    }

    void SpawnFloor(Vector2 position, bool spawnedFromWalker = true) {
        // check that there is not a floor already here
        if (FloorAtPosition(position))
            return;
        
        //there is a chance the floor that is place will place a [50%] 2x2
        if (Random.Range(0f, 1f) < 0.3f && spawnedFromWalker) {
            // 2x2
            SpawnFloor(new Vector2(position.x + unitScale, position.y), false);
            SpawnFloor(new Vector2(position.x, position.y + unitScale), false);
            SpawnFloor(new Vector2(position.x + unitScale, position.y + unitScale), false);
        }

        int index = Random.Range(0, floorPrefabs.Count-3);
        float chance = Random.Range(0f, 1f);
        if (chance < 0.15) {
            if (Random.Range(0f, 1f) < 0.5f)
                index = floorPrefabs.Count - 3;
            else
                index = floorPrefabs.Count - 2;
        } else if (chance < 0.01) {
            index = floorPrefabs.Count - 1;
        }


        GameObject temp = (GameObject) Instantiate(floorPrefabs[index], position, Quaternion.identity);
        temp.transform.parent = this.transform;
        temp.name = "Floor_" + position.x + "_" + position.y;
        temp.transform.position = new Vector3(position.x, 0f, position.y);
        
        temp.transform.Rotate(new Vector3(1f, 0f, 0f), 90f);
        //SpriteRenderer tempSR = temp.AddComponent<SpriteRenderer>();
        //tempSR.sprite = floorSprites[index];
        //temp.transform.parent = this.transform;
        //temp.name = "Floor_" + floorCount;
        placedFloors.Add(temp);
        floorCount++;
    }

    void SpawnPlayer() {
        // Better solution - Spawn a room off the edge of the map that spawns the players inside that has a door that opens to the outside
        
        // Figure out a nice deadend for the player spawn
        // find a block with 3 surrounding walls
        for (int i = 0; i < placedFloors.Count; i++) {
            int count = 0;

            if (!FloorAtPosition(placedFloors[i].transform.position + new Vector3(unitScale, 0f, 0f))) {
                count++;
            }
            if (!FloorAtPosition(placedFloors[i].transform.position + new Vector3(-unitScale, 0f, 0f))) {
                count++;
            }
            if (!FloorAtPosition(placedFloors[i].transform.position + new Vector3(0f, 0f, unitScale))) {
                count++;
            }
            if (!FloorAtPosition(placedFloors[i].transform.position + new Vector3(0f, 0f, -unitScale))) {
                count++;
            }

            if (count > 2) {
                player.transform.position = placedFloors[i].transform.position;
                return;
            }
        }

        Debug.Log("No spot found.");
    }

    void SpawnEnemies() {
        int enemyCount = 0;
        for (int i = 0; i < placedFloors.Count; i++) {
            if (Random.Range(0f, 1f) < 0.05f) {
                if (Vector3.Distance(placedFloors[i].transform.position, player.transform.position) > 5f) {
                    SpawnEnemy(placedFloors[i].transform.position);
                    enemyCount++;
                }
            }
        }

        Debug.Log("Spawned " + enemyCount + " enemies.");
    }

    void SpawnWalls() {
        for (int i = 0; i < placedFloors.Count; i++) {
            if (!FloorAtPosition(placedFloors[i].transform.position + new Vector3(unitScale, 0f, 0f))) {
                SpawnWall(placedFloors[i].transform.position + new Vector3(unitScale, 0f, 0f));
            }
            if (!FloorAtPosition(placedFloors[i].transform.position + new Vector3(-unitScale, 0f, 0f))) {
                SpawnWall(placedFloors[i].transform.position + new Vector3(-unitScale, 0f, 0f));
            }
            if (!FloorAtPosition(placedFloors[i].transform.position + new Vector3(0f, 0f, unitScale))) {
                SpawnWall(placedFloors[i].transform.position + new Vector3(0f, 0f, unitScale));
            }
            if (!FloorAtPosition(placedFloors[i].transform.position + new Vector3(0f, 0f, -unitScale))) {
                SpawnWall(placedFloors[i].transform.position + new Vector3(0f, 0f, -unitScale));
            }
        }
    }

    void SpawnWall(Vector3 position) {
        //if (!WallAtPosition(position)) {
            int index = 0;
            if (Random.Range(0f, 1f) < 0.05f)
                index = 1;

            GameObject temp = (GameObject) Instantiate(wallPrefabs[index], position, Quaternion.identity);
            temp.transform.parent = this.transform;
            temp.name = "Wall_" + position.x + "_" + position.y;

            placedWalls.Add(temp);
        //}
    }

    void SpawnEnemy(Vector3 position) {
        GameObject temp = (GameObject) Instantiate(Enemies[Random.Range(0, Enemies.Count)], position, Quaternion.identity);
        
        temp.name = "Enemy";
    }

    public bool FloorAtPosition(Vector3 position) {
        for (int i = 0; i < placedFloors.Count; i++) {
            if (Mathf.RoundToInt(placedFloors[i].transform.position.x) == Mathf.RoundToInt(position.x) && Mathf.RoundToInt(placedFloors[i].transform.position.y) == Mathf.RoundToInt(position.y) && Mathf.RoundToInt(placedFloors[i].transform.position.z) == Mathf.RoundToInt(position.z)) {
                //Debug.Log("Floor at position " + position + " | " + placedFloors[i].transform.position + " | Floor (" + Mathf.RoundToInt(placedFloors[i].transform.position.x) + ", " + Mathf.RoundToInt(placedFloors[i].transform.position.y) + ", " + Mathf.RoundToInt(placedFloors[i].transform.position.z) + ") | Pos (" + Mathf.RoundToInt(position.x) + ", " + Mathf.RoundToInt(position.y) + ", " + Mathf.RoundToInt(position.z) + ")");
                return true;
            }
        }

        return false;
    }

    public GameObject GetFloorAtPosition(Vector3 position) {
        for (int i = 0; i < placedFloors.Count; i++) {
            if (placedFloors[i].transform.position == position)
                return placedFloors[i];
        }

        return null;
    }

    bool WallAtPosition(Vector3 position) {
        for (int i = 0; i < placedWalls.Count; i++) {
            if (Mathf.RoundToInt(placedWalls[i].transform.position.x) == Mathf.RoundToInt(position.x) && Mathf.RoundToInt(placedWalls[i].transform.position.y) == Mathf.RoundToInt(position.y))
                return true;
        }

        return false;
    }
}
