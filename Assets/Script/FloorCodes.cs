using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;


public class FloorCodes : MonoBehaviour
{
    public GameObject player;
    public GameObject player2;
    public Animator animator;
    static List<(int, int)> path = new List<(int, int)>();
    public GameObject gameoverPanel;
    public GameObject gameWinPanel;
    public GameObject skipPanel;
    public GameObject pauseButton;
    public GameObject gameStartPanel;
    public GameObject pausePanel;
    private Animator cameraanimator;
    public GameObject cameraAnimator;
    public GameObject cameraAnimator2;
    public int health = 1;
    public Material woodMaterial;
    public Material stoneMaterial;
    public Material metalMaterial;
    public static Material tempWoodMaterial;
    public GameObject fistPrefab; // Patlama efekti prefabýný ekleyin
    public GameObject fistEffectPrefab; // Patlama efekti prefabýný ekleyin



    public GameObject meteorskillImageClone;
    public GameObject changeRealityskillImageClone;
    public GameObject timeshiftskillImageClone;

    public static FloorCodes instance;


    //public AudioClip sesDosyasi; // Inspector'dan sürükleyip býrakarak ses dosyasýný atayýn



    /*gameobject'in hareketleri þu þekilde olmalýdýr, 
     * hedef -z yönündeyse rotationY = 0,
     * hedef +z yönündeyse rotationY = 180, 
     * hedef -x yönündeyse rotationY = 90,  
     * hedef +x yönündeyse rotationY = -90
     
     Yukarý giderse z artar,
    Aþþaðý giderse z azalýr,
    Sola giderse x azalýr,
    Aaða giderse x artar.
     
     
     */

    void Start()
    {
        if (Config.CurrentChapter < 7)
        {
            cameraanimator = cameraAnimator.GetComponent<Animator>();

        }
        else
        {
            cameraanimator = cameraAnimator2.GetComponent<Animator>();
        }

        tempWoodMaterial = woodMaterial;
        var floors = GameObject.FindGameObjectsWithTag("floor").ToList();
        foreach (var floor in floors)
        {
            float distance = Vector3.Distance(player.transform.position, floor.transform.position);
            if (distance == 0)
            {
                SkillControl.position = Convert.ToInt32(floor.name.Replace("floor", ""));
                SkillControl.oldPosition = SkillControl.position;
                break;
            }
        }

    }
    private void Awake()
    {
            instance = this;
    }


    private void Update()
    {
         
        tempWoodMaterial = woodMaterial;
        if (cameraanimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            skipPanel.SetActive(false);
        }

       
    }

    public static FloorCodes GetInstance()
    {
        return instance;
    }
    public void WinCheck()
    {
        if (SkillControl.isMeteorActive)
        {
            int rows = Config.ColAndRowCount;
            int cols = Config.ColAndRowCount;

            int[,] matrix = new int[rows, cols];
            int[,] matrix2 = new int[rows, cols];
            int num = 1;
            int current1 = 0;
            int current2 = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = num++;
                    matrix2[i, j] = matrix[i, j];
                    if (matrix[i, j] == SkillControl.position)
                    {
                        current1 = i;
                        current2 = j;
                    }
                }
            }

            List<(int, int)> path = FindEscapePath(current1, current2, matrix2, true);


            if (path == null || path.All(a => a.Item1 == current1 && a.Item2 == current2))
            {
                path = FindEscapePath(current1, current2, matrix2, false);
                if (path == null || path.All(a => a.Item1 == current1 && a.Item2 == current2))
                {
                    SkillControl.isMeteorActive = false;

                    Config.ChangeLevel();
                    Debug.Log("Kazandýnýz");
                    animator.Play("defeat");
                    GameWin();
                    //GameObject.Destroy(this.gameObject);
                    //SceneManager.LoadSceneAsync("Game"); 

                    return;
                }
            }


        }
    }

    void GameOver()
    {
        gameoverPanel.SetActive(true);
        gameStartPanel.SetActive(false);
        SkillControl.UpdateSkill();
        Config.ChangeLevelPoint();



    }

    void GameWin()
    {
        gameWinPanel.SetActive(true);
        gameStartPanel.SetActive(false);
        SkillControl.UpdateSkill();

    }

    public void GameOverButtonOnClick()
    {



        gameoverPanel.SetActive(false);
        PlayerPrefs.SetInt("SkipGoToEndOfAnimation", 1);
    }


    public void GameWinButtonOnClick()
    {


        gameWinPanel.SetActive(false);
        PlayerPrefs.SetInt("SkipGoToEndOfAnimation", 1);
    }




    private async void OnMouseDown()
    {
        SkillControl.timeshiftActive = true;

        if (animator.GetBool("isJumping"))
        {
            // Eðer animasyon oynanýyorsa, Input iþlemini engelle
            return;
        }
        if (SkillControl.oneHitActive)
        {
            health = 1;
            meteorskillImageClone.SetActive(false);
            changeRealityskillImageClone.SetActive(false);
            timeshiftskillImageClone.SetActive(false);

        }
        else
        {
            var mat1 = this.GetComponent<Renderer>().material;
            if (mat1.mainTexture.Equals(stoneMaterial.mainTexture))
            {
                health = 2;
            }
            if (mat1.mainTexture.Equals(metalMaterial.mainTexture))
            {
                health = 100;
                return;
            }


        }



        if (cameraanimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            return;
        }

        if (pausePanel.activeSelf)
        {
            return;
        }
        var floorCheckPosition = this.transform.position;
        var playerPosition = player.transform.position;
        if ((int)floorCheckPosition.x == (int)playerPosition.x && (int)floorCheckPosition.y == (int)playerPosition.y && (int)floorCheckPosition.z == (int)playerPosition.z)
        {
            return;
        }
        if (Config.CurrentChapterDifficult > 5)
        {

            var clickCount = PlayerPrefs.GetInt("ClickCount", 0);
            clickCount++;
            PlayerPrefs.SetInt("ClickCount", clickCount);
            if (clickCount > 2 && clickCount % 2 == 0)
            {

                var floors = Config.closedObjects;

                if (floors.Any())
                {
                    floors.ShuffleMe();
                    var floor = floors.FirstOrDefault(a => !a.activeSelf);
                    if (floor != null)
                    {
                        floor.SetActive(true);
                        floor.GetComponent<Renderer>().material = woodMaterial;

                    }


                }

            }
        }


        health--;
        if (health == 1)
        {
            this.GetComponent<Renderer>().material = woodMaterial;

        }
        else if (health == 0)
        {
            if (SkillControl.oneHitActive)
            {

                Vector3 floorPosition = this.transform.position;
                Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
                float xOffset = -0.40f; // X ekseninde eklemek istediðiniz deðer
                GameObject fist = Instantiate(fistPrefab, new Vector3(floorPosition.x + xOffset, floorPosition.y + 15, floorPosition.z), rotation);
                GameObject fisteffect = Instantiate(fistEffectPrefab, new Vector3(floorPosition.x, floorPosition.y, floorPosition.z), Quaternion.identity);
                Rigidbody fistRigidbody = fist.GetComponent<Rigidbody>();
                fistRigidbody.velocity = new Vector3(0, -40, 0);
                Destroy(fist, 1f);
                Destroy(fisteffect, 1f);
                SkillControl.oneHitActive = false;
                this.gameObject.SetActive(false);
                Config.closedObjects.Add(this.gameObject);

            }
            else
            {
                this.gameObject.SetActive(false);
                Config.closedObjects.Add(this.gameObject);
            }

        }

        if (this.transform.position == player.transform.position)
        {
            return;

        }
  

        int rows = Config.ColAndRowCount;
        int cols = Config.ColAndRowCount;

        int[,] matrix = new int[rows, cols];
        int[,] matrix2 = new int[rows, cols];
        int num = 1;
        int current1 = 0;
        int current2 = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = num++;
                matrix2[i, j] = matrix[i, j];
                if (matrix[i, j] == SkillControl.position)
                {
                    current1 = i;
                    current2 = j;
                }
            }
        }

        List<(int, int)> path = FindEscapePath(current1, current2, matrix2, true);


        if (path == null || path.All(a => a.Item1 == current1 && a.Item2 == current2))
        {
            path = FindEscapePath(current1, current2, matrix2, false);
            if (path == null || path.All(a => a.Item1 == current1 && a.Item2 == current2))
            {
                Config.ChangeLevel();
                Debug.Log("Kazandýnýz");
                animator.Play("defeat");
                GameWin();
                //GameObject.Destroy(this.gameObject);
                //SceneManager.LoadSceneAsync("Game"); 

                return;
            }
        }
        foreach (var step in path)
        {
            var t = matrix[step.Item1, step.Item2];



            //float distance = Vector3.Distance(player.transform.position, currentFloor.transform.position);
            if (t == SkillControl.position)
            {
                continue;
            }
            SkillControl.oldPosition = SkillControl.position;

            var currentFloor = GameObject.Find($"floor{t}");

            await MoveToTargetPosition(currentFloor.transform.position, 0.4f);



            SkillControl.position = Convert.ToInt32(currentFloor.name.Replace("floor", ""));

            int checkRow = 0;
            int checkCol = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (matrix[i, j] == SkillControl.position)
                    {
                        checkRow = i;
                        checkCol = j;
                    }
                }
            }
            if (checkRow == 0 || checkCol == 0)
            {
                Debug.Log("Kaybettiniz");
                GameObject.Destroy(this.gameObject);
                GameOver();
                return;
            }
            if (checkRow == 8 || checkCol == 8)
            {
                Debug.Log("Kaybettiniz");
                GameObject.Destroy(this.gameObject);
                GameOver();
                return;
            }
            //Debug.Log("ttt" + position);
            break;
        }





        //var model = models.OrderBy(a => a.Distance).FirstOrDefault();

        //if (model != null)
        //{

        //}
    }


    async Task MoveToTargetPosition(Vector3 target, float duration)
    {

        Vector3 startPosition = player.transform.position;
        float angel = 0;
        if ((int)target.x == (int)startPosition.x)
        {
            angel = target.z < startPosition.z ? 180 : 0;
        }
        else if ((int)target.z == (int)startPosition.z)
        {
            angel = target.x < startPosition.x ? 270 : 90;
        }
        Debug.Log(angel);
        player2.transform.rotation = Quaternion.Euler(0, angel, 0);



        //PlayerPrefs.SetInt("JumpIsActive", 1);      
        //animator.ResetTrigger("jump");
        //animator.Play("jump");


        // Animasyon geçiþini yönetmek için "isJumping" parametresini kullanýn
        animator.SetBool("isJumping", true);
        await Task.Yield(); // Bu satýr, animasyon geçiþine izin vermek için bir frame bekler


        float elapsedTime = 0;
        int frameRate = 60;
        float timePerFrame = 1f / frameRate;

        while (elapsedTime < duration)
        {
            await Task.Delay((int)(timePerFrame * 1000));
            elapsedTime += timePerFrame;
            float progress = elapsedTime / duration;

            player.transform.position = Vector3.Lerp(startPosition, target, progress);
        }

        player.transform.position = target;
        //PlayerPrefs.SetInt("JumpIsActive", 0);
        OnJumpAnimationEnd();

    }

    public void OnJumpAnimationEnd()
    {
        animator.SetBool("isJumping", false);
    }







    static List<(int, int)> FindEscapePath(int row, int col, int[,] matrix, bool isFirst)
    {
        var openList = new List<Node>();
        var closedList = new HashSet<Node>();
        var startNode = new Node(row, col);
        openList.Add(startNode);


        while (openList.Count > 0)
        {
            openList.Sort((n1, n2) => n1.F.CompareTo(n2.F));
            var currentNode = openList[0];

            if (IsEdgeCell(currentNode.Row, currentNode.Col, matrix))
            {
                return GetPath(currentNode);
            }

            openList.RemoveAt(0);
            closedList.Add(currentNode);

            int[,] neighbors = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };


            for (int i = 0; i < neighbors.GetLength(0); i++)
            {
                int newRow = currentNode.Row + neighbors[i, 0];
                int newCol = currentNode.Col + neighbors[i, 1];

                if (!IsValidMove(newRow, newCol, isFirst, matrix) || matrix[newRow, newCol] == 1)
                {
                    continue;
                }

                var neighborNode = new Node(newRow, newCol, currentNode);
                neighborNode.G = currentNode.G + 1;
                neighborNode.H = GetHeuristic(newRow, newCol, matrix);
                neighborNode.F = neighborNode.G + neighborNode.H;

                if (closedList.Contains(neighborNode))
                {
                    continue;
                }

                var openListNode = openList.Find(n => n.Equals(neighborNode));

                if (openListNode == null || openListNode.G > neighborNode.G)
                {
                    if (openListNode != null)
                    {
                        openList.Remove(openListNode);
                    }

                    openList.Add(neighborNode);
                }
            }
        }


        return null;
    }

    static List<(int, int)> GetPath(Node node)
    {
        var path = new List<(int, int)>();

        while (node != null)
        {
            path.Add((node.Row, node.Col));
            node = node.Parent;
        }

        path.Reverse();
        return path;
    }

    static int GetHeuristic(int row, int col, int[,] matrix)
    {
        int minDistance = Math.Min(row, Math.Min(col, Math.Min(matrix.GetLength(0) - 1 - row, matrix.GetLength(1) - 1 - col)));
        return minDistance;
    }

    static bool IsValidMove(int row, int col, bool isFirst, int[,] matrix)
    {
        var floor = GameObject.Find($"floor{matrix[row, col]}");
        if (floor == null)
        {
            return false;
        }
        if (!floor.activeSelf)
        {
            return false;
        }
        if (Config.CurrentChapterDifficult > 3)
        {
            if (!CheckNeigbourhMaterial(row, col, matrix))
            {
                if (isFirst && !IsNeighborFloorActive(row, col, matrix))
                {
                    return false;
                }
            }
        }


        return row >= 0 && row < matrix.GetLength(0) && col >= 0 && col < matrix.GetLength(1);
    }
    static bool IsNeighborFloorActive(int row, int col, int[,] matrix)
    {
        int[,] neighbors = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };
        int count = 0;
        for (int i = 0; i < neighbors.GetLength(0); i++)
        {
            int newRow = row + neighbors[i, 0];
            int newCol = col + neighbors[i, 1];
            if (newRow == 0 || newCol == 0)
            {
                continue;
            }
            if (newRow == 8 || newCol == 8)
            {
                continue;
            }

            if (newRow >= 0 && newRow < matrix.GetLength(0) && newCol >= 0 && newCol < matrix.GetLength(1))
            {
                var floor = GameObject.Find($"floor{matrix[newRow, newCol]}");
                if (floor == null || !floor.activeSelf)
                {
                    count++;

                }
            }
        }
        if (count > 1)
        {
            return false;
        }
        return true;
    }
    static bool CheckNeigbourhMaterial(int row, int col, int[,] matrix)
    {
        int[,] neighbors = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

        for (int i = 0; i < neighbors.GetLength(0); i++)
        {
            int newRow = row + neighbors[i, 0];
            int newCol = col + neighbors[i, 1];
            if (newRow == 0 || newCol == 0 || newRow == 8 || newCol == 8)
            {
                if (newRow >= 0 && newRow < matrix.GetLength(0) && newCol >= 0 && newCol < matrix.GetLength(1))
                {
                    var floor = GameObject.Find($"floor{matrix[newRow, newCol]}");
                    if (floor != null && floor.activeSelf)
                    {
                        var mat1 = floor.GetComponent<Renderer>()?.material;
                        if (mat1 != null && !mat1.mainTexture.Equals(tempWoodMaterial.mainTexture))
                        {
                            return true;
                        }
                    }
                }
            }


        }
        return false;
    }
    static bool IsEdgeCell(int row, int col, int[,] matrix)
    {
        return row == 0 || row == matrix.GetLength(0) - 1 || col == 0 || col == matrix.GetLength(1) - 1;
    }
}


public class DistanceModel
{
    public float Distance;
    public GameObject Floor;
}
class Node
{
    public int Row { get; set; }
    public int Col { get; set; }
    public Node Parent { get; set; }
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }

    public Node(int row, int col, Node parent = null)
    {
        Row = row;
        Col = col;
        Parent = parent;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Node)obj;
        return Row == other.Row && Col == other.Col;
    }

    public override int GetHashCode()
    {
        return Row.GetHashCode() ^ Col.GetHashCode();
    }


}

public static class DictionaryHelpers
{
    public static void ShuffleMe<T>(this IList<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;

        for (int i = list.Count - 1; i > 1; i--)
        {
            int rnd = random.Next(i + 1);

            T value = list[rnd];
            list[rnd] = list[i];
            list[i] = value;
        }
    }
}