/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:TravelManagement"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Media.Animation;
using ModernUINavigation.Pages.Settings;
using Newtonsoft.Json;

// Toolkit namespace
using SimpleMvvmToolkit;
using TravelManagement.Helpers;
using TravelManagement.Models;
using TravelManagement.Services;
using TravelManagement.ViewModels;

namespace TravelManagement
{
    /// <summary>
    /// This class creates ViewModels on demand for Views, supplying a
    /// ServiceAgent to the ViewModel if required.
    /// <para>
    /// Place the ViewModelLocator in the App.xaml resources:
    /// </para>
    /// <code>
    /// &lt;Application.Resources&gt;
    ///     &lt;vm:ViewModelLocator xmlns:vm="clr-namespace:TravelManagement"
    ///                                  x:Key="Locator" /&gt;
    /// &lt;/Application.Resources&gt;
    /// </code>
    /// <para>
    /// Then use:
    /// </para>
    /// <code>
    /// DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
    /// </code>
    /// <para>
    /// Use the <strong>mvvmlocator</strong> or <strong>mvvmlocatornosa</strong>
    /// code snippets to add ViewModels to this locator.
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        // Create MainPageViewModel on demand
        //public MainPageViewModel MainPageViewModel
        //{
        //    get { return new MainPageViewModel(); }
        //}

        public ModelsUserControlViewModel ModelsUserControlViewModel
        {
            get
            {
                return new ModelsUserControlViewModel();
            }
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                return new MainWindowViewModel();
            }
        }

        public AppearanceViewModel AppearanceViewModel
        {
            get { return new AppearanceViewModel(); }
        }

        public MapLayerViewModel CommonMapViewModel
        {
            get
            {
                IMapService serviceAgent = new LocalMapServiceAgent();
                return new MapLayerViewModel(serviceAgent);
            }
        }

        public ScenicSpotManageViewModel ScenicSpotViewModel
        {
            get
            {
                ISceniceSpot sceniceAgent = new SceniceSpotServiceAgent();
                return new ScenicSpotManageViewModel(sceniceAgent);
            }
        }

        public PedestrianVolumeViewModel PedestrianVolumeViewModel
        {
            get { return new PedestrianVolumeViewModel(new PedestrianVolumeServer()); }
        }

        public ScanScenicSpotsViewModel ScanScenicSpotsViewModel
        {
            get { return new ScanScenicSpotsViewModel(); }
        }

        public QueryScenicInforViewModel QueryScenicInforViewModel
        {
            get
            {
                IMapService serviceAgent = new LocalMapServiceAgent();
                return new QueryScenicInforViewModel(serviceAgent);
            }
        }

        public ManageTouristViewModel ManageTouristViewModel
        {
            get
            {
                ITouristPoints serviceAgent = new TouristPointsManage();
                return new ManageTouristViewModel(serviceAgent);
            }
        }

        public EmergencyCallViewModel EmergencyCallViewModel
        {
            get { return new EmergencyCallViewModel(new EmergencyCallServer()); }
        }

        public LostContactViewModel LostContactViewModel
        {
            get { return new LostContactViewModel(new LostContactServer()); }
        }

        public QueryTourisViewModel QueryTourisViewModel
        {
            get
            {
                ITouristPoints serviceAgent = new TouristPointsManage();
                return new QueryTourisViewModel(serviceAgent);
            }
        }

        public AddAdminViewModel AddAdminViewModel
        {
            get
            {
                return new AddAdminViewModel(new AddAdminServer());
            }
        }

        public CheckAdminViewModel CheckAdminViewModel
        {
            get
            {
                return new CheckAdminViewModel(new CheckAdminServer());
            }
        }

        public LogInViewModel LogInViewModel
        {
            get
            {
                return new LogInViewModel(new LogInServer());
            }
        }

        public ServerLogViewModel ServerLogViewModel
        {
            get
            {
                //测试用
                //return new ServerLogViewModel(new ServerLogServer());
                return new ServerLogViewModel(Global.LogServer);
            }
        }

        public SendMsgsViewModel SendMsgsViewModel
        {
            get
            {
                return new SendMsgsViewModel(new SendMsgsServer());
            }
        }

        public SendEmergencyMsgsViewModel SendEmergencyMsgsViewModel
        {
            get
            {
                return new SendEmergencyMsgsViewModel(new SendEmergencyMsgsServer());
            }
        }
    }
}