// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//Custom Delegate to return a custom exception
string genre = "Action";

IEnumerable<Movie> movieRepository = GetAllMovies();

var actionMovie = movieRepository.MovieListFilter(
    m => m.Genre == genre,
    exception => { return new MovieFilterException($"Only one Movie with {genre} was expected. \nHowever {exception.Count()} were found"); });

Console.WriteLine(actionMovie.Name);

//To throw the exception
List<Movie> movies = new List<Movie>(movieRepository);

movies.Add(new Movie("Die hard","Action"));

//var actionMovie2 = movies.MovieListFilter(
//    m => m.Genre == genre,
//    exception => { return new MovieFilterException($"Only one Movie with {genre} was expected. \nHowever {exception.Count()} were found"); });

static IEnumerable<Movie> GetAllMovies()
{
    yield return new Movie("The Godfather", "Drama");
    yield return new Movie("The Shawshank Redemption", "Drama");
    yield return new Movie("Forrest Gump", "Comedy");
    yield return new Movie("12 Angry Men", "Drama");
    yield return new Movie("Back to the Future", "Sci-fi");
    yield return new Movie("Terminator 2: Judgment Day", "Sci-fi");
    yield return new Movie("A Space Odyssey", "Sci-fi");
    yield return new Movie(" Raiders of the Lost Ark", "Action");
    yield return new Movie("Nightmare on elm street", "Horror");
    yield return new Movie("Friday 13", "Horror");
    yield return new Movie("Day of the dead", "Horror");
    yield return new Movie("Psycho", "Thriller");
    yield return new Movie("The Silence of the Lambs", "Thriller");
}

public static class MovieExtension
{
    public static T MovieListFilter<T>(
        this IEnumerable<T> movies, 
        Func<T,bool> filter,
        Func <IEnumerable<T>, Exception> exception)
    {
        var matchedValue = new List<T>();
        
        foreach (var m in movies) 
        {
            if (filter(m))
            {
                matchedValue.Add(m);
            }
        }

        if (matchedValue.Count == 1)
        {
            return matchedValue[0];
        }

        throw exception(matchedValue);
    }
}


class Movie
{
    public Movie(string name, string genre)
    {
        Name = name;
        Genre = genre;
    }
    public string Name { get; set; }
    public string Genre { get; set; }
}

[Serializable]
public sealed class MovieFilterException: Exception
{
    public MovieFilterException() { }

    public MovieFilterException(string message) : base(message) { } 
    public MovieFilterException(string message, Exception inner) : base(message, inner) { }

    private MovieFilterException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}