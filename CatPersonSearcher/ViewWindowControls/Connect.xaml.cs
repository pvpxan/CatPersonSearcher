﻿using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CatPersonSearcher
{
    /// <summary>
    /// Interaction logic for Connect.xaml
    /// </summary>
    public partial class Connect : UserControl
    {
        public Connect()
        {
            InitializeComponent();
            this.DataContext = new ConnectViewModel();
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                FactoryService.Register(this.DataContext as ViewModelBase, parentWindow);
            }
        }
    }
}
