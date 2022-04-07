using System;
using System.Collections.Generic;
using BadASCII;

namespace TextAdventure { 

enum GameState {
    Explore,
    Help,
    Menu,
    End
}

struct Action {
    public enum ActionType {
        None,

        North,
        South,
        East,
        West,

        Up,
        Down,

        Take,
        Give,
        Use,
        Search
    }

    public ActionType type;
    public string     arg;
}

struct PlayerData {
    public Locations.Location location;
    public Locations.Location previousLocation;

    public int                health;
    public int                maxHealth;

    public bool               piosoned; 
    public bool               givenBook;

    public Dictionary<string, int> inverntory;
}

class Program { 
    static private GameState  mGameState;

    static private string     mCurrentCommand;
    static private string     mLastMessage;
    static private PlayerData mPlayerData;

    static private void Init() {
        Console.CursorVisible = false;
        Console.SetBufferSize(150 + 1, 50 + 1);
        Console.SetWindowSize(150 + 1, 50 + 1);

        mPlayerData = new PlayerData();
        mPlayerData.location = Locations.Location.Forest;
        mPlayerData.maxHealth = 35;
        mPlayerData.health = mPlayerData.maxHealth;
        mPlayerData.piosoned = false;
        mPlayerData.givenBook = false;

        mPlayerData.inverntory = new Dictionary<string, int>();
        mPlayerData.inverntory.Add("berries", 0);
        mPlayerData.inverntory.Add("master key", 0);
        mPlayerData.inverntory.Add("book", 0);
        mPlayerData.inverntory.Add("potion", 0);

        PoisonPlayer();

        mGameState = GameState.Menu;
        mCurrentCommand = "";
        mLastMessage = "";

        Renderer.Init();
        Locations.Init();
    }

    static private string GetUserCommand() {
        if (Console.KeyAvailable) {
            ConsoleKey key  = Console.ReadKey(true).Key;

            // Check if its a letter.
            if ((int)key >= 65 && (int)key <= 90) {
                char letter = (char)((int)key + 32);
                mCurrentCommand += letter;
            }

            switch (key) {
            case ConsoleKey.Spacebar:
                mCurrentCommand += " ";
                break;

            case ConsoleKey.Backspace:
                if (mCurrentCommand.Length > 0) mCurrentCommand = mCurrentCommand.Substring(0, mCurrentCommand.Length - 1);
                break;

            case ConsoleKey.Enter:

                string command = mCurrentCommand;
                mCurrentCommand = "";
                return command;
            }
        }
        
        return "";
    }

    static private Action InterpretPlayerAction(string[] command) {
        Action action = new Action();
        if (Array.IndexOf(command, "north") != -1 || Array.IndexOf(command, "n") != -1) {
            action.type = Action.ActionType.North;
        }
        else if (Array.IndexOf(command, "south") != -1 || Array.IndexOf(command, "s") != -1) {
            action.type = Action.ActionType.South;
        }
        else if (Array.IndexOf(command, "east") != -1 || Array.IndexOf(command, "e") != -1) {
            action.type = Action.ActionType.East;
        }
        else if (Array.IndexOf(command, "west") != -1 || Array.IndexOf(command, "w") != -1) {
            action.type = Action.ActionType.West;
        }
        else if (Array.IndexOf(command, "up") != -1 || Array.IndexOf(command, "u") != -1) {
            action.type = Action.ActionType.Up;
        }
        else if (Array.IndexOf(command, "down") != -1 || Array.IndexOf(command, "d") != -1) {
            action.type = Action.ActionType.Down;
        }
        else if (command[0] == "take" || command[0] == "get") {
            action.type = Action.ActionType.Take;
            action.arg = command[1];
            if (string.IsNullOrEmpty(command[1])) SetMessage("What do you want to take?"); 
            else SetMessage("You can't find any " + command[1] + " here to take.");
        }
        else if (command[0] == "give") {
            action.type = Action.ActionType.Give;
            action.arg = command[1];
            if (string.IsNullOrEmpty(command[1])) SetMessage("What do you want to give?"); 
            else if (!mPlayerData.inverntory.ContainsKey(command[1])) SetMessage("You do not have a " + command[1] + " to give.");
            else SetMessage("There is no one to give the " + command[1] + " to.");
        }
        else if (command[0] == "search") {
            action.type = Action.ActionType.Search;
            action.arg = command[1];
            if (string.IsNullOrEmpty(command[1])) SetMessage("What do you want to search?"); 
            else SetMessage("You can't find anything.");
        }
        else if (command[0] == "eat" || command[0] == "drink" || command[0] == "use") {
            action.type = Action.ActionType.Use;
            action.arg = command[1];
            if (string.IsNullOrEmpty(command[1])) SetMessage("What do you want to " + command[0] + "?"); 
            else SetMessage("You don't have any " + command[1] + " to " + command[0] + " right now.");

            if (action.arg == "berries" && GetPlayerInfo().inverntory["berries"] > 0) {
                if (GetPlayerInfo().piosoned) SetMessage("You eat the berries and feel revitalized. The effects of the poison begin to slow.");
                mPlayerData.inverntory["berries"]--;
                SetMessage("You eat the berries and feel revitalized.");
                HealPlayer(10);
                HealPoison();
            }

            if (action.arg == "potion" && GetPlayerInfo().inverntory["potion"] > 0) {
                SetMessage("You drink the potion and begin to feel weightless. You start floating and hit your head on the ceiling, then fall back down.");
                mPlayerData.inverntory["potion"]--;
            }

        }
        else if (command[0] == "help") {
            mGameState = GameState.Help;
        }
        else {
            SetMessage("You try " + command[0] + "ing but you fail miserably.");
        }

        return action;
    }

    static private void Menu() {
        Renderer.DrawMenu();
        if (Console.KeyAvailable) {
            mGameState = GameState.Help;

            while (Console.KeyAvailable) {
                Console.ReadKey();
            }
        }
        System.Threading.Thread.Sleep(100);
    }

    static private void Help() { 
        Renderer.DrawHelp();
        if (Console.KeyAvailable) {
            mGameState = GameState.Explore;

            while (Console.KeyAvailable) {
                Console.ReadKey();
            }
        }
    }

    static private void EndGame() {
        Renderer.DrawEnd();
        if (Console.KeyAvailable) {
            mGameState = GameState.Menu;

            while (Console.KeyAvailable) {
                Console.ReadKey();
            }
        }
    }

    static private void Explore() {
        Action action = new Action();
        action.type = Action.ActionType.None;
        action.arg = "";

        string command = GetUserCommand();
        if (command != "") {
            string[] splitCommand = new string[5];
            Array.Copy(command.Split(' '), splitCommand, command.Split(' ').Length);
            action = InterpretPlayerAction(splitCommand); 
        }

        Locations.SelectLocation(mPlayerData.location, action);
    }


    static private bool RunFrame() {
        switch (mGameState) {

        case GameState.Menu:
            Menu();
            break;

        case GameState.Help:
            Help();
            break;

        case GameState.Explore:
            Explore();
            break;

        case GameState.End:
            EndGame();
            break;
        }

        return true;
    }

    static private void RunGame() {
        bool isRunning = true;
        while (isRunning) {
            Renderer.StartFrame();
            isRunning = RunFrame();
            Renderer.EndFrame();            

        }
    }

    static public PlayerData GetPlayerInfo() {
        return mPlayerData;
    }

    static public void SetPlayerLocation(Locations.Location location) {
        mPlayerData.previousLocation = mPlayerData.location;
        mPlayerData.location = location;
    }

    static public void PoisonPlayer() {
        DamagePlayer(20);
        mPlayerData.piosoned = true;
    }

    static public void HealPoison() {
        mPlayerData.piosoned = false;
    }

    static public void HealPlayer(int amount) {
        mPlayerData.health += amount;
        if (mPlayerData.health > mPlayerData.maxHealth) mPlayerData.health = mPlayerData.maxHealth;
    }

    static public void DamagePlayer(int amount) {
        mPlayerData.health -= amount;
        if (mPlayerData.health < 0) mPlayerData.health = 0;
    }

    static public void GivePlayerItems(string item, int amount) {
        mPlayerData.inverntory[item] += amount;
    }

    static public void SetPlayerHasGivenBook() {
        mPlayerData.givenBook = true;
        mPlayerData.inverntory["book"]--;
    }

    static public void SetMessage(string message) {
        mLastMessage = message;
    }

    static public string GetLastMessage() {
        return mLastMessage;
    }

    static public string GetCurrentCommand() {
        return mCurrentCommand;
    }

    static public GameState GetGameState() {
        return mGameState;
    }

    static public void SetGameState(GameState gameState) {
        mGameState = gameState;
    }

    static public void Main(string[] args) {
        Init();
        RunGame();

        Console.ReadKey();

    }
}

}
