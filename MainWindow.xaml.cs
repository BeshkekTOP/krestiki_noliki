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

using System;
using System.Collections.Generic;
using System.Windows;

using System;
using System.Collections.Generic;
using System.Windows;

namespace krestiki_noliki
{
    public partial class MainWindow : Window
    {
        private enum Player { None, Cross, Zero }
        private Player currentPlayer;
        private Player[,] board;
        private List<Button> buttons;

        public MainWindow()
        {
            InitializeComponent();
            currentPlayer = Player.Cross;
            board = new Player[3, 3];
            buttons = new List<Button> { btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9 };
            foreach (var button in buttons)
            {
                button.IsEnabled = false;
            }
        }

        private void BtnNewGame_Click(object sender, RoutedEventArgs e)
        {
            currentPlayer = Player.Cross;
            board = new Player[3, 3];
            foreach (var button in buttons)
            {
                button.Content = "";
                button.IsEnabled = true;
            }
            txtResult.Text = "";
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var row = Grid.GetRow(button);
            var col = Grid.GetColumn(button);
            button.Content = currentPlayer == Player.Cross ? "X" : "O";
            board[row, col] = currentPlayer;
            if (CheckWin(currentPlayer))
            {
                txtResult.Text = $"Игрок {currentPlayer} выйграл!";
                DisableButtons();
            }
            else if (IsBoardFull())
            {
                txtResult.Text = "Ничья!";
                DisableButtons();
            }
            else
            {
                SwitchPlayer();
                var robotMove = GetRobotMove();
                var robotButton = buttons[robotMove];
                robotButton.Content = currentPlayer == Player.Cross ? "X" : "O";
                board[robotMove / 3, robotMove % 3] = currentPlayer;
                if (CheckWin(currentPlayer))
                {
                    txtResult.Text = $"Игрок {currentPlayer} выйграл!";
                    DisableButtons();
                }
                else if (IsBoardFull())
                {
                    txtResult.Text = "Ничья!";
                    DisableButtons();
                }
                else
                {
                    SwitchPlayer();
                }
            }
        }

        private bool CheckWin(Player player)
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player)
                {
                    return true;
                }
                if (board[0, i] == player && board[1, i] == player && board[2, i] == player)
                {
                    return true;
                }
            }
            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
            {
                return true;
            }
            if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
            {
                return true;
            }
            return false;
        }

        private bool IsBoardFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == Player.None)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void SwitchPlayer()
        {
            currentPlayer = currentPlayer == Player.Cross ? Player.Zero : Player.Cross;
        }

        private int GetRobotMove()
        {
            var random = new Random();
            List<int> availableMoves = new List<int>();

            for (int i = 0; i < 9; i++)
            {
                if (board[i / 3, i % 3] == Player.None)
                {
                    availableMoves.Add(i);
                }
            }

            if (availableMoves.Count > 0)
            {
                return availableMoves[random.Next(availableMoves.Count)];
            }

            return -1;
        }

        private void DisableButtons()
        {
            foreach (var button in buttons)
            {
                button.IsEnabled = false;
            }
        }

    }
}