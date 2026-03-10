using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using MusicPlusPlus.Context;
using MusicPlusPlus.Models;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;

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
        CoverI.Source = new Bitmap("Resources/placeholder_cover.png");
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
        if (NameTB.Text == "" || NameTB.Text == null || CoverPathTB.Text == "" || ArtistsCB.SelectedItem == null || SelectedGenresLB.Items.Count == 0)
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
        if (GenresACB.SelectedItem != null && !SelectedGenresLB.Items.Contains(GenresACB.SelectedItem))
        {
            SelectedGenresLB.Items.Add(GenresACB.SelectedItem);
        }
        GenresACB.SelectedItem = null;
    }

    //кнопка удаления выбранного жанра из списка
    private void GenreDeleteB_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (SelectedGenresLB.SelectedItem != null)
        {
            SelectedGenresLB.Items.Remove(SelectedGenresLB.SelectedItem);
        }
    }

    //поиск обложки
    private async void FindCoverB_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string imagepath;
        if (CoverPathTB.Text != null || CoverPathTB.Text.Trim() != "")
        {
            imagepath = CoverPathTB.Text.Trim();
        }
        else
        {
            return;
        }

        try
        {
            using (HttpClient client = new HttpClient())
            {
                byte[] imagedata = await client.GetByteArrayAsync(imagepath);
                using (var stream = new MemoryStream(imagedata))
                {
                    Bitmap image = new Bitmap(stream);
                    /*
                     if (image.PixelSize.Width > 1000 || image.PixelSize.Height > 1000)
                    {
                        new MessageWindow("Некорректный ввод", "Изображение не должно превышать 1000x1000").Show();
                        CoverPathTB.Clear();
                        return;
                    }
                     */
                    CoverI.Source = image;
                }
            }
        }
        catch(Exception ex)
        {
            try
            {
                CoverI.Source = new Bitmap(imagepath);
            }
            catch (Exception ex2)
            {
                new MessageWindow("Ошибка", ex.Message).Show();
                CoverPathTB.Clear();
            }
            
        }
    }
}