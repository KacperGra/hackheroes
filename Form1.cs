﻿using System;
using System.Collections.Generic;
using System.IO;
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
            using(StreamReader loading = new StreamReader("..\\..\\users.dat"))
            {
                string name;
                byte age;
                float weight;
                uint height;
                Gender gender;

                string line;
                string[] arr = new string[4];

                while(!loading.EndOfStream)
                {
                    name = loading.ReadLine();

                    line = loading.ReadLine();
                    arr = line.Split(' ');

                    age = Convert.ToByte(arr[0]);
                    weight = Convert.ToSingle(arr[1]);
                    height = Convert.ToUInt32(arr[2]);
                    gender = arr[3] == "1" ? Gender.Female : Gender.Male;

                    Program.users.Add(new User(name, age, weight, height, gender));
                    listBoxUsers.Items.Add(name);
                }
            }

            if(Program.users.Count == 0)
            {
                Program.users.Add(new User("User", 18, 80f, 180, Gender.Male));
            }

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

            listBoxUsers.SelectedIndex = Program.currentUserIndex;
            textBoxCurrentName.Text = Program.users[Program.currentUserIndex].name;
            numericUpDownCurrentAge.Value = Program.users[Program.currentUserIndex].age;
            numericUpDownCurrentHeight.Value = Program.users[Program.currentUserIndex].height;
            numericUpDownCurrentWeight.Value = Convert.ToDecimal(Program.users[Program.currentUserIndex].weight);
            if (Program.users[Program.currentUserIndex].gender == Gender.Male)
            {
                radioButtonCurrentMale.Checked = true;
            }
            else
            {
                radioButtonCurrentFemale.Checked = true;
            }
            updateArrowButtons();
            setEditInfoVisibility(false);
        }

        private void buttonReturn_Click(object sender, EventArgs e)
        {
            changePanel(0, false);
        }

        private void setEditInfoVisibility(bool visibility)
        {
            textBoxCurrentName.Visible = visibility;
            numericUpDownCurrentAge.Visible = visibility;
            numericUpDownCurrentHeight.Visible = visibility;
            numericUpDownCurrentWeight.Visible = visibility;
            radioButtonCurrentFemale.Visible = visibility;
            radioButtonCurrentMale.Visible = visibility;
            label18.Visible = visibility;
            label19.Visible = visibility;
            label20.Visible = visibility;
            label22.Visible = visibility;
            label23.Visible = visibility;
            buttonDelete.Visible = visibility;
            buttonSaveChanges.Visible = visibility;
        }

        private void updateArrowButtons()
        {
            buttonArrowUp.Enabled = (listBoxUsers.SelectedIndex > 0);
            buttonArrowDown.Enabled = (listBoxUsers.SelectedIndex < listBoxUsers.Items.Count - 1);
        }

        private void updateAgeForm(Label lbl, NumericUpDown numericUD)
        {
            uint val = Convert.ToUInt16(numericUD.Value);
            if (val == 1)
            {
                lbl.Text = "rok";
            }
            else if (val >= 2 && val <= 4)
            {
                lbl.Text = "lata";
            }
            else if (val >= 5 && val <= 21)
            {
                lbl.Text = "lat";
            }
            else
            {
                uint lastDigit = val % 10;
                switch (lastDigit)
                {
                    case 2:
                    case 3:
                    case 4:
                        lbl.Text = "lata";
                        break;
                    default:
                        lbl.Text = "lat";
                        break;
                }
            }
        }

        private void numericUpDownAge_ValueChanged(object sender, EventArgs e)
        {
            updateAgeForm(label15, numericUpDownAge);
        }

        private void numericUpDownCurrentAge_ValueChanged(object sender, EventArgs e)
        {
            updateAgeForm(label17, numericUpDownCurrentAge);
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text == "" || (radioButtonFemale.Checked == false && radioButtonMale.Checked == false))
            {
                string missingInfo = "";
                
                if (textBoxName.Text == "" && radioButtonFemale.Checked == false && radioButtonMale.Checked == false)
                {
                    missingInfo = "imię, płeć";
                }
                else if (radioButtonFemale.Checked == false && radioButtonMale.Checked == false)
                {
                    missingInfo = "płeć";
                }
                else if(textBoxName.Text == "")
                {
                    missingInfo = "imię";
                }

                string message = "Aby utworzyć profil musisz podać wszystkie dane!\nBrakujące dane: " + missingInfo + ".";
                string caption = "Niepoprawnie wypełniony formularz";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;
                
                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Close();
                }

                Program.currentUserIndex = listBoxUsers.Items.Count - 1;
                listBoxUsers.SelectedIndex = listBoxUsers.Items.Count - 1;
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
                User newUser = new User(textBoxName.Text, Convert.ToByte(numericUpDownAge.Value), Convert.ToSingle(numericUpDownWeight.Value), Convert.ToUInt16(numericUpDownHeight.Value), gend);
                Program.users.Add(newUser);
                listBoxUsers.Items.Add(newUser.name);

                Program.currentUserIndex = Program.users.Count - 1; 
                listBoxUsers.SelectedIndex = Program.currentUserIndex;
            }

            updateArrowButtons();
        }

        private void listBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateArrowButtons();

            if(listBoxUsers.SelectedIndex != -1)
            {
                Program.currentUserIndex = listBoxUsers.SelectedIndex;

                textBoxCurrentName.Text = Program.users[Program.currentUserIndex].name;
                numericUpDownCurrentAge.Value = Program.users[Program.currentUserIndex].age;
                numericUpDownCurrentWeight.Value = Convert.ToDecimal(Program.users[Program.currentUserIndex].weight);
                numericUpDownCurrentHeight.Value = Convert.ToUInt16(Program.users[Program.currentUserIndex].height);
                if (Program.users[Program.currentUserIndex].gender == Gender.Male)
                {
                    radioButtonCurrentMale.Checked = true;
                }
                else
                {
                    radioButtonCurrentFemale.Checked = true;
                }
            }
        }

        private void buttonDelete_Click_1(object sender, EventArgs e)
        {
            int indexToRemove = listBoxUsers.SelectedIndex;
            if (listBoxUsers.Items.Count <= 1)
            {
                string message = "Nie można usunąć jedynego istniejącego profilu!\nUtwórz nowy profil lub edytuj już istniejący.";
                string caption = "Nie można usunąć profilu";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Close();
                }
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

                listBoxUsers.SelectedIndex = 0;
                Program.currentUserIndex = 0;

                updateArrowButtons();
                setEditInfoVisibility(false);
            }
        }

        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedIndex != -1)
            {
                Program.users[listBoxUsers.SelectedIndex].name = textBoxCurrentName.Text;
                Program.users[listBoxUsers.SelectedIndex].age = Convert.ToByte(numericUpDownCurrentAge.Value);
                Program.users[listBoxUsers.SelectedIndex].weight = Convert.ToSingle(numericUpDownCurrentWeight.Value);
                Program.users[listBoxUsers.SelectedIndex].height = Convert.ToUInt16(numericUpDownCurrentHeight.Value);

                if (radioButtonCurrentMale.Checked)
                {
                    Program.users[listBoxUsers.SelectedIndex].gender = Gender.Male;
                }
                else
                {
                    Program.users[listBoxUsers.SelectedIndex].gender = Gender.Female;
                }
                listBoxUsers.Items[listBoxUsers.SelectedIndex] = Program.users[listBoxUsers.SelectedIndex].name;
                setEditInfoVisibility(false);
            }
            else
            {
                string message = "Aby edytować dane, musisz najpierw zaznaczyć profil!";
                string caption = "Nie zaznaczono profilu";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Close();
                }
            }
        }

        private void buttonArrowUp_Click(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedIndex > 0)
            {
                --listBoxUsers.SelectedIndex;
            }
            setEditInfoVisibility(false);
        }

        private void buttonArrowDown_Click(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedIndex < listBoxUsers.Items.Count - 1)
            {
                ++listBoxUsers.SelectedIndex;
            }
            setEditInfoVisibility(false);
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            setEditInfoVisibility(true);
        }

        private void Hackheroes_FormClosing(object sender, FormClosingEventArgs e)
        {
            using(StreamWriter saving = new StreamWriter("..\\..\\users.dat"))
            {
                foreach (User user in Program.users)
                {
                    saving.WriteLine(user.name);
                    saving.WriteLine(user.getData());
                }
            }          
        }
    }
}
