using UnityEngine;

public class FloorWalker {
    public Vector2 location = new Vector2(0, 0);
    public Vector2 direction = new Vector2(1, 0);

    public FloorWalker(Vector2 vec) {
        location = vec;
        float dir = 1f;
        if (Random.Range(0f, 1f) > 0.5f) {
            dir = 1f;
        } else {
            dir = -1f;
        }
        if (Random.Range(0f, 1f) > 0.5f) {
            direction = new Vector2(dir, 0);
        } else {
            direction = new Vector2(0, dir);
        }
    }
    public FloorWalker(float x, float y) {
        location = new Vector2(x, y);
        float dir = 1f;
        if (Random.Range(0f, 1f) > 0.5f) {
            dir = 1f;
        } else {
            dir = -1f;
        }
        if (Random.Range(0f, 1f) > 0.5f) {
            direction = new Vector2(dir, 0);
        } else {
            direction = new Vector2(0, dir);
        }
    }

    void RotateWalker(float degrees) {
        // positive is clockwise, negative counter clockwise
        float d = Mathf.Deg2Rad * (-degrees);
        float new_x = (direction.x * Mathf.Cos(d)) - (direction.y * Mathf.Sin(d));
        float new_y = (direction.x * Mathf.Sin(d)) + (direction.y * Mathf.Cos(d));

        direction = new Vector2(new_x, new_y);
    }

    public void Move(float unitScale) {
        location = new Vector2(Mathf.RoundToInt(location.x + direction.x * unitScale), Mathf.RoundToInt(location.y + direction.y * unitScale));
    }

    public bool RotationChance(float rotationChance, float flipChance) {
        float chance = Random.Range(0f, 1f);
        if (chance < flipChance) {
            direction *= -1f;
            return true;
        } else if (chance < rotationChance) {
            if (Random.Range(0f, 1f) > 0.5f) {
                RotateWalker(90f);
            } else {
                RotateWalker(-90f);
            }
        }

        return false;
    }
}
