using MauiAppMovies.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace MauiAppMovies
{
    public partial class MainPage : ContentPage
    {
        string apiKey = "63ff67fb44d6b922e7b92ac62c6188b1";
        string movieUri = "https://api.themoviedb.org/3/";
        private TrendingMovies movieList;
        private GenreList genres;
        public ObservableCollection<Genre> Genres { get; set; } = new();
        public ObservableCollection<MovieResult> Movies { get; set; } = new();
        public bool IsLoading { get; set; }
        private readonly HttpClient httpClient;

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
            genres = await httpClient.GetFromJsonAsync<GenreList>($"genre/movie/list?api_key={apiKey}&language=en-US");
            movieList = await httpClient.GetFromJsonAsync<TrendingMovies>($"trending/movie/week?api_key={apiKey}&language=en-US");
            IsLoading = false;
            OnPropertyChanged(nameof(IsLoading));
        }

     
    }

}
