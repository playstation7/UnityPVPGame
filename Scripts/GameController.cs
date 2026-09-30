using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{

    public GameObject wallHor;
    public GameObject wallVert;
    int borderY;
    int borderX;
    [SerializeField]
    public Sprite field;

    public GameObject greentank;
    public GameObject redtank;

    public List<Cell> cells = new List<Cell>();
    public List<Cell> freeCells = new List<Cell>();

    public List<Wall> walls = new List<Wall>();

    public Cell greenCell;
    public Cell redCell;


    public GameObject greenText;
    public GameObject redText;

    bool find = false;


    public GameObject[] buffs;

    private bool gameEnd = false;
    public bool gameEnding 
    {
        set 
        {
            if (gameEnd != true) 
            {
                gameEnd = true;
                StartCoroutine("gameStop");
            }
            
        }
    }

    IEnumerator gameStop() 
    {
        yield return new WaitForSeconds(3f);
        Time.timeScale = 0f;
        if (GameObject.Find("redtank") == null && GameObject.Find("greentank") == null)
        {
            SceneManager.LoadScene(1);
        }
        if (GameObject.Find("redtank") != null)
        {
            int num = Int32.Parse(GameObject.Find("redText").GetComponent<TextMeshProUGUI>().text);
            num++;
            Counter.red = num;
            GameObject.Find("redText").GetComponent<TextMeshProUGUI>().text = num.ToString();

        }
        if (GameObject.Find("greentank") != null)
        {
            int num = Int32.Parse(GameObject.Find("greenText").GetComponent<TextMeshProUGUI>().text);
            num++;
            Counter.green = num;
            GameObject.Find("greenText").GetComponent<TextMeshProUGUI>().text = num.ToString();

        }
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(1);
    }

    public struct Cell 
    {
        public int x;
        public int y;
        public bool reserve;
        public Cell(int x, int y, bool reserve = false) 
        {
            this.x = x;
            this.y = y;
            this.reserve = reserve;
            
                    
        }
    }

    public struct Wall
    {
        public float x;
        public float y;
        public Wall(float x, float y)
        {
            this.x = x;
            this.y = y;


        }
    }

    IEnumerator buffCreate() 
    {
        while(true) { 
        yield return new WaitForSeconds(5f);
        List<Cell> buffcells =
            (from num in freeCells
            where num.reserve == false
            select num).ToList();
        if (buffcells.Count > 0) 
        {
            int rand = Random.Range(0, buffcells.Count);
            int f = buffcells[rand].x;
            int d = buffcells[rand].y;
            Debug.Log(buffcells.Count);
            Cell cell = new Cell(f, d, true);
            freeCells[freeCells.IndexOf(new Cell(f, d))] = cell;
            Instantiate(buffs[Random.Range(0, buffs.Length)], new Vector3(f + 0.5f, d + 0.5f, 0f), Quaternion.Euler(0f, 0f, Random.Range(0, 360))).gameObject.GetComponent<buffActivate>().cell = cell;
        }
        }


    }

    // Start is called before the first frame update
    void Awake()
    {
        GameObject.FindAnyObjectByType<Singletone>().GetComponent<Singletone>().SoundCheck();

        StartCoroutine("buffCreate");
        Time.timeScale = 1f;
        borderY = Random.Range(2, 4);//4 
        borderX = Random.Range(2, 8);//8
        switch (borderX) 
        {
            case 2:
                Camera.main.GetComponent<Camera>().orthographicSize = borderY == 2 ? 2.5f : 3.5f;
                break;
            case 3:
                Camera.main.GetComponent<Camera>().orthographicSize = borderY == 2 ? 3f : 3.5f;
                break;
            case 4:
                Camera.main.GetComponent<Camera>().orthographicSize = borderY == 2 ? 3f : 4f;
                break;
            case 5:
                Camera.main.GetComponent<Camera>().orthographicSize = borderY == 2 ? 3.5f : 4.5f;
                break;
            case 6:
                Camera.main.GetComponent<Camera>().orthographicSize = borderY == 2 ? 3.6f : 4.5f;
                break;
            case 7:
                Camera.main.GetComponent<Camera>().orthographicSize = borderY == 2 ? 4.1f : 4.5f;
                break;
            default:
                Camera.main.GetComponent<Camera>().orthographicSize = 4.5f;
                break;
        };
        wallCreate();
        int gX, gY;
        int rX, rY;
        gX = Random.Range(-borderX, borderX);
        gY = Random.Range(-borderY, borderY);
        greentank.transform.position = new Vector3(gX + 0.5f, gY + 0.5f);
        greentank.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));//Random.Range(0f, 360f)
        greenCell = new Cell(gX, gY);
        rX = Random.Range(-borderX, borderX);
        rY = Random.Range(-borderY, borderY);
        redtank.transform.position = new Vector3(rX + 0.5f, rY + 0.5f); 
        redtank.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));//Random.Range(0f, 360f)
        redCell = new Cell(rX, rY);
        checkConnect(greenCell);
        if(find == false || (gX == rX && gY == rY )){ SceneManager.LoadScene(1); }

        //Debug.Log(FindObjectsOfType<AudioSource>()[0].gameObject);

    }

    private void Start()
    {
        greenText.GetComponent<TextMeshProUGUI>().text = Counter.green.ToString();
        redText.GetComponent<TextMeshProUGUI>().text = Counter.red.ToString();
    }

    private void wallCreate()
    {
        for (int y = -borderY; y <= borderY; y++)
        {
            for (int x = -borderX; x <= borderX; x++)
            {
                
                if (y == borderY)
                {
                    if (x != borderX)
                    {
                        GameObject gameObject =  Instantiate(wallHor, new Vector3(x + 0.5f, y, 0), Quaternion.identity);
                        walls.Add(new Wall(x + 0.5f,y));
                        gameObject.transform.SetParent(GameObject.Find("Walls").transform);
                        
                        cells.Add(new Cell(x, y));
                        continue;
                    }

                }
                if (y == -borderY)
                {
                    if (x != borderX) 
                    {
                        GameObject gameObject = Instantiate(wallHor, new Vector3(x + 0.5f, y, 0), Quaternion.identity);
                        walls.Add(new Wall(x + 0.5f, y));
                        gameObject.transform.SetParent(GameObject.Find("Walls").transform);
                        

                    }


                }
                else
                {

                    if (x != borderX && Random.Range(0, 3) == 0)
                    {
                        
                        GameObject gameObject = Instantiate(wallHor, new Vector3(x + 0.5f, y, 0), Quaternion.identity);
                        walls.Add(new Wall(x + 0.5f, y));
                        gameObject.transform.SetParent(GameObject.Find("Walls").transform);
                    }
                }
                if (x == borderX)
                {
                    if (y != borderY)
                    {
                        GameObject gameObject = Instantiate(wallVert, new Vector3(x, y + 0.5f, 0), Quaternion.identity);
                        walls.Add(new Wall(x, y + 0.5f));
                        gameObject.transform.SetParent(GameObject.Find("Walls").transform);
                        cells.Add(new Cell(x, y));
                        continue;
                    }
                    else if (y == borderY) continue;

                }
                if (x == -borderX)
                {
                    if (y != borderY)
                    {
                        GameObject gameObject = Instantiate(wallVert, new Vector3(x, y + 0.5f, 0), Quaternion.identity);
                        walls.Add(new Wall(x, y + 0.5f));
                        gameObject.transform.SetParent(GameObject.Find("Walls").transform);
                        
                    }
                }
                else
                {

                    if (Random.Range(0, 3) == 0)
                    {
                        GameObject gameObject = Instantiate(wallVert, new Vector3(x, y + 0.5f, 0), Quaternion.identity);
                        walls.Add(new Wall(x, y + 0.5f));
                        gameObject.transform.SetParent(GameObject.Find("Walls").transform);
                       
                    }
                }
                
                cells.Add(new Cell(x, y));

                //добавление поля в каждую клетку
                GameObject gb = new GameObject("field");
                gb.AddComponent<SpriteRenderer>().sprite = field;
                gb.GetComponent<SpriteRenderer>().sortingLayerName = "field";
                gb.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                gb.transform.parent = GameObject.Find("Fields").transform;
                //


            }
        }
        
        
    }


    public void checkConnect(Cell cell) 
    {
        
        //Debug.Log(cell.x + " , " + cell.y);
        if (cell.x == redCell.x && cell.y == redCell.y ) 
        {
            find = true;
        }
        freeCells.Add(new Cell(cell.x,cell.y));
        //up
        if (!walls.Contains(new Wall(cell.x + 0.5f, cell.y + 1f)) && cells.Contains(new Cell(cell.x, cell.y + 1)) && !freeCells.Contains(new Cell(cell.x, cell.y + 1)))
        {
            checkConnect(new Cell(cell.x, cell.y + 1));
        }
        
        //down
        if (!walls.Contains(new Wall(cell.x + 0.5f, cell.y)) && cells.Contains(new Cell(cell.x, cell.y - 1)) && !freeCells.Contains(new Cell(cell.x, cell.y - 1)))
        {
            checkConnect(new Cell(cell.x, cell.y - 1));
        }
        
        //right
        if (!walls.Contains(new Wall(cell.x + 1f, cell.y + 0.5f)) && cells.Contains(new Cell(cell.x + 1, cell.y)) && !freeCells.Contains(new Cell(cell.x + 1, cell.y)))
        {
            checkConnect(new Cell(cell.x + 1, cell.y));
        }
        
        //left
        if (!walls.Contains(new Wall(cell.x, cell.y + 0.5f)) && cells.Contains(new Cell(cell.x - 1, cell.y)) && !freeCells.Contains(new Cell(cell.x - 1, cell.y)))
        {
            checkConnect(new Cell(cell.x - 1, cell.y));
        }

        return;
        

    }


    

    // Update is called once per frame
    void Update()
    {
        
    }
}
