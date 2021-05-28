using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquip : MonoBehaviour {
    public List<GameObject> head_prefab = new List<GameObject>();
    public List<GameObject> body_prefab = new List<GameObject>();

    private static string path = System.IO.Directory.GetCurrentDirectory() + "/Assets/Project_R/Resources/Equip/Prefab/";
    private string head_path = path + "Head";
    private string body_path = path + "Body";

    CharacterParts characterparts;

    void Awake() {
        characterparts = GameObject.Find("Player").GetComponent<CharacterParts>();
    }

    void Start() {
        Find_Path(head_path, "Head");
        Find_Path(body_path, "Body");
        //characterparts.ChangeHead(head_prefab[0]);
        //characterparts.ChangeChest(body_prefab[0]);
    }

    void Find_Path(string path, string category) {
        if(System.IO.Directory.Exists(path)) {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
            foreach (var item in di.GetFiles()) {
                if (item.Extension == ".prefab") {
                    string name = item.Name.Replace(".prefab", "");
                    if (category == "Head")
                        head_prefab.Add(Resources.Load<GameObject>("Equip/Prefab/" + category + "/" + name));
                    if (category == "Body")
                        body_prefab.Add(Resources.Load<GameObject>("Equip/Prefab/" + category + "/" + name));
                }
            }
        }else {
            Debug.Log("찾는 폴더가 없습니다.\n");
        }
    }
}
