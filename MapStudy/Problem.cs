namespace MapStudy
{
    public class Problem
    {
        public ProblemType Type { get; set; }
        public object Answer { get; set; }

        public bool IsCorrect(object input)
        {
            if (input.GetType().Equals(Answer.GetType()))
            {
                if (input is string)
                {
                    string str = (string)input;
                    string strAnswer = (string)Answer;

                    str = str.ToLower().Trim();
                    strAnswer = strAnswer.ToLower().Trim();

                    if (str.StartsWith("the"))
                    {
                        str = str.Remove(0, 3);
                    }

                    str = str.Trim();
                    return string.Compare(str, strAnswer, true) == 0;
                }
                else if (input is int)
                {
                    int i = (int)input;
                    int iAnswer = (int)Answer;

                    return i == iAnswer;
                }
            }

            throw new System.Exception("Incorrect problem input.");
        }

        public Problem(ProblemType type, object answer)
        {
            Type = type;
            Answer = answer;
        }
    }
}
