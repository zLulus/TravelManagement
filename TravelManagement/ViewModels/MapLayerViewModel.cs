using System;
using System.Collections.Generic;
using System.Windows;
using System.Threading;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AllDemo;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.LocalServices;

// Toolkit namespace
using SimpleMvvmToolkit;
using TravelManagement.Services;

namespace TravelManagement.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvmprop</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// </summary>
    public class MapLayerViewModel : ViewModelBase<MapLayerViewModel>
    {
        #region  fileds
        private MapView MyMapView;
        private IMapService serviceAgent;
        private bool isFirst = true;
        #endregion

        #region Command
        
        public ICommand LoadMapViewCommand
        {
            get
            {
                return new DelegateCommand<MapView>(mapView =>
                {
                    this.MyMapView = mapView;
                    if (isFirst)
                    {
                        CrackHelper.Crack();
                        serviceAgent.CreateLocalFeatureService(MyMapView);
                        isFirst = false;
                    }
                }); 
            }
        }

        public ICommand SwitchMapCommand
        {
            get
            {
                return new DelegateCommand<string>(Tag =>
                {
                    MyMapView.Map.Layers.Remove(MyMapView.Map.Layers["BaseMap"]);
                    MyMapView.Map.Layers.Insert(0, new ArcGISTiledMapServiceLayer
                    {
                        ID = "BaseMap",
                        DisplayName = "影像底图",
                        ServiceUri = Tag
                    });
                });
            }
        }

        #endregion

        #region Methods
        public MapLayerViewModel(IMapService serviceAgent)
        {
            this.serviceAgent = serviceAgent;
        }
        #endregion

    }
}