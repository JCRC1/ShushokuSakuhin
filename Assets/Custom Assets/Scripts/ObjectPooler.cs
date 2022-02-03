using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolNote
{
    public GameObject noteToPool;
    public int poolAmount;
    public bool shouldExpand = true;

}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;

    public List<GameObject> pooledNotes;
    public List<ObjectPoolNote> notesToPool;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pooledNotes = new List<GameObject>();
        foreach (ObjectPoolNote note in notesToPool)
        {
            for (int i = 0; i < note.poolAmount; i++)
            {
                GameObject obj = (GameObject)Instantiate(note.noteToPool);
                obj.SetActive(false);
                pooledNotes.Add(obj);
                obj.transform.parent = transform;
            }
        }
    }

    public GameObject GetPooledNote(string tag)
    {
        for (int i = 0; i < pooledNotes.Count; i++)
        {
            if (!pooledNotes[i].activeInHierarchy && pooledNotes[i].tag == tag)
            {
                return pooledNotes[i];
            }
        }
        foreach (ObjectPoolNote note in notesToPool)
        {
            if (note.noteToPool.tag == tag)
            {
                if (note.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(note.noteToPool);
                    obj.SetActive(false);
                    pooledNotes.Add(obj);
                    obj.transform.parent = transform;
                    return obj;
                }
            }
        }
        return null;
    }
}
