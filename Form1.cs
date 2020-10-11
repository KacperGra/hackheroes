﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace app
{
    public partial class Hackheroes : Form
    {
        private readonly List<Panel> panels = new List<Panel>();

        public Hackheroes()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void Hackheroes_Load(object sender, EventArgs e)
        {
            panels.Add(panel0); //buttons
            panels.Add(panel1); //BMI
            panels.Add(panel2); //sport activity
            panels.Add(panel3); //quiz
            panels.Add(panel4); //calculator
            panels.Add(panel5); //surveys
            panels.Add(panel6); //profiles
        }

        private void changePanel(int index, bool visibility)
        {
            panels[index].BringToFront();
            buttonReturn.Visible = visibility;
        }

        private void buttonBMI_Click(object sender, EventArgs e)
        {
            changePanel(1, true);
        }

        private void buttonActivity_Click(object sender, EventArgs e)
        {
            changePanel(2, true);
        }

        private void buttonQuiz_Click(object sender, EventArgs e)
        {
            changePanel(3, true);
        }

        private void buttonCalculator_Click(object sender, EventArgs e)
        {
            changePanel(4, true);
        }

        private void buttonSurvey_Click(object sender, EventArgs e)
        {
            changePanel(5, true);
        }

        private void buttonProfile_Click(object sender, EventArgs e)
        {
            changePanel(6, true);
        }

        private void buttonReturn_Click(object sender, EventArgs e)
        {
            changePanel(0, false);
        }

        private void numericUpDownAge_ValueChanged(object sender, EventArgs e)
        {
            uint val = Convert.ToUInt16(numericUpDownAge.Value);
            if(val == 1)
            {
                label15.Text = "rok";
            }
            else if (val >= 2 && val <= 4)
            {
                label15.Text = "lata";
            }
            else if (val >= 5 && val <= 21)
            {
                label15.Text = "lat";
            }
            else
            {
                uint lastDigit = val % 10;
                switch (lastDigit)
                {
                    case 2:
                    case 3:
                    case 4:
                        label15.Text = "lata";
                        break;
                    default:
                        label15.Text = "lat";
                        break;
                }
            }
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text == "" || (radioButtonFemale.Checked == false && radioButtonMale.Checked == false))
            {
                Console.WriteLine("Nie mozna utowrzyc profilu bez podania wszystkich danych!");
            }
            else
            {
                Gender gend;
                if (radioButtonFemale.Checked)
                {
                    gend = Gender.Female;
                }
                else
                {
                    gend = Gender.Male;
                }
                User newUser = new User(textBoxName.Text, Convert.ToByte(numericUpDownAge.Value), Convert.ToSingle(numericUpDownWeight.Value), Convert.ToByte(numericUpDownHeight.Value), gend);
                Program.users.Add(newUser);
                listBoxUsers.Items.Add(newUser);
            }
            // DEBUG ONLY //////////////////
            foreach (User aUser in Program.users)
            {
                Console.WriteLine(aUser.name);
            }
            ////////////////////////////////
        }

        private void listBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBoxUsers.SelectedIndex != -1)
            {
                Program.currentUserIndex = listBoxUsers.SelectedIndex;

                Console.WriteLine("listBoxUsers.SelectedIndex: " + listBoxUsers.SelectedIndex);
                Console.WriteLine("Program.currentUserIndex: " + Program.currentUserIndex);

                Console.WriteLine("Current user: " + Program.users[Program.currentUserIndex].name);

                textBoxCurrentName.Text = Program.users[Program.currentUserIndex].name;
                numericUpDownCurrentAge.Value = Program.users[Program.currentUserIndex].age;
                numericUpDownCurrentWeight.Value = Convert.ToDecimal(Program.users[Program.currentUserIndex].weight);
                numericUpDownCurrentHeight.Value = Convert.ToDecimal(Program.users[Program.currentUserIndex].height);
                if (Program.users[Program.currentUserIndex].gender == Gender.Male)
                {
                    radioButtonCurrentMale.Checked = true;
                }
                else
                {
                    radioButtonCurrentFemale.Checked = true;
                }
            }
            else
            {
                Console.WriteLine("Brak wartosci dla SelectedIndex! (indexChanged)");
            }
            

            // DEBUG ONLY //////////////////
            foreach (User aUser in Program.users)
            {
                Console.WriteLine(aUser.name);
            }
            ////////////////////////////////
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            Program.users.RemoveAt(listBoxUsers.SelectedIndex);
            listBoxUsers.Items.RemoveAt(listBoxUsers.SelectedIndex);
            // DEBUG ONLY //////////////////
            foreach (User aUser in Program.users)
            {
                Console.WriteLine(aUser.name);
            }
            ////////////////////////////////
        }

        private void buttonDelete_Click_1(object sender, EventArgs e)
        {
            int indexToRemove = listBoxUsers.SelectedIndex;
            if (listBoxUsers.Items.Count <= 1)
            {
                Console.WriteLine("Nie mozna usunac jedynego profilu! Utworz nowy i nastepnie usun poprzedni.");
            }
            else
            {
                if (indexToRemove > 0)
                {
                    --listBoxUsers.SelectedIndex;
                }
                else
                {
                    ++listBoxUsers.SelectedIndex;
                }
                Program.users.RemoveAt(indexToRemove);
                listBoxUsers.Items.RemoveAt(indexToRemove);
            }
            // DEBUG ONLY //////////////////
            foreach (User aUser in Program.users)
            {
                Console.WriteLine(aUser.name);
            }
            ////////////////////////////////
        }

        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedIndex != -1)
            {
                Program.users[listBoxUsers.SelectedIndex].name = textBoxCurrentName.Text;
                Program.users[listBoxUsers.SelectedIndex].age = Convert.ToByte(numericUpDownCurrentAge.Value);
                Program.users[listBoxUsers.SelectedIndex].weight = Convert.ToSingle(numericUpDownCurrentWeight.Value);
                Program.users[listBoxUsers.SelectedIndex].height = Convert.ToSingle(numericUpDownCurrentHeight.Value);

                if (radioButtonCurrentMale.Checked)
                {
                    Program.users[listBoxUsers.SelectedIndex].gender = Gender.Male;
                }
                else
                {
                    Program.users[listBoxUsers.SelectedIndex].gender = Gender.Female;
                }
                listBoxUsers.Items[listBoxUsers.SelectedIndex] = Program.users[listBoxUsers.SelectedIndex];
            }
            else
            {
                Console.WriteLine("Brak wartosci dla SelectedIndex!");
            }
        }
    }
}
