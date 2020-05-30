using System;
using System.Collections.Generic;
using System.Linq;
using Web.Models;

namespace Web
{
    public class Transformations
    {
        public List<ViewModel> ToViewModel(List<ResponseModel> questionSets)
        {
            var random = new Random();
            var result = new List<ViewModel>();
            foreach (var questionSet in questionSets)
            {
                var viewModel = new ViewModel();
                viewModel.Question = questionSet.Question;
                viewModel.Answers = new List<string>() { questionSet.Correct_Answer };
                viewModel.Answers.AddRange(questionSet.Incorrect_Answers);
                viewModel.Answers = viewModel.Answers.OrderBy(x => random.Next()).ToList();
                viewModel.CorrectAnswer = viewModel.Answers.FindIndex(x => x == questionSet.Correct_Answer);
                viewModel.QuestionType = questionSet.Type;
                result.Add(viewModel);
            }

            return result;
        }
    }
}
