using UnityEngine;

public class AnimationPhase : MonoBehaviour {

    private Animator animator;
    [Range(0.0f, 1.0f)]
    public float speed;

    void Start() {
        if (GetComponent<Animator>()) {
            animator = GetComponent<Animator>();
            animator.Play(0, 0, speed);
        }
    }

}