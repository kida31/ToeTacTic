using Godot;
using System;
using Game;

public partial class Main : Node
{
    private Match _currentMatch;
    private Player _lastPlayerWentFirst = Player.None;

    [Export] private Grid _gameGrid;

    [Export] private GUI _gui;
    
    private Logger logger;

    public override void _Ready()
    {
        logger = Logger.newForNode(this);
        logger.Info("Setup");

        logger.Info($"GameGrid = {_gameGrid?.Name}");
        logger.Info($"GUI = {_gui?.Name}");

        GetNode<Button>("%NewGameButton").Pressed += StartNewMatch;
        GetNode<Button>("%CloseGameButton").Pressed += CloseGame;

        StartNewMatch();
    }

    private void StartNewMatch()
    {
        logger?.Info("Setting up new game...");

        // Setup new Match
        if (_lastPlayerWentFirst == Player.None)
        {
            _currentMatch = new Match();
            _lastPlayerWentFirst = _currentMatch.CurrentPlayer;
        }
        else
        {
            // Take turns being first
            var playerGoesFirst = _lastPlayerWentFirst == Player.Circle ? Player.Cross : Player.Circle;
            _currentMatch = new Match(playerGoesFirst);
            _lastPlayerWentFirst = playerGoesFirst;
        }

        if (_gameGrid != null)
        {
            _gameGrid.SetMatch(_currentMatch);
        }
        else
        {
            logger?.Error("MISSING GAME GRID");
        }

        if (_gui != null)
        {
            _gui.SetMatch(_currentMatch);
        }
        else
        {
            logger?.Error("MISSING GUI");
        }


        logger?.Info("New game started");
    }

    private void CloseGame()
    {
        logger.Info("Closing game...");
        GetTree().Quit();
    }
}