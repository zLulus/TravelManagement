using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AllDemo;
using Esri.ArcGISRuntime.Controls;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks.Query;
using SimpleMvvmToolkit;
using TravelManagement.Services;
// Toolkit namespace

namespace TravelManagement.ViewModels
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         Use the <strong>mvvmprop</strong> snippet to add bindable properties to this ViewModel.
    ///     </para>
    /// </summary>
    public class QueryScenicInforViewModel : ViewModelBase<QueryScenicInforViewModel>
    {
        #region  fileds

        private bool isFirst = true;
        private MapView MyMapView;
        private GraphicsOverlay _graphicsOverlays;
        private GraphicsOverlay _states;
        private readonly IMapService FestureServiceAgent;
        private IReadOnlyList<Feature> mapFeature;
        private Feature _resultFeature;

        public IReadOnlyList<Feature> MapFeature
        {
            get { return mapFeature; }
            set
            {
                mapFeature = value;
                NotifyPropertyChanged(m => m.MapFeature);
            }
        }

        public Feature ResultFeature
        {
            get { return _resultFeature; }
            set
            {
                _resultFeature = value;
                NotifyPropertyChanged(m => m.ResultFeature);
            }
        }

        #endregion

        #region Command

        public ICommand LoadCommand
        {
            get
            {
                return new DelegateCommand<MapView>(mapView =>
                {
                    MyMapView = mapView;
                    _graphicsOverlays = MyMapView.GraphicsOverlays["graphicsOverlay"];
                    _states = MyMapView.GraphicsOverlays["states"];
                    if (isFirst)
                    {
                        CrackHelper.Crack();
                        FestureServiceAgent.CreateLocalFeatureService(MyMapView);
                        isFirst = false;
                    }
                });
            }
        }

        public ICommand SelectedLayerCommand
        {
            get
            {
                return new DelegateCommand<ComboBox>(async queryComboBox =>
                {
                    if (Global.LocalFeatureUrl != string.Empty && queryComboBox.SelectedIndex != 0)
                    {
                        var queryTask = new QueryTask(
                            new Uri(Global.LocalFeatureUrl + "/" + (queryComboBox.SelectedIndex - 1)));
                        var query = new Query(string.Format("1=1", ""))
                        {
                            OutFields = OutFields.All,
                            ReturnGeometry = true,
                            OutSpatialReference = MyMapView.SpatialReference
                        };
                        var result = await queryTask.ExecuteAsync(query);

                        var featureSet = result.FeatureSet;
                        if (featureSet != null && featureSet.Features.Count > 0)
                        {
                            var graphic = featureSet.Features;
                            MapFeature = graphic;
                        }
                    }
                });
            }
        }

        public ICommand ListViewSelectCommand
        {
            get
            {
                return new DelegateCommand<ListView>((lv =>
                {
                    _graphicsOverlays.Graphics.Clear();
                    var symbol = new SimpleLineSymbol {Color = Colors.LawnGreen, Width = 4};
                    if (lv.SelectedItem != null)
                    {
                        var graphic = new Graphic((lv.SelectedItem as Feature).Geometry, symbol);
                        if (graphic != null)
                        {
                           // _states.Graphics.Clear();
                            graphic.Attributes["Name"] = (lv.SelectedItem as Feature).Attributes["Name"];
                            var selectedFeatureExtent = graphic.Geometry.Extent;
                            var displayExtent = selectedFeatureExtent.Expand(1.3);

                            _states.Graphics.Add(graphic);
                           // ChangeRenderer();
                            MyMapView.SetView(displayExtent);
                            _graphicsOverlays.Graphics.Add(graphic);
                        }
                    }
                }));
            }
        }

        #endregion

        public QueryScenicInforViewModel(IMapService serviceAgent)
        {
            FestureServiceAgent = serviceAgent;
        }

        public async void MyMapView_OnMapViewTapped(object sender, MapViewInputEventArgs e)
        {
            for (var i = 0; i < MyMapView.Map.Layers.Count; i++)
            {
                var layer = MyMapView.Map.Layers[i];
                if (layer is FeatureLayer)
                {
                    var _featureLayer = layer as FeatureLayer;
                    var rows = await _featureLayer.HitTestAsync(MyMapView, e.Position);
                    if (rows != null && rows.Length > 0)
                    {
                        // Forcing query to be executed against local cache
                        var features = await (_featureLayer.FeatureTable as ServiceFeatureTable).QueryAsync(rows, true);
                        ResultFeature = features.FirstOrDefault();
                        break; //查出来 即停止 不需要继续循环
                    }
                    ResultFeature = null;
                }
            }
        }


        private void ChangeRenderer()
        {
         
            var renderer = new UniqueValueRenderer()
            {
                Fields = new ObservableCollection<string>(new List<string> { "Name" })
            };

            renderer.Infos = new UniqueValueInfoCollection(_states.Graphics
                .Select(g => g.Attributes["Name"])
                .Distinct()
                .Select(obj => new UniqueValueInfo
                {
                    Values = new ObservableCollection<object>(new object[] { obj }),
                    Symbol = new SimpleFillSymbol() { Outline = new SimpleLineSymbol() { Color =Colors.BlueViolet,Width = 6}, Color = Colors.Black  }
                }));

            _states.Renderer = renderer;
        }
    }
}


