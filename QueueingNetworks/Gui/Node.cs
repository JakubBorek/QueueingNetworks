using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Gui
{
    public class Node : INotifyPropertyChanged
    {
        public Node()
        {
            Connections = new List<Connection>();
        }
        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                NotifyPropertyChanged("Color");
            }
        }

        public string Name { get; set; }

        public Brush Color
        {
            get { return Selected ? Brushes.Red : Brushes.Black; }
        }

        public Node This
        {
            get { return this; }
        }

        public List<Connection> Connections { get; set; }

        public List<double> Mi { get; set; }

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}
