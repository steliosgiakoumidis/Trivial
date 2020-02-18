using System;
using System.Collections.Generic;
using System.Linq;
using Trivial.DataModels;

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
                viewModel.Question = questionSet.question;
                viewModel.Answers = new List<string>() { questionSet.correct_answer };
                viewModel.Answers.AddRange(questionSet.incorrect_answers);
                viewModel.Answers = viewModel.Answers.OrderBy(x => random.Next()).ToList();
                viewModel.CorrectAnswer = viewModel.Answers.FindIndex(x => x == questionSet.correct_answer);
                viewModel.QuestionType = questionSet.type;
                result.Add(viewModel);
            }

            return result;
        }
    }
}
