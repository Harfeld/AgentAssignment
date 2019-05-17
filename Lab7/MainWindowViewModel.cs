using Lab7;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace opgave1
{
    public class MainWindowViewModel : BindableBase
    {
        ObservableCollection<Agent> agents;

        public MainWindowViewModel()
        {
            agents = new ObservableCollection<Agent> {
            new Agent("0022", "Kusken", "KBU", "Herning"),
            new Agent("0028", "Fisken", "Studere", "Aarhus")
        };
        CurrentAgent = agents[0];
        }

        #region Properties

        Agent currentAgent = null;
        public Agent CurrentAgent
        {
            get { return currentAgent; }
            set
            {
                if (currentAgent != value)
                {
                    SetProperty(ref currentAgent, value);
                }
            }
        }
        
        int currentIndex = -1;
        public int CurrentIndex
        {
            get { return currentIndex; }
            set
            {
                SetProperty(ref currentIndex, value);
            }
        }
        
        public ObservableCollection<Agent> Agents
        {
            get { return agents; }
        }
        #endregion

        #region Commands

        ICommand _PreviousCommand;
        public ICommand PreviusCommand
        {
            get
            {
                return _PreviousCommand ??
                (_PreviousCommand = new DelegateCommand(
                    () => --CurrentIndex, 
                    () => CurrentIndex > 0).ObservesProperty(() => CurrentIndex));
            }
        }

        ICommand _NextCommand;
        public ICommand NextCommand
        {
            get
            {
                return _NextCommand ?? (_NextCommand = new DelegateCommand(
                       () => ++CurrentIndex,
                       () => CurrentIndex < (Agents.Count - 1)).ObservesProperty(() => CurrentIndex));
            }
        }

        ICommand _AddAgentCommand;
        public ICommand AddAgentCommand
        {
            get {
                return _AddAgentCommand ?? (_AddAgentCommand = new DelegateCommand(() => 
                {
                    Agents.Add(new Agent());
                    CurrentIndex = Agents.Count - 1;
                }));
            }
        }

        ICommand _DeleteAgentCommand;
        public ICommand DeleteAgentCommand => _DeleteAgentCommand ?? (_DeleteAgentCommand = 
            new DelegateCommand(
                ()=>
                {
                    Agents.RemoveAt(CurrentIndex);
                    RaisePropertyChanged("Count");
                }, 
                ()=>
                {
                    if (Agents.Count > 0 && CurrentIndex >= 0)
                        return true;
                    else
                        return false;
                })
            .ObservesProperty(() => CurrentIndex));

        ICommand _CloseAppCommand;
        public ICommand CloseAppCommand
        {
            get
            {
                return _CloseAppCommand ?? (_CloseAppCommand = new DelegateCommand(() =>
                {
                    App.Current.MainWindow.Close();
                }));
            }
        }
        #endregion
    }
}