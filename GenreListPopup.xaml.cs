using CommunityToolkit.Maui.Views;
using MauiAppMovies.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Windows.Input;
namespace MauiAppMovies;

public partial class GenreListPopup : Popup
{
    public ObservableCollection<UserGenre> Genres { get; set; }
    private bool _selectionHasChanged = false;

    public GenreListPopup(List<UserGenre> Genres)
    {
        BindingContext = this;
        this.Genres = new ObservableCollection<UserGenre>(Genres);
        ResultWhenUserTapsOutsideOfPopup = _selectionHasChanged;
        InitializeComponent();
    }


    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectionHasChanged = true;
        var selectedItems = e.CurrentSelection;
        foreach (var genre in Genres)
        {
            if (selectedItems.Contains(genre))
            {
                genre.Selected = true;
            }
            else
            {
                genre.Selected = false;
            }
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close(_selectionHasChanged);
    }


}