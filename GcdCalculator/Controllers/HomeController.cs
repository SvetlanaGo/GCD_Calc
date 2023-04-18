using GcdCalculator.Models;
using GcdCalculator.Services.StaticClasses;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GcdCalculator.Controllers
{
    /// <summary>
    /// Class HomeController
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class
        /// </summary>
        public HomeController()
        {
        }

        delegate T CalculateByTwoParams<T, U>(out U output, T first, T second);
        delegate T CalculateByThreeParams<T, U>(out U output, T first, T second, T third);
        delegate T CalculateByManyParams<T, U, in V>(out U output, T first, T second, V manyParams);

        /// <summary>
        /// Calculates a greatest commn divisor
        /// </summary>
        /// <param name="vdata">The ResultViewModel</param>
        /// <returns>Returns a representation of the result model</returns>
        public IActionResult Index(ResultViewModel vdata, string submit)
        {
            if (submit != "Calculate")
                return View(vdata);

            try
            {
                if (!this.CheckRequiredParams(vdata))
                    return View(vdata);

                this.GcdCalculate(vdata);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                vdata.Error = ex.Message;
            }
            catch (ArgumentException ex)
            {
                vdata.Error = ex.Message;
            }

            return View(vdata);
        }

        /// <summary>
        /// Represents handling an unexpected error
        /// </summary>
        /// <returns>Returns a representation of the error model</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool CheckRequiredParams(ResultViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.CountNumbers))
            {
                model.Error = "Error: The count of numbers is not selected.";
                return false;
            }

            if (!this.CheckIntegerParam(model.First, "First", model) || !this.CheckIntegerParam(model.Second, "Second", model))
                return false;

            if (string.IsNullOrWhiteSpace(model.Algorithm))
            {
                model.Error = "Error: No algorithm selected.";
                return false;
            }

            return true;
        }

        private bool CheckIntegerParam(int? integerParam, string fieldName, ResultViewModel model)
        {
            if (!integerParam.HasValue)
            {
                model.Error = @$"Error: The value entered in the ""{fieldName}"" field is not an integer in the allowed range.";

                return false;
            }

            return true;
        }

        private bool CheckManyParams(ResultViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.NumbersOther))
            {
                model.Error = "Error: Integer's field is not filled in.";

                return false;
            }

            return true;
        }

        private void GcdCalculate(ResultViewModel model) =>
            (
                (model.Extended, model.CountNumbers) switch
                {
                    ("on", "2") => () => this.ExtendedGcdCalculateByTwooParams(model),
                    ("on", "3") => () => this.ExtendedGcdCalculateByThreeParams(model),
                    ("on", "4") => () => this.ExtendedGcdCalculateByManyParams(model),
                    (_, "2")    => () => this.GcdCalculateByTwooParams(model),
                    (_, "3")    => () => this.GcdCalculateByThreeParams(model),
                    (_, "4")    => () => this.GcdCalculateByManyParams(model),
                    _ => new Action(() => { })
                }
            )();

        private void GcdCalculateByTwooParams(ResultViewModel model) => 
            model.Result = this.GetAlgorithmByTwoParams(model)(model.First.Value, model.Second.Value);

        private void GcdCalculateByThreeParams(ResultViewModel model)
        {
            if (!this.CheckIntegerParam(model.Third, "Third", model))
                return;

            model.Result = this.GetAlgorithmByThreeParams(model)(model.First.Value, model.Second.Value, model.Third.Value);
        }

        private void GcdCalculateByManyParams(ResultViewModel model)
        {
            if (!this.CheckManyParams(model))
                return;

            var numberList = this.GetNumberList(model);
            if (!numberList.Any())
            {
                return;
            }

            model.Result = this.GetAlgorithmByManyParams(model)(model.First.Value, model.Second.Value, numberList.ToArray());
        }

        private void ExtendedGcdCalculateByTwooParams(ResultViewModel model) =>
            (model.Result, model.CalculationTime) = (this.GetExtendedAlgorithmByTwoParams(model)(out long ms, model.First.Value, model.Second.Value), ms);

        private void ExtendedGcdCalculateByThreeParams(ResultViewModel model)
        {
            if (!this.CheckIntegerParam(model.Third, "Third", model))
                return;

            (model.Result, model.CalculationTime) = 
                (this.GetExtendedAlgorithmByThreeParams(model)(out long ms, model.First.Value, model.Second.Value, model.Third.Value), ms);
        }

        private void ExtendedGcdCalculateByManyParams(ResultViewModel model)
        {
            if (!this.CheckManyParams(model))
                return;

            var numberList = this.GetNumberList(model);
            if (!numberList.Any())
            {
                return;
            }

            (model.Result, model.CalculationTime) =
                (this.GetExtendedAlgorithmByManyParams(model)(out long ms, model.First.Value, model.Second.Value, numberList.ToArray()), ms);
        }

        private Func<int, int, int> GetAlgorithmByTwoParams(ResultViewModel model) =>
            model.Algorithm == "Stein" ? GcdAlgorithms.GetGcdByStein : GcdAlgorithms.GetGcdByEuclidean;

        private Func<int, int, int, int> GetAlgorithmByThreeParams(ResultViewModel model) =>
            model.Algorithm == "Stein" ? GcdAlgorithms.GetGcdByStein : GcdAlgorithms.GetGcdByEuclidean;

        private Func<int, int, int[], int> GetAlgorithmByManyParams(ResultViewModel model) =>
            model.Algorithm == "Stein" ? GcdAlgorithms.GetGcdByStein : GcdAlgorithms.GetGcdByEuclidean;
                
        private CalculateByTwoParams<int, long> GetExtendedAlgorithmByTwoParams(ResultViewModel model) =>
            model.Algorithm == "Stein" ? GcdAlgorithms.GetGcdByStein : GcdAlgorithms.GetGcdByEuclidean;

        private CalculateByThreeParams<int, long> GetExtendedAlgorithmByThreeParams(ResultViewModel model) =>
            model.Algorithm == "Stein" ? GcdAlgorithms.GetGcdByStein : GcdAlgorithms.GetGcdByEuclidean;

        private CalculateByManyParams<int, long, int[]> GetExtendedAlgorithmByManyParams(ResultViewModel model) =>
            model.Algorithm == "Stein" ? GcdAlgorithms.GetGcdByStein : GcdAlgorithms.GetGcdByEuclidean;        

        private IEnumerable<int> GetNumberList(ResultViewModel model)
        {
            var numbers = model.NumbersOther.Split(new[] { "\n", "\r\n", " " }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var count = numbers.Length;
            var numberList = new List<int>(capacity: count);
            int number;
            for (int i = 0; i < count; i++)
            {
                if (int.TryParse(numbers[i], out number))
                {
                    numberList.Add(number);
                }
                else
                {
                    model.Error = $"Error: The entered value {numbers[i]} is not an integer in the allowed range.";
                    return Enumerable.Empty<int>();
                }
            }

            return numberList;
        }
    }
}