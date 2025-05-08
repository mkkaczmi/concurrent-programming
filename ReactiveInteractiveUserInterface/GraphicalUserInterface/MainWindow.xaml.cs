//__________________________________________________________________________________________
//
//  Copyright 2024 Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started
//  comment using the discussion panel at
//  https://github.com/mpostol/TP/discussions/182
//__________________________________________________________________________________________

using System;
using System.Windows;
using TP.ConcurrentProgramming.Presentation.ViewModel;

namespace TP.ConcurrentProgramming.PresentationView
{
    /// <summary>Code‑behind widoku głównego.</summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainWindowViewModel();
            DataContext = viewModel;

            int balls = new Random().Next(5, 10);  // losowa liczba kul 5‑9
            viewModel.Start(balls);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
                vm.Dispose();

            base.OnClosed(e);
        }
    }
}
