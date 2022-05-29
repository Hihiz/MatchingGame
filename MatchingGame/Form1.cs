using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        // Используйте этот случайный объект, чтобы выбрать случайные значки для квадратов
        Random random = new Random();
        // Каждая из этих букв представляет собой интересный значок
        // в шрифте Webdings,
        // и каждый значок появляется дважды в этом списке
        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z"
        };

        // первый щелчок указывает на первый элемент управления меткой
        // который игрок нажимает, но он будет равен нулю
        // если игрок еще не нажал на ярлык
        Label firstClicked = null;
        // второй щелчок указывает на второй элемент управления меткой
        // что игрок нажимает
        Label secondClicked = null;

        public Form1()
        {
            InitializeComponent();

            AssignIconsToSquares();
        }

        private void AssignIconsToSquares()
        {
            // Панель TableLayoutPyoutPanel имеет 16 меток,
            // а список значков содержит 16 значков,
            // поэтому значок выбирается случайным образом из списка
            // и добавляется к каждой метке
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Таймер включается / включается только после того, как игроку были показаны два несоответствующих
            // значка,
            // поэтому игнорируйте любые щелчки, если таймер запущен
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // Если нажатая метка laed черного цвета, игрок нажал
                // значок, который уже был показан --
                // игнорировать щелчок
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                // Если значение firstClicked равно null, то это первый значок
                // в паре, на которую нажал игрок,
                // поэтому установите firstClicked на метку, на которую нажал игрок
                //, измените ее цвет на черный и верните
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }
                // Если игрок заходит так далеко, таймер не
                // запущен, а значение firstClicked не равно null,
                // так что это должен быть второй значок, на который нажал игрок.
                // Установите его цвет на черный
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                CheckForWinner();//

                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                // Если игрок зашел так далеко, игрок
                // нажал на две разные иконки, поэтому запустите
                // таймер (который будет ждать три четверти
                // секунды, а затем скроет значки)
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Остановить таймер
            timer1.Stop();
            // Скрыть оба значка
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;
            // // Сброс первого и второго кликов
            // итак, в следующий раз, когда ярлык будет
            // щелкнул, программа знает, что это первый щелчок
            firstClicked = null;
            secondClicked = null;
        }

        private void CheckForWinner()
        {
            // Просмотрите все метки в TableLayoutPanel,
            // проверяя каждую из них, чтобы увидеть, соответствует ли ее значок
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            // Если цикл не вернулся, он не нашел
            // любые непревзойденные значки
            // Это означает, что пользователь выиграл. Показать сообщение и закрыть форму
            MessageBox.Show("Вы подобрали все значки!", "Поздравляю");
            Close();
        }
    }
}
