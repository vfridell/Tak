using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TakLib;

namespace TakSOM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CommonOpenFileDialog _openFolderDialog;
        List<Tuple<BoardAnalysisData, Board>> _boardsToAnalyze = new List<Tuple<BoardAnalysisData, Board>>();

        public MainWindow()
        {
            InitializeComponent();
            _openFolderDialog = new CommonOpenFileDialog("Open PTN Folder");
            _openFolderDialog.IsFolderPicker = true;
            _openFolderDialog.InitialDirectory = null;
            _openFolderDialog.AddToMostRecentlyUsedList = false;
            _openFolderDialog.AllowNonFileSystemItems = false;
            _openFolderDialog.EnsureFileExists = true;
            _openFolderDialog.EnsurePathExists = true;
            _openFolderDialog.EnsureReadOnly = false;
            _openFolderDialog.EnsureValidNames = true;
            _openFolderDialog.Multiselect = false;
            _openFolderDialog.ShowPlacesList = true;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Analyze_OnClick(object sender, RoutedEventArgs e)
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };
            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("e1"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("a4"));
            game.ApplyMove(NotationParser.Parse("a5-"));
            game.ApplyMove(NotationParser.Parse("b4<"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("2a4>"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("Sb2"));
            game.ApplyMove(NotationParser.Parse("c5"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("Sc4"));
            game.ApplyMove(NotationParser.Parse("Cd3"));
            game.ApplyMove(NotationParser.Parse("c4<"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("b2>"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("e5-"));
            game.ApplyMove(NotationParser.Parse("Cb2"));
            game.ApplyMove(NotationParser.Parse("c2-"));
            game.ApplyMove(NotationParser.Parse("e2"));
            game.ApplyMove(NotationParser.Parse("Sd2"));
            game.ApplyMove(NotationParser.Parse("b2>"));
            game.ApplyMove(NotationParser.Parse("2e4<11"));
            game.ApplyMove(NotationParser.Parse("c2-"));
            game.ApplyMove(NotationParser.Parse("d4>"));
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("d3<"));
            game.ApplyMove(NotationParser.Parse("2c1+"));
            game.ApplyMove(NotationParser.Parse("2c3+"));
            game.ApplyMove(NotationParser.Parse("e3+"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("3c2+"));
            game.ApplyMove(NotationParser.Parse("d1"));
            game.ApplyMove(NotationParser.Parse("2c3-"));
            game.ApplyMove(NotationParser.Parse("Sb1"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("b1>"));
            game.ApplyMove(NotationParser.Parse("c2-"));
            game.ApplyMove(NotationParser.Parse("d2<"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("2c2>11"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("4c4-"));
            game.ApplyMove(NotationParser.Parse("2e4<"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("3d4+"));
            game.ApplyMove(NotationParser.Parse("Sa5"));
            game.ApplyMove(NotationParser.Parse("3c1>"));
            game.ApplyMove(NotationParser.Parse("a3"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("4d1>"));
            game.ApplyMove(NotationParser.Parse("d2+"));
            game.ApplyMove(NotationParser.Parse("e3<"));
            game.ApplyMove(NotationParser.Parse("2e2+"));
            game.ApplyMove(NotationParser.Parse("5e1+41"));
            game.ApplyMove(NotationParser.Parse("2c3>"));
            game.ApplyMove(NotationParser.Parse("Sd2"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("3b4>"));
            game.ApplyMove(NotationParser.Parse("a5>"));
            game.ApplyMove(NotationParser.Parse("c5>"));
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("4d3+"));
            game.ApplyMove(NotationParser.Parse("3e3+"));
            game.ApplyMove(NotationParser.Parse("e3"));

            BoardAnalyzer analyzer = new BoardAnalyzer(game.CurrentBoard.Size, BoardAnalysisWeights.bestWeights);
            
            BoardDisplayTest boardDisplayTest = new BoardDisplayTest(game.Boards.ToList().Select(b => new Tuple<IAnalysisResult,Board>(analyzer.Analyze(b), b)).ToList());
            boardDisplayTest.Show();
        }

        private void Test_OnClick(object sender, RoutedEventArgs e)
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };
            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("e1"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("a4"));
            game.ApplyMove(NotationParser.Parse("a5-"));
            game.ApplyMove(NotationParser.Parse("b4<"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("2a4>"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("Sb2"));
            game.ApplyMove(NotationParser.Parse("c5"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("Sc4"));
            game.ApplyMove(NotationParser.Parse("Cd3"));
            game.ApplyMove(NotationParser.Parse("c4<"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("b2>"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("e5-"));
            game.ApplyMove(NotationParser.Parse("Cb2"));
            game.ApplyMove(NotationParser.Parse("c2-"));
            game.ApplyMove(NotationParser.Parse("e2"));
            game.ApplyMove(NotationParser.Parse("Sd2"));
            game.ApplyMove(NotationParser.Parse("b2>"));
            game.ApplyMove(NotationParser.Parse("2e4<11"));
            game.ApplyMove(NotationParser.Parse("c2-"));
            game.ApplyMove(NotationParser.Parse("d4>"));
            game.ApplyMove(NotationParser.Parse("b5"));
            game.ApplyMove(NotationParser.Parse("d3<"));
            game.ApplyMove(NotationParser.Parse("2c1+"));
            game.ApplyMove(NotationParser.Parse("2c3+"));
            game.ApplyMove(NotationParser.Parse("e3+"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("3c2+"));
            game.ApplyMove(NotationParser.Parse("d1"));
            game.ApplyMove(NotationParser.Parse("2c3-"));
            game.ApplyMove(NotationParser.Parse("Sb1"));
            game.ApplyMove(NotationParser.Parse("e5"));
            game.ApplyMove(NotationParser.Parse("b1>"));
            game.ApplyMove(NotationParser.Parse("c2-"));
            game.ApplyMove(NotationParser.Parse("d2<"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("2c2>11"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("4c4-"));
            game.ApplyMove(NotationParser.Parse("2e4<"));
            game.ApplyMove(NotationParser.Parse("b3"));
            game.ApplyMove(NotationParser.Parse("3d4+"));
            game.ApplyMove(NotationParser.Parse("Sa5"));
            game.ApplyMove(NotationParser.Parse("3c1>"));
            game.ApplyMove(NotationParser.Parse("a3"));
            game.ApplyMove(NotationParser.Parse("c1"));
            game.ApplyMove(NotationParser.Parse("d3"));
            game.ApplyMove(NotationParser.Parse("4d1>"));
            game.ApplyMove(NotationParser.Parse("d2+"));
            game.ApplyMove(NotationParser.Parse("e3<"));
            game.ApplyMove(NotationParser.Parse("2e2+"));
            game.ApplyMove(NotationParser.Parse("5e1+41"));
            game.ApplyMove(NotationParser.Parse("2c3>"));
            game.ApplyMove(NotationParser.Parse("Sd2"));
            game.ApplyMove(NotationParser.Parse("c4"));
            game.ApplyMove(NotationParser.Parse("3b4>"));
            game.ApplyMove(NotationParser.Parse("a5>"));
            game.ApplyMove(NotationParser.Parse("c5>"));
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("4d3+"));
            game.ApplyMove(NotationParser.Parse("3e3+"));
            game.ApplyMove(NotationParser.Parse("e3"));

            BoardDisplayTest boardDisplayTest = new BoardDisplayTest(game.Boards.ToList());
            boardDisplayTest.Show();
        }

        private void LoadFiles_OnClick(object sender, RoutedEventArgs e)
        {
            FileTextBox.Clear();
            _boardsToAnalyze.Clear();
            CommonFileDialogResult result = _openFolderDialog.ShowDialog();
            if(result == CommonFileDialogResult.Ok)
            {
                string directory = _openFolderDialog.FileName;

                IEnumerable<string> files = Directory.EnumerateFiles(directory, "*.ptn", SearchOption.TopDirectoryOnly);

                List<Game> games = new List<Game>();
                foreach (string file in files)
                {
                    try
                    {
                        games.Add(Game.CreateGameFromTranscript(file));
                        FileTextBox.Text += $"Loaded file {file}\n";
                    }
                    catch(Exception ex)
                    {
                        FileTextBox.Text += $"Error loading file {file}: {ex.Message}\n";
                    }
                }

                foreach(Game game in games)
                {
                    BoardAnalyzer analyzer = new BoardAnalyzer(game.CurrentBoard.Size, BoardAnalysisWeights.bestWeights);
                    _boardsToAnalyze.AddRange(game.Boards.Select(b => new Tuple<BoardAnalysisData, Board>((BoardAnalysisData)analyzer.Analyze(b), b)));
                }
            }
        }

    }
}
