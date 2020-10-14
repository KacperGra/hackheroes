﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

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
        public static class ModifyProgressBarColor
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
            static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
            public static void SetState(ProgressBar bar, int state)
            {
                SendMessage(bar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Button4_Click(object sender, EventArgs e)
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
                    gender = arr[3] == "Female" ? Gender.Female : Gender.Male;

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

            center(label6, 30); //BMI
        }

        private void ChangePanel(int index, bool visibility)
        {
            panels[index].BringToFront();
            buttonReturn.Visible = visibility;
        }

        private void UpdateButtonDeleteEnabledStatus()
        {
            if (listBoxUsers.Items.Count > 1)
            {
                buttonDelete.Enabled = true;
            }
            else
            {
                buttonDelete.Enabled = false;
            }
        }

        private void updateArrow()
        {
            float BMI = Program.users[Program.currentUserIndex].BMI;
            float value = BMI * 2.5f - 10f;

            if (value < 0f)
            {
                value = 0f;
            }
            else if(value > 100f)
            {
                value = 100f;
            }

            pictureBoxArrow.Location = new Point(75 + (int)value * 8, 147);
        }

        private string getInterpretation(float BMI)
        {
            if(BMI < 18.5f)
            {
                return "Niedowaga";
            }
            else if(BMI < 25f)
            {
                return "Norma";
            }
            else if(BMI < 30f)
            {
                return "Nadwaga";
            }
            else
            {
                return "Otyłość";
            }
        }

        private void center(Control control, int h)
        {
            control.Location = new Point(1000 / 2 - control.Size.Width / 2, h);
        }

        private void buttonBMI_Click(object sender, EventArgs e)
        {
            int userIndex = Program.currentUserIndex;
            Calculator.CalculateBMI(Program.users[userIndex]);

            updateArrow();

            labelBMI.Text = "Twoje BMI wynosi: " + Program.users[userIndex].BMI.ToString("0.##");
            labelBMIInterpretation.Text = getInterpretation(Program.users[userIndex].BMI);

            center(labelBMI, 300);
            center(labelBMIInterpretation, 360);

            changePanel(1, true);
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
            Calculator.CalculateMacro(Program.users[Program.currentUserIndex]);
        }

        private void ButtonSurvey_Click(object sender, EventArgs e)
        {
            ChangePanel(5, true);
        }

        private void ButtonProfile_Click(object sender, EventArgs e)
        {
            ChangePanel(6, true);

            UpdateButtonDeleteEnabledStatus();
            buttonSaveChanges.Enabled = false;

            int userIndex = listBoxUsers.SelectedIndex = Program.currentUserIndex;

            textBoxCurrentName.Text = Program.users[userIndex].name;
            numericUpDownCurrentAge.Value = Program.users[userIndex].age;
            numericUpDownCurrentHeight.Value = Program.users[userIndex].height;
            numericUpDownCurrentWeight.Value = Convert.ToDecimal(Program.users[userIndex].weight);
            
            if (Program.users[userIndex].gender == Gender.Male)
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

        private void ButtonReturn_Click(object sender, EventArgs e)
        {
            ChangePanel(0, false);
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
            if(numericUpDownCurrentAge.Value != Program.users[Program.currentUserIndex].age)
            {
                buttonSaveChanges.Enabled = true;
            }
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

                Program.currentUserIndex = listBoxUsers.SelectedIndex = listBoxUsers.Items.Count - 1;
            }
            else
            {
                Gender gender = radioButtonMale.Checked == true ? Gender.Male : Gender.Female;

                User newUser = new User(textBoxName.Text, Convert.ToByte(numericUpDownAge.Value), Convert.ToSingle(numericUpDownWeight.Value), Convert.ToUInt16(numericUpDownHeight.Value), gender);

                Program.users.Add(newUser);
                listBoxUsers.Items.Add(newUser.name);

                Program.currentUserIndex = listBoxUsers.SelectedIndex = Program.users.Count - 1; 
            }

            UpdateArrowButtons();
            UpdateButtonDeleteEnabledStatus();
            setEditInfoVisibility(false);
        }

        private void ListBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateArrowButtons();
            setEditInfoVisibility(false);

            if (listBoxUsers.SelectedIndex != -1)
            {
                int userIndex = Program.currentUserIndex = listBoxUsers.SelectedIndex;

                textBoxCurrentName.Text = Program.users[userIndex].name;
                numericUpDownCurrentAge.Value = Program.users[userIndex].age;
                numericUpDownCurrentWeight.Value = Convert.ToDecimal(Program.users[userIndex].weight);
                numericUpDownCurrentHeight.Value = Convert.ToUInt16(Program.users[userIndex].height);
                radioButtonCurrentMale.Checked = Program.users[userIndex].gender == Gender.Male;
                radioButtonCurrentFemale.Checked = Program.users[userIndex].gender == Gender.Female;
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

                listBoxUsers.SelectedIndex = Program.currentUserIndex = 0;

                UpdateButtonDeleteEnabledStatus();
                updateArrowButtons();
                setEditInfoVisibility(false);
                buttonSaveChanges.Enabled = false;
            }
        }

        private void ButtonSaveChanges_Click(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedIndex != -1)
            {
                int userIndex = listBoxUsers.SelectedIndex;

                Program.users[userIndex].name = textBoxCurrentName.Text;
                Program.users[userIndex].age = Convert.ToByte(numericUpDownCurrentAge.Value);
                Program.users[userIndex].weight = Convert.ToSingle(numericUpDownCurrentWeight.Value);
                Program.users[userIndex].height = Convert.ToUInt16(numericUpDownCurrentHeight.Value);

                if (radioButtonCurrentMale.Checked)
                {
                    Program.users[userIndex].gender = Gender.Male;
                }
                else
                {
                    Program.users[userIndex].gender = Gender.Female;
                }
                listBoxUsers.Items[userIndex] = Program.users[userIndex].name;
                setEditInfoVisibility(false);
                buttonSaveChanges.Enabled = false;
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

        private void textBoxCurrentName_TextChanged(object sender, EventArgs e)
        {
            if (textBoxCurrentName.Text != Program.users[Program.currentUserIndex].name)
            {
                buttonSaveChanges.Enabled = true;
            }
        }

        private void numericUpDownCurrentWeight_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownCurrentWeight.Value != Convert.ToDecimal(Program.users[Program.currentUserIndex].weight))
            {
                buttonSaveChanges.Enabled = true;
            }
        }

        private void numericUpDownCurrentHeight_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownCurrentHeight.Value != Program.users[Program.currentUserIndex].height)
            {
                buttonSaveChanges.Enabled = true;
            }
        }

        private void radioButtonCurrentMale_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCurrentMale.Checked && Program.users[Program.currentUserIndex].gender != Gender.Male)
            {
                buttonSaveChanges.Enabled = true;
            }
        }

        private void radioButtonCurrentFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCurrentFemale.Checked && Program.users[Program.currentUserIndex].gender != Gender.Female)
            {
                buttonSaveChanges.Enabled = true;
            }
        }
    }
}