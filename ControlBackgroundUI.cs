using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ControlBackgroundUI : MonoBehaviour {
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private float scaleMultiplier = 3f;

    private float rightEdge;
    private float leftEdge;
    private Vector3 distanceBetweenEdges;

    private void Start() {
        CalculateEdges();

        distanceBetweenEdges = new Vector3(0f, rightEdge - leftEdge, 0f);

    }

    private void CalculateEdges() {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        rightEdge = transform.position.y + spriteRenderer.bounds.extents.y / scaleMultiplier;
        leftEdge = transform.position.y - spriteRenderer.bounds.extents.y / scaleMultiplier;
    }

    private void Update() {
        transform.localPosition += scrollSpeed * Vector3.up * Time.deltaTime;

        if (PassedEdge()) {
            MoveRightSpriteToOppositeEdge();
        }
    }

    private bool PassedEdge() {
        return scrollSpeed > 0 && transform.position.y > rightEdge ||
            scrollSpeed < 0 && transform.position.y < leftEdge;
    }

    private void MoveRightSpriteToOppositeEdge() {
        if(scrollSpeed > 0) {
            transform.position -= distanceBetweenEdges;
        }
        else {
            transform.position +=distanceBetweenEdges;
        }
    }
}
