using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class summonCharacter : MonoBehaviour

{
    public string characterName;
    public GameObject parentObject;

    private GameObject characterPrefab;

    // Start is called before the first frame update
    private void Start()
    {
        characterPrefab = Resources.Load<GameObject>("Prefabs/Battle/Buddy/player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick() {
        Debug.Log("ƒNƒŠƒbƒN‚³‚ê‚Ü‚µ‚½");
        GameObject characterClone = Instantiate(this.characterPrefab, this.transform);
        characterClone.transform.SetParent(parentObject.transform, false);
    }
}
