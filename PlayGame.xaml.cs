
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http.Json;
using roverthing1.Classes;
using MonkeyCache.FileStore;
using roverthing1.ViewModels;

namespace roverthing1;

public partial class PlayGame : ContentPage
{
    public PlayGame(PlayGameViewModel model)
    {
        InitializeComponent();
        BindingContext= model;
    }
}

