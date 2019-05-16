using opgave1;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace opgave1
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        ObservableCollection<Agent> agents = new ObservableCollection<Agent>();

        public MainWindowViewModel()
        {
            Agents.Add(new Agent("0022", "Kusken", "KBU", "Herning"));
            Agents.Add(new Agent("0028", "Fisken", "Studere", "Aarhus"));

            //DataContext = Agents;
        }

        Agent currentAgent = null;
        public Agent CurrentAgent
        {
            get { return currentAgent; }
            set
            {
                if (currentAgent != value)
                {
                    currentAgent = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ObservableCollection<Agent> Agents
        {
            get { return agents; }
        }

        public void AddNewAgent()
        {
            agents.Add(new Agent());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}