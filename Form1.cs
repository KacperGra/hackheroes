﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace app
{
    public partial class Hackheroes : Form
    {
        private readonly List<Panel> panels = new List<Panel>();

        private readonly Color blue1 = Color.FromArgb(0, 168, 255);
        private readonly Color purple1 = Color.FromArgb(156, 136, 255);
        private readonly Color darkblue1 = Color.FromArgb(72, 126, 176);
        private readonly Color red1 = Color.FromArgb(232, 65, 24);
        private readonly Color green1 = Color.FromArgb(76, 209, 55);
        private readonly Color yellow1 = Color.FromArgb(251, 197, 49);
        private readonly Color white1 = Color.FromArgb(220, 221, 225);

        public Hackheroes()
        {
            InitializeComponent();
            InitializeColors();
        }

        private void InitializeColors()
        {
            this.BackColor = white1;
            buttonBMI.BackColor = blue1;
            buttonActivity.BackColor = yellow1;
            buttonQuiz.BackColor = green1;
            buttonCalculator.BackColor = purple1;
            buttonSurvey.BackColor = red1;
            buttonProfile.BackColor = darkblue1;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Button4_Click(object sender, EventArgs e)
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

        private void ChangePanel(int index, bool visibility)
        {
            panels[index].BringToFront();
            buttonReturn.Visible = visibility;
        }

        private void ButtonBMI_Click(object sender, EventArgs e)
        {
            ChangePanel(1, true);
        }

        private void ButtonActivity_Click(object sender, EventArgs e)
        {
            ChangePanel(2, true);
        }

        private void ButtonQuiz_Click(object sender, EventArgs e)
        {
            ChangePanel(3, true);
        }

        private void ButtonCalculator_Click(object sender, EventArgs e)
        {
            ChangePanel(4, true);
        }

        private void ButtonSurvey_Click(object sender, EventArgs e)
        {
            ChangePanel(5, true);
        }

        private void ButtonProfile_Click(object sender, EventArgs e)
        {
            ChangePanel(6, true);

            listBoxUsers.SelectedIndex = Program.currentUserIndex;
            textBoxCurrentName.Text = Program.users[Program.currentUserIndex].name;
            numericUpDownCurrentAge.Value = Program.users[Program.currentUserIndex].age;
            numericUpDownCurrentHeight.Value = Program.users[Program.currentUserIndex].height;
            numericUpDownCurrentWeight.Value = Convert.ToDecimal(Program.users[Program.currentUserIndex].weight);
            if(Program.users[Program.currentUserIndex].gender == Gender.Male)
            {
                radioButtonCurrentMale.Checked = true;
            }
            else
            {
                radioButtonCurrentFemale.Checked = true;
            }

            textBoxCurrentName.Visible = false;
            numericUpDownCurrentAge.Visible = false;
            numericUpDownCurrentHeight.Visible = false;
            numericUpDownCurrentWeight.Visible = false;
            radioButtonCurrentFemale.Visible = false;
            radioButtonCurrentMale.Visible = false;
            label18.Visible = false;
            label19.Visible = false;
            label20.Visible = false;
            label22.Visible = false;
            label23.Visible = false;
            buttonDelete.Visible = false;
            buttonSaveChanges.Visible = false;
        }

        private void ButtonReturn_Click(object sender, EventArgs e)
        {
            ChangePanel(0, false);
        }

        private void UpdateArrowButtons()
        {
            buttonArrowUp.Enabled = (listBoxUsers.SelectedIndex > 0);
            buttonArrowDown.Enabled = (listBoxUsers.SelectedIndex < listBoxUsers.Items.Count - 1);
        }

        private void UpdateAgeForm(Label lbl, NumericUpDown numericUD)
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

        private void NumericUpDownAge_ValueChanged(object sender, EventArgs e)
        {
            UpdateAgeForm(label15, numericUpDownAge);
        }

        private void NumericUpDownCurrentAge_ValueChanged(object sender, EventArgs e)
        {
            UpdateAgeForm(label17, numericUpDownCurrentAge);
        }

        private void ButtonCreate_Click(object sender, EventArgs e)
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

            UpdateArrowButtons();
        }

        private void ListBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateArrowButtons();

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

        private void ButtonDelete_Click_1(object sender, EventArgs e)
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
            }
        }

        private void ButtonSaveChanges_Click(object sender, EventArgs e)
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

        private void ButtonArrowUp_Click(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedIndex > 0)
            {
                --listBoxUsers.SelectedIndex;
            }
        }

        private void ButtonArrowDown_Click(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedIndex < listBoxUsers.Items.Count - 1)
            {
                ++listBoxUsers.SelectedIndex;
            }
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            textBoxCurrentName.Visible = true;
            numericUpDownCurrentAge.Visible = true;
            numericUpDownCurrentHeight.Visible = true;
            numericUpDownCurrentWeight.Visible = true;
            radioButtonCurrentFemale.Visible = true;
            radioButtonCurrentMale.Visible = true;
            label18.Visible = true;
            label19.Visible = true;
            label20.Visible = true;
            label22.Visible = true;
            label23.Visible = true;
            buttonDelete.Visible = true;
            buttonSaveChanges.Visible = true;
        }
    }
}
