﻿using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text.Json;

namespace app
{
    class Question
    {
        public string ask { get; set; }
        public string correctAnswer { get; set; }
        public string[] incorrectAnswers { get; set; }

        public Question(string _ask, string _correctAnswer, string[] _incorrectAnswers)
        {
            ask = _ask;
            correctAnswer = _correctAnswer;
            incorrectAnswers = new string[3];
            incorrectAnswers[0] = _incorrectAnswers[0];
            incorrectAnswers[1] = _incorrectAnswers[1];
            incorrectAnswers[2] = _incorrectAnswers[2];
        }

       public Question()
       {
       }
    }

    static class Quiz
    {
        public static List<Question> questions;
        public static List<Question> drawnQuestions = new List<Question>();
        public static int score;
        public static int questionNumber;
        public static bool isAnswerChosen;

        public static void DrawQuestion()
        {
            int index;
            do
            {
                index = Program.rnd.Next(questions.Count);
            } while(drawnQuestions.Contains(questions[index]));

            drawnQuestions.Add(questions[index]);
        }

        public static void GetQuestions()
        {
            drawnQuestions.Clear();

            for (int i = 0; i < 5; i++)
            {
                DrawQuestion();
            }
        }
    }
}
