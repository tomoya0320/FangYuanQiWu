using UnityEngine;

public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T> {
  private static T m_instance;

  public static T Instance {
    get {
      if (m_instance == null) {
        GameObject obj = new GameObject(typeof(T).Name);
        m_instance = obj.AddComponent<T>();
      }
      return m_instance;
    }
  }
  protected virtual void Awake() {
    if (m_instance == null) {
      m_instance = this as T;
    } else {
      Destroy(gameObject);
    }
  }
}