using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MusicPlusPlus.Context;
using MusicPlusPlus.Models;
using System;
using System.Linq;

namespace MusicPlusPlus;

public partial class AlbumCreationWindow : Window
{
    public AlbumCreationWindow()
    {
        InitializeComponent();
        LoadInfo();
    }

    //подгрузка инфы при открытии окна
    private void LoadInfo()
    {
        NameTB.Focus();
        using (var context = new MusicdbContext())
        {
            var artists = context.Artists.Select(x => x.Artistname).ToList();
            ArtistsCB.ItemsSource = artists;

            var genres = context.Genres.Select(x => x.Genrename).ToList();
            GenresACB.ItemsSource = genres;

            YearDP.SelectedDate = DateTime.Now;
        }
    }

    //проверка корректности ввода даты
    private void YearDP_SelectedDateChanged(object? sender, DatePickerSelectedValueChangedEventArgs e)
    {
        if (YearDP.SelectedDate >= DateTime.Today + TimeSpan.FromDays(365))
        {
            YearDP.SelectedDate = DateTime.Now;
            new MessageWindow("Некорректный ввод", "Неверно введён год.").Show();
        }
    }

    //кнопка сохранения альбома
    private void SaveB_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (NameTB.Text == "" || NameTB.Text == null || CoverPathTB.Text == "" || SelectedGenresLB.Items.Count == 0)
        {
            new MessageWindow("Некорректный ввод", "Заполните все поля!").Show();
            return;
        }

        try
        {
            using (var context = new MusicdbContext())
            {
                string albumname = NameTB.Text.Trim();
                string coverpath = CoverPathTB.Text.Trim();

                Album newalbum = new Album
                {
                    Albumname = albumname,
                    Albumreleaseyear = YearDP.SelectedDate.Value.Year.ToString(),
                    Coverpath = coverpath,
                    Albumduration = "0"
                };

                context.Albums.Add(newalbum);
                context.SaveChanges();

                
                var album = context.Albums.OrderBy(x => x.Albumid).LastOrDefault();
                var artist = context.Artists.FirstOrDefault(x => x.Artistname == ArtistsCB.SelectedItem);
                

                

                /*var genresgroup = SelectedGenresLB.Items.Cast<Genre>().GroupBy(x => x.Genreid).Select(x => new { Genreid = x.Key });
                foreach (var genre in genresgroup)
                {
                    var genreinalbum = new Genre
                    {
                        
                    };
                }*/
            }
            this.Close();
        }
        catch (Exception ex) 
        {
            new MessageWindow("Ошибка", $"{ex}").Show();
            return;
        }
    }

    //открытие окна добавления исполнителя 
    private void AddArtistB_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

    }

    

    //загрузка списка выбранных жанров
    private void GenresACB_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (GenresACB.SelectedItem != null)
        {
            SelectedGenresLB.Items.Add(GenresACB.SelectedItem);
        }
    }
    

    //jjj
}