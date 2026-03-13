using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using MusicPlusPlus.Context;
using MusicPlusPlus.Models;
using System;
using System.Linq;

namespace MusicPlusPlus;

public partial class PlaylistCreationW : Window
{
    public PlaylistCreationW()
    {
        InitializeComponent();
        LoadInfo();
    }

    //загрузка тегов
    private void LoadInfo()
    {
        NameTB.Focus();

        using (var context = new MusicdbContext())
        {
            var alltags = context.Tags.Select(t => t.Tagname).ToList();
            TagsACB.ItemsSource = alltags;
        }
    }

    //перемещение выбранных тегов в листбокс
    private void TagsACB_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (TagsACB.SelectedItem != null && !SelectedTagsLB.Items.Contains(TagsACB.SelectedItem))
        {
            SelectedTagsLB.Items.Add(TagsACB.SelectedItem);
        }
        TagsACB.SelectedItem = null;
    }

    //удаление выбранного тега
    private void TagDeletionB_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (SelectedTagsLB.SelectedItem != null)
        {
            SelectedTagsLB.Items.Remove(SelectedTagsLB.SelectedItem);
        }
    }

    //удаление всех тегов
    private void ClearSelectionB_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SelectedTagsLB.Items.Clear();
    }

    //сохранение
    private void SaveB_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        //проверки
        if (string.IsNullOrWhiteSpace(NameTB.Text))
        {
            new MessageWindow("Некорректный ввод", "Введите название плейлиста").Show();
            return;
        }
        if (SelectedTagsLB.Items.Count == 0)
        {
            new MessageWindow("Некорректный ввод", "Выберите хотя бы один тег").Show();
            return;
        }

        try
        {
            using (var context = new MusicdbContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Userid == Properties.Settings.Default.userid);
                int playlistid = context.Playlists.OrderBy(x => x.Playlistid).Select(x => x.Playlistid).LastOrDefault() + 1;
                var newplaylist = new Playlist
                {
                    Playlistid = (short)playlistid,
                    Playlistname = NameTB.Text.Trim(),
                    Userid = user.Userid,
                    Playlistcreationdate = DateOnly.FromDateTime(DateTime.Now),
                    Likes = 0,
                    Playlistdescription = DescriptionTB?.Text?.Trim() ?? ""
                };

                context.Playlists.Add(newplaylist);
                context.SaveChanges();

                var playlist = context.Playlists.OrderBy(x => x.Playlistid).LastOrDefault();

                foreach (var selectedtag in SelectedTagsLB.Items)
                {
                    var tagname = selectedtag.ToString();
                    var tag = context.Tags.FirstOrDefault(t => t.Tagname == tagname);
                    if (tag != null)
                    {
                        context.Database.ExecuteSqlRaw(
                            "INSERT INTO public.playlisttags (playlistid, tagid) VALUES ({0}, {1})",
                            playlist.Playlistid,
                            tag.Tagid
                        );
                        context.SaveChanges();
                    }
                }

                this.Close();
            }
        }
        catch (Exception ex)
        {
            new MessageWindow("Ошибка", ex.Message).Show();
            return;
        }
        
    }
}