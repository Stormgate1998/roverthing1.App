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
using static UIKit.UIGestureRecognizer;

namespace roverthing1.ViewModels;

public partial class FullMapViewModel : ObservableObject
{
    [ObservableProperty]
    private Dictionary<string, MapCell> cells;

    public RoverAPIService service;

    [ObservableProperty]
    private GraphicsDrawable drawable;
    public FullMapViewModel(RoverAPIService service)
    {
      this.service = service;
        Cells = service.map;
        Drawable = new GraphicsDrawable(Cells);
    }

}