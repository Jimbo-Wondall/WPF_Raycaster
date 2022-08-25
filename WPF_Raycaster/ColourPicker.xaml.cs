using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace WPF_Raycaster
{
    /// <summary>
    /// Interaction logic for ColourPicker.xaml
    /// </summary>
    public partial class ColourPicker : Window
    {
        SolidColorBrush colour = new SolidColorBrush();
        int currentSwatch;
        public ColourPicker(int swatch)
        {
            InitializeComponent();
            if (swatch < OptionsPane.wallColors.Count)
            {
                colour = OptionsPane.wallColors[swatch];
            }
            else
            {
                colour.Color = Color.FromRgb(0,0,0);
            }
            currentSwatch = swatch;
            txtRed.Text = colour.Color.R.ToString();
            txtGreen.Text = colour.Color.G.ToString();
            txtBlue.Text = colour.Color.B.ToString();
            sliderRed.Value = colour.Color.R;
            sliderGreen.Value = colour.Color.G;
            sliderBlue.Value = colour.Color.B;

            InitEvents();
        }

        private void InitEvents()
        {
            sliderRed.ValueChanged += SliderValueChanged;
            sliderGreen.ValueChanged += SliderValueChanged;
            sliderBlue.ValueChanged += SliderValueChanged;
            

            //txtRed.KeyDown += TextKeyDown;
            //txtGreen.KeyDown += TextKeyDown;
            //txtBlue.KeyDown += TextKeyDown;

            
            txtRed.TextChanged += TextChanged;
            txtGreen.TextChanged += TextChanged;
            txtBlue.TextChanged += TextChanged;
            

            btnSave.Click += SaveSwatch;
        }

        private void TextKeyDown(object sender, KeyEventArgs e)
        {

            if (!byte.TryParse(txtRed.Text, out byte r))
            {
                txtRed.Text = "0";
                r = 0;
            }
            if (!byte.TryParse(txtGreen.Text, out byte g))
            {
                txtGreen.Text = "0";
                g = 0;
            }
            if (!byte.TryParse(txtBlue.Text, out byte b))
            {
                txtBlue.Text = "0";
                b = 0;
            }
            sliderRed.Value = r;
            sliderGreen.Value = g;
            sliderBlue.Value = b;
            SetColour(r, g, b);
        }

        private void SaveSwatch(object sender, RoutedEventArgs e)
        {
            if (currentSwatch > OptionsPane.wallColors.Count)
            {
                OptionsPane.NewColour(colour);
            }
            else
            {
                OptionsPane.UpdateColour(currentSwatch, colour);
            }

        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!byte.TryParse(txtRed.Text, out byte r))
            {
                txtRed.Text = "0";
                r = 0;
            }
            if (!byte.TryParse(txtGreen.Text, out byte g))
            {
                txtGreen.Text = "0";
                g = 0;
            }
            if (!byte.TryParse(txtBlue.Text, out byte b))
            {
                txtBlue.Text = "0";
                b = 0;
            }
            sliderRed.Value = r;
            sliderGreen.Value = g;
            sliderBlue.Value = b;
            SetColour(r,g,b);
        }
        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtRed.Text = sliderRed.Value.ToString();
            txtGreen.Text = sliderGreen.Value.ToString();
            txtBlue.Text = sliderBlue.Value.ToString();
            SetColour((byte)sliderRed.Value, (byte)sliderGreen.Value, (byte)sliderBlue.Value);
        }

        private void SetColour(byte red, byte green, byte blue)
        {
            colour.Color = Color.FromRgb(red, green, blue);
            rectColourPreview.Fill = colour;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            //TextBox txtBox = sender as TextBox;
            //e.Handled = byte.TryParse(txtBox.Text, out byte _);
        }
    }
}
