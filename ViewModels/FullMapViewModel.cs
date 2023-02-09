using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MonkeyCache.FileStore;
using System.Xml.Linq;
using roverthing1.Classes;

namespace roverthing1.ViewModels;

public partial class FullMapViewModel : ObservableObject
{
    [ObservableProperty]
    private MapCell[,] cells;

    public RoverAPIService service;

    public FullMapViewModel(RoverAPIService service)
    {
      this.service = service;
        Cells = service.map;
    }
}