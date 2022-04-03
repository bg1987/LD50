using DefaultNamespace;
using UnityEngine;
public class Pipe : MonoBehaviour
{
    //checked every second
    [Range(0,1)]
    public float breakChance;

    public Breakable breakablePart;

    public SpriteRenderer mySprite;
    public Sprite pipeH;
    public Sprite pipeV;
    public Sprite pipeUR;
    public Sprite pipeUL;
    public Sprite pipeDR;
    public Sprite pipeDL;
    
    private float timePassed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        int x = (int) transform.position.x;
        int y = (int) transform.position.y;
        var myIndex = PipeGenerator.grid[x, y];

        //horizontal
        if (Mathf.Abs(PipeGenerator.Cell(x + 1, y) - myIndex) == 1 &&
            Mathf.Abs(PipeGenerator.Cell(x - 1, y) - myIndex) == 1)
        {
            SetOrientation(PipeOrientation.H);
        }
        //vertical
        if (Mathf.Abs(PipeGenerator.Cell(x, y + 1) - myIndex) == 1 &&
            Mathf.Abs(PipeGenerator.Cell(x, y - 1) - myIndex) == 1)
        {
            SetOrientation(PipeOrientation.V);
        }
        //UR
        if (Mathf.Abs(PipeGenerator.Cell(x, y - 1) - myIndex) == 1 &&
            Mathf.Abs(PipeGenerator.Cell(x + 1, y) - myIndex) == 1)
        {
            SetOrientation(PipeOrientation.UR);
        }
        //UL
        if (Mathf.Abs(PipeGenerator.Cell(x, y - 1) - myIndex) == 1 &&
            Mathf.Abs(PipeGenerator.Cell(x - 1, y) - myIndex) == 1)
        {
            SetOrientation(PipeOrientation.UL);
        }
        //DR
        if (Mathf.Abs(PipeGenerator.Cell(x, y + 1 ) - myIndex) == 1 &&
            Mathf.Abs(PipeGenerator.Cell(x + 1, y) - myIndex) == 1)
        {
            SetOrientation(PipeOrientation.DR);
        }
        //DL
        if (Mathf.Abs(PipeGenerator.Cell(x, y + 1 ) - myIndex) == 1 &&
            Mathf.Abs(PipeGenerator.Cell(x - 1, y) - myIndex) == 1)
        {
            SetOrientation(PipeOrientation.DL);
        }

    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > 1)
        {
            if (!breakablePart.broken && Random.value < breakChance)
            {
                breakablePart.Break();
            }

            timePassed = 0;
        }
    }

    void SetOrientation(PipeOrientation orientation)
    {
        switch (orientation)
        {
            case PipeOrientation.H:
                mySprite.sprite = pipeH;
                break;
            case PipeOrientation.V:
                mySprite.sprite = pipeV;
                break;
            case PipeOrientation.UR:
                mySprite.sprite = pipeUR;
                break;
            case PipeOrientation.UL:
                mySprite.sprite = pipeUL;
                break;
            case PipeOrientation.DR:
                mySprite.sprite = pipeDR;
                break;
            case PipeOrientation.DL:
                mySprite.sprite = pipeDL;
                break;
        }
    }
}
