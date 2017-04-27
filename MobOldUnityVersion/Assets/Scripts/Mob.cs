using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Mob : MonoBehaviour {
    
    public Slider widthSlider;
    public Slider distanceSlider;
    public Slider disorderSlider;
    public InputField numSoldiersInput;
    public Button destroyButton;
    public Button createButton;
    private List<GameObject> soldiers;

    void Start() {
        soldiers = new List<GameObject>();
        destroyButton.onClick.AddListener(DestroyMob);
        createButton.onClick.AddListener(GenerateMob);
    }

    private void DestroyMob() {
        foreach(GameObject g in soldiers) Destroy(g);
        soldiers = new List<GameObject>();
    }

    private int NumSoldiers() {
        int num;
        if(int.TryParse(numSoldiersInput.text, out num)) {
            return num;
        } else return 1;
    }

    private void GenerateMob() {
        int numberOfSoldiers = NumSoldiers();
        int formationWidth = (int) Mathf.Ceil(Mathf.Sqrt(numberOfSoldiers) * (widthSlider.value + 0.01f));
        float dist = distanceSlider.value * 10;
        int formationHeight = (int) Mathf.Ceil(numberOfSoldiers / formationWidth) + 1;
        for(int x = 0; x < formationWidth; x++) {
            for(int y = 0; y < formationHeight; y++) {
                if(y * formationWidth + x < numberOfSoldiers) {
                    float randX = Random.Range((x - disorderSlider.value) * dist, x * dist);
                    float randY = Random.Range((y - disorderSlider.value) * dist, y * dist);
                    PrimitiveType shape;
                    float rand = Random.Range(0.0f, 1.0f);
                    if(rand < 0.3f) shape = PrimitiveType.Sphere;
                    else if(rand < 0.6f) shape = PrimitiveType.Capsule;
                    else shape = PrimitiveType.Cube;
                    GameObject soldier = GameObject.CreatePrimitive(shape);
                    soldier.transform.position = new Vector3(randX, 1.0f, randY);
                    soldiers.Add(soldier);
                }
            }
        }
    }
}
