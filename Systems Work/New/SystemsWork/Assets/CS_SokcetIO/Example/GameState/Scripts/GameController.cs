using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject GameContainer;
    [SerializeField]
    private Transform PlayersContainer;

    [SerializeField]
    private GameObject PlayerPrefab;

    private GameState State;
    private Dictionary<string, Transform> PlayersToRender;
    internal void StartGame(GameState state)
    {
        PlayersToRender = new Dictionary<string, Transform>();
        GameObject.Find("PanelConnect").SetActive(false);
        GameContainer.SetActive(true);

        foreach (Player player in state.Players)
        {
            GameObject playerGameObject = Instantiate(PlayerPrefab, PlayersContainer);
            playerGameObject.transform.position = new Vector2(player.x, player.y);
            playerGameObject.GetComponent<GamePlayer>().Id = player.Id;
            playerGameObject.GetComponent<GamePlayer>().Username = player.Id;

            PlayersToRender[player.Id] = playerGameObject.transform;
        }

        var Socket = NetworkController._Instance.Socket;

        InputController._Instance.onAxisChange += (axis) => { Socket.Emit("move", axis); };

        State = state;
        Socket.On("updateState", UpdateState);

        
    }

    private void UpdateState(string json)
    {
        GameStateData jsonData = JsonUtility.FromJson<GameStateData>(json);

        State = jsonData.State;

    }

    void Update()
    {
        if (State != null)
        {
            foreach (Player player in State.Players)
            {
                if (PlayersToRender.ContainsKey(player.Id))
                {
                    PlayersToRender[player.Id].position = new Vector2(player.x,player.y);
                }
               
                
            }
        }

    }


}

[Serializable]
public class GameStateData
{
    public GameState State;
}
