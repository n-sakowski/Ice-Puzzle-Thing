using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class FinishChecker : MonoBehaviour
{

    [SerializeField] private AudioClip finishSoundClip;

    [Header("Finish Tile Settings")]
    public Tilemap finishTilemap;   
    public Tile finishTile;         

    [Header("UI Settings")]
    public GameObject winPanel;     

    private bool hasWon = false;    

    private void Update()
    {
        if (hasWon) return;

        CheckFinish();
    }

    private void CheckFinish()
    {
        if (finishTilemap == null || finishTile == null) return;

        Vector3Int playerCell = finishTilemap.WorldToCell(transform.position);

        TileBase tileAtPlayer = finishTilemap.GetTile(playerCell);

        if (tileAtPlayer == finishTile)
        {
            hasWon = true;
            Debug.Log("You Win!");

            SoundFXManager.instance.PlaySoundFXClip(finishSoundClip, transform, 0.35f);


            // StartCoroutine(DelayedMessage());

            

            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }
        }

        

        

    }

    /*
    private IEnumerator DelayedMessage()
    {
        yield return new WaitForSeconds(2f);   
        Debug.Log("Press R to restart the level or press ENTER to play another level!");
    }
    */

}
