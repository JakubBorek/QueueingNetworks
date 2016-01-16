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
            Mi = new List<double>() { 1 };
            type = NodeType.Type1;

        }

        public enum NodeType { Type1, Type3 };
        private NodeType type;
        public NodeType Type
        {
            get { return type; }
            set
            {
                type = value;
                NotifyPropertyChanged("Type");
            }
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

        public void UpdateMiLength(int length)
        {
            while (Mi.Count < length)
            {
                Mi.Add(1);
            }

            while (Mi.Count > length)
            {
                Mi.RemoveAt(Mi.Count - 1);
            }
        }

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
