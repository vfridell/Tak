using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Analyze_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
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

        private void TextBox_OnPreviewDragOver(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TextBox_OnDrop(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
