namespace CRApiSolution.Exceptions
{
    // Exception for failures in the popular movies service
    public class PopularMoviesException : Exception
    {
        public PopularMoviesException(string message) : base(message) { }
    }

    // Exception for failures in intelligent schedule generation
    public class IntelligentScheduleGenerationException : Exception
    {
        public IntelligentScheduleGenerationException(string message) : base(message) { }
    }

    // Exception for invalid query parameters
    public class InvalidQueryParametersException : Exception
    {
        public InvalidQueryParametersException(string message) : base(message) { }
    }

    // Exception for failures in retrieving successful movies
    public class SuccessfulMoviesException : Exception
    {
        public SuccessfulMoviesException(string message) : base(message) { }
    }

    // Exception for when no matches are found
    public class NoMatchException : Exception
    {
        public NoMatchException(string message) : base(message) { }
    }

}
