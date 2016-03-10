using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerMiniMap : MonoBehaviour {

    public MapGenerator map;
    public RawImage minimap;
    List<GameObject> floorsSeen = new List<GameObject>();
    List<GameObject> floorsScouted = new List<GameObject>();
    float timer = 0f;

    int mapscale = 10;
    int mapborder = 2;
    int scoutdistance = 4;
    int viewdistance = 6;

    void Start () {
	
	}
	
	void Update () {
        int currentx = Mathf.RoundToInt(transform.position.x - (scoutdistance * 0.5f * map.unitScale));
        int currenty = Mathf.RoundToInt(transform.position.z - (scoutdistance * 0.5f * map.unitScale));

        int endx = currentx + Mathf.RoundToInt(scoutdistance * map.unitScale);
        int endy = currenty + Mathf.RoundToInt(scoutdistance * map.unitScale);

        GameObject temp;

        for (int x = currentx; x < endx; x++) {
            for (int y = currenty; y < endy; y++) {
                temp = map.GetFloorAtPosition(new Vector3(x, 0f, y));
                if (temp && !floorsSeen.Contains(temp)) {
                    Ray ray = new Ray(transform.position + new Vector3(0f, 0.5f, 0f), (temp.transform.position - (transform.position + new Vector3(0f, 1f, 0f))).normalized);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit)) {

                        if (hit.collider.tag != "Wall")
                            floorsSeen.Add(temp);
                    }
                }
            }
        }

        currentx = Mathf.RoundToInt(transform.position.x - (viewdistance * 0.5f * map.unitScale));
        currenty = Mathf.RoundToInt(transform.position.z - (viewdistance * 0.5f * map.unitScale));

        endx = currentx + Mathf.RoundToInt(viewdistance * map.unitScale);
        endy = currenty + Mathf.RoundToInt(viewdistance * map.unitScale);

        for (int x = currentx; x < endx; x++) {
            for (int y = currenty; y < endy; y++) {
                temp = map.GetFloorAtPosition(new Vector3(x, 0f, y));
                if (temp && !floorsScouted.Contains(temp)) {
                    Ray ray = new Ray(transform.position + new Vector3(0f, 0.5f, 0f), (temp.transform.position - (transform.position + new Vector3(0f, 1f, 0f))).normalized);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit)) {

                        if (hit.collider.tag != "Wall")
                            floorsScouted.Add(temp);
                    }
                }
            }
        }
    }

    void LateUpdate() {
        int minx = 10000;
        int maxx = -10000;
        int miny = 10000;
        int maxy = -10000;

        for (int i = 0; i < floorsSeen.Count; i++) {
            // for each position add a 2x2 white dot to the minimap
            // get the min position and the max position in both x and y direction
            // this will makup the scale of the minimap
            if (floorsSeen[i].transform.position.x / map.unitScale < minx) {
                minx = Mathf.RoundToInt(floorsSeen[i].transform.position.x / map.unitScale);
            }
            if (floorsSeen[i].transform.position.x / map.unitScale > maxx) {
                maxx = Mathf.RoundToInt(floorsSeen[i].transform.position.x / map.unitScale);
            }

            if (floorsSeen[i].transform.position.z / map.unitScale < miny) {
                miny = Mathf.RoundToInt(floorsSeen[i].transform.position.z / map.unitScale);
            }
            if (floorsSeen[i].transform.position.z / map.unitScale > maxy) {
                maxy = Mathf.RoundToInt(floorsSeen[i].transform.position.z / map.unitScale);
            }
        }

        DrawMap(maxx, minx, maxy, miny);
    }

    void DrawMap(int maxx, int minx, int maxy, int miny) {
        timer += Time.deltaTime;
        if (timer > 0.25f) {
            if ((maxx - minx > 0) && (maxy - miny > 0)) {
                //Debug.Log("Width: " + (maxx - minx) + " | Height: " + (maxy - miny));
                Texture2D texture = new Texture2D((((maxx - minx) + mapborder + mapborder) * mapscale), (((maxy - miny) + mapborder + mapborder) * mapscale));
                texture.mipMapBias = 0;
                minimap.rectTransform.sizeDelta = new Vector2(((maxx - minx) + mapborder+ mapborder) * mapscale, ((maxy - miny) + mapborder+ mapborder) * mapscale);

                for (int x = 0; x < minimap.rectTransform.sizeDelta.x; x++) {
                    for (int y = 0; y < minimap.rectTransform.sizeDelta.y; y++) {
                        texture.SetPixel(x, y, new Color(0f, 0f, 0f, 0f));
                    }
                }

                int basex;
                int basey;

                // Draw the minimap view distance
                for (int i = 0; i < floorsScouted.Count; i++) {
                    basex = Mathf.RoundToInt((((floorsScouted[i].transform.position.x / map.unitScale) - minx) + mapborder) * mapscale);
                    basey = Mathf.RoundToInt((((floorsScouted[i].transform.position.z / map.unitScale) - miny) + mapborder) * mapscale);

                    for (int xoffset = 0; xoffset < mapscale; xoffset++) {
                        for (int yoffset = 0; yoffset < mapscale; yoffset++) {
                            texture.SetPixel(basex + xoffset, basey + (mapscale - yoffset), new Color(0.75f, 0.75f, 0.75f, 0.5f));
                        }
                    }
                }

                // Draw the minimap
                for (int i = 0; i < floorsSeen.Count; i++) {
                    basex = Mathf.RoundToInt((((floorsSeen[i].transform.position.x / map.unitScale) - minx) + mapborder) * mapscale);
                    basey = Mathf.RoundToInt((((floorsSeen[i].transform.position.z / map.unitScale) - miny) + mapborder) * mapscale);

                    for (int xoffset = 0; xoffset < mapscale; xoffset++) {
                        for (int yoffset = 0; yoffset < mapscale; yoffset++) {
                            texture.SetPixel(basex + xoffset, basey + (mapscale - yoffset), new Color(1f, 1f, 1f, 1f));
                        }
                    }
                }

                // Mark the player
                basex = Mathf.RoundToInt((((transform.position.x / map.unitScale) - minx) + mapborder) * mapscale);
                basey = Mathf.RoundToInt((((transform.position.z / map.unitScale) - miny) + mapborder) * mapscale);
                for (int xoffset = 0; xoffset < mapscale; xoffset++) {
                    for (int yoffset = 0; yoffset < mapscale; yoffset++) {
                        texture.SetPixel(basex + xoffset, basey + (mapscale - yoffset), new Color(0.1f, 0.65f, 0.35f, 1f));
                    }
                }

                texture.alphaIsTransparency = true;
                texture.wrapMode = TextureWrapMode.Repeat;
                texture.Apply();
                texture.filterMode = FilterMode.Point;
                //minimap.sprite = Sprite.Create(texture, new Rect(0f, 0f, (maxx - minx), (maxy - miny)), new Vector2(0.5f, 0.5f));
                minimap.texture = texture;

                timer = 0f;
            }
        }
    }
}
