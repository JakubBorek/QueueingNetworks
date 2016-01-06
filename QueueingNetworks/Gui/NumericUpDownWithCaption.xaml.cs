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

namespace Gui
{
    /// <summary>
    /// Interaction logic for NumericUpDownWithCaption.xaml
    /// </summary>
    public partial class NumericUpDownWithCaption : UserControl
    {
        public NumericUpDownWithCaption()
        {
            InitializeComponent();
            Label.Content = "";
            UpDown.Minimum = 0;
        }

        public string Caption
        {
            get { return (string)Label.Content; }
            set { Label.Content = value; }
        }

        public int Value
        {
            get { return (int)UpDown.Value; }
            set { UpDown.Value = value; }
        }
    }
}
