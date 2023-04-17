namespace GcdCalculator.Models
{
    /// <summary>
    /// Represents a view model of the result to calculates GCD.
    /// </summary>
    public class ResultViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultViewModel"/> class
        /// </summary>
        /// <param name="countNumbers">The number of integers for which it is necessary to find a common divisor</param>
        /// <param name="first">The first integer</param>
        /// <param name="second">The second integer</param>
        /// <param name="third">The third integer</param>
        /// <param name="numbersOther">Other integers</param>
        /// <param name="algorithm">The algorithm by which it is necessary to perform the calculation</param>
        /// <param name="extended">Indication of the need to calculate the execution time of the algorithm</param>
        public ResultViewModel(string countNumbers, int? first, int? second, int? third, string? numbersOther, string algorithm, string extended) =>
            (this.CountNumbers, this.First, this.Second, this.Third, this.NumbersOther, this.Algorithm, this.Extended) = 
            (countNumbers, first, second, third, numbersOther, algorithm, extended);

        /// <summary>
        /// Gets or sets the number of integers for which it is necessary to find a common divisor
        /// </summary>
        public string CountNumbers { get; set; }

        /// <summary>
        /// Gets or sets the first integer
        /// </summary>
        public int? First { get; set; }

        /// <summary>
        /// Gets or sets the second integer
        /// </summary>
        public int? Second { get; set; }

        /// <summary>
        /// Gets or sets the third integer
        /// </summary>
        public int? Third { get; set; }

        /// <summary>
        /// Gets or sets other integers
        /// </summary>
        public string? NumbersOther { get; set; }

        /// <summary>
        /// Gets or sets the algorithm by which it is necessary to perform the calculation
        /// </summary>
        public string Algorithm { get; set; }

        /// <summary>
        /// Gets or sets the indication of the need to calculate the execution time of the algorithm
        /// </summary>
        public string Extended { get; set; }

        /// <summary>
        /// Gets or sets the error value
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Gets or sets the calculated result
        /// </summary>
        public int? Result { get; set; }

        /// <summary>
        /// Gets or sets the calculation time in milliseconds
        /// </summary>
        public long? CalculationTime { get; set; }
    }
}
