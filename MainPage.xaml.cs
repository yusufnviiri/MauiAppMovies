using CommunityToolkit.Maui.Views;
using MauiAppMovies.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Windows.Input;

namespace MauiAppMovies
{
    public partial class MainPage : ContentPage
    {
        string apiKey = "63ff67fb44d6b922e7b92ac62c6188b1";
        string movieUri = "https://api.themoviedb.org/3/";
        string imageBaseUri = "https://image.tmdb.org/t/p/w500";
        public ICommand ShowMovies => new Command <MovieResult>((movie) => ShowMovieDetails(movie)); 
        List<UserGenre> genreList { get; set; } = new();

        private TrendingMovies movieList;
        private GenreList _genres;
        public ObservableCollection<Genre> Genres { get; set; } = new();
        public ObservableCollection<MovieResult> Movies { get; set; } = new();
        public bool IsLoading { get; set; }
        private readonly HttpClient httpClient;
        public ICommand ChooseGenres => new Command(async () => await ShowGenreList());

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            httpClient = new HttpClient { BaseAddress=new Uri(movieUri)};
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            IsLoading = true;
            OnPropertyChanged(nameof(IsLoading));
            _genres = await httpClient.GetFromJsonAsync<GenreList>($"genre/movie/list?api_key={apiKey}&language=en-US");
            movieList = await httpClient.GetFromJsonAsync<TrendingMovies>($"trending/movie/week?api_key={apiKey}&language=en-US");

            foreach (var movie in movieList.results)
            {
                movie.poster_path = $"{imageBaseUri}{movie.poster_path}";

            }
            foreach(var genre in _genres.genres)
            {
                genreList.Add(new UserGenre { id=genre.id,name=genre.name,Selected=false });
            }
            LoadFilteredMovies();

            IsLoading = false;
            OnPropertyChanged(nameof(IsLoading));
        }

        private void LoadFilteredMovies() {
            Movies.Clear();
            if (genreList.Any(g => g.Selected))
            {
                var selectedGenrId = genreList.Where(g => g.Selected).Select(g=>g.id);
                foreach (var movie in movieList.results)
                {
                    if (movie.genre_ids.Any(id => selectedGenrId.Contains(id)))
                    {
                        Movies.Add(movie);
                    }
                }
            }
            else
            {
                foreach (var movie in movieList.results)
                {
                    Movies.Add(movie);
                }
                }
        }
        private void ShowMovieDetails(MovieResult movie)
        {
            var moviePopup = new MovieDetailsPopup(movie, _genres.genres);
            this.ShowPopup(moviePopup);
        }

        private async Task ShowGenreList()
        {
            var genrePopup = new GenreListPopup(genreList); 
var selected = await this.ShowPopupAsync(genrePopup); 
if ((bool)selected)
            {
                Genres.Clear();
                foreach (var genre in genreList)
                {
                    if (genre.Selected)
                    {
                        Genres.Add(new Genre
                        {
                            name = genre.name
                        });
                    }
                }
                LoadFilteredMovies();
            }
        }
    }



}


