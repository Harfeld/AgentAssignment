using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace Lab8
{
    public class MainWindowViewModel : BindableBase
    {
        ObservableCollection<Agent> agents;
        string FileName = "";
        DispatcherTimer ticker = new DispatcherTimer();

        public MainWindowViewModel()
        {
            agents = new ObservableCollection<Agent> {
                #if DEBUG
                new Agent("0022", "Kusken", "KBU", "Herning"),
                new Agent("0028", "Fisken", "Studere", "Aarhus")
                #endif
            };
            CurrentAgent = agents[0];

            ticker.Interval = TimeSpan.FromSeconds(1);
            ticker.Tick += new EventHandler(Ticked);
            ticker.Start();
        }

        void Ticked(object sender, EventArgs e)
        {
            clock.Update();
        }

        #region Properties

        Clock clock = new Clock();
        public Clock Clock { get => clock; set => clock = value; }

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
            set { SetProperty(ref agents, value); }
        }
        #endregion

        #region opgave1 Commands

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
            get
            {
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
                () =>
                {
                    Agents.RemoveAt(CurrentIndex);
                    RaisePropertyChanged("Count");
                },
                () =>
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

        #region opgave2 Commands

        ICommand _SaveAsCommand;
        public ICommand SaveAsCommand
        {
            get
            {
                return _SaveAsCommand ?? (_SaveAsCommand = new DelegateCommand<string>(
                    (string argFileName) =>
                    {
                        if (argFileName == "")
                        {
                            MessageBox.Show("You must enter a file name in the File Name textbox!", "Unable to save file",
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        else
                        {
                            FileName = argFileName;
                            SaveFile();
                        }
                    }));
            }
        }

        ICommand _SaveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _SaveCommand ?? (_SaveCommand = new DelegateCommand(SaveFile, IsSavePossible)
                  .ObservesProperty(() => Agents.Count));
            }
        }

        private bool IsSavePossible()
        {
            return (FileName != "") && (Agents.Count > 0);
        }

        private void SaveFile()
        {
            // Create an instance of the XmlSerializer class and specify the type of object to serialize.
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Agent>));
            TextWriter writer = new StreamWriter(FileName);
            // Serialize all the agents.
            serializer.Serialize(writer, Agents);
            writer.Close();
        }

        ICommand _NewFileCommand;
        public ICommand NewFileCommand
        {
            get
            {
                return _NewFileCommand ?? (_NewFileCommand = new DelegateCommand(() =>
                {
                    MessageBoxResult res = MessageBox.Show("Any unsaved data will be lost. Are you sure you want to initiate a new file?", "Warning",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                    if (res == MessageBoxResult.Yes)
                    {
                        Agents.Clear();
                        FileName = "";
                    }
                }));
            }
        }

        ICommand _OpenFileCommand;
        public ICommand OpenFileCommand
        {
            get
            {
                return _OpenFileCommand ?? (_OpenFileCommand = new DelegateCommand<string>(
                    (string argFileName) =>
                    {
                        if (argFileName == "")
                        {
                            MessageBox.Show("You must enter a file name in the File Name textbox!", "Unable to save file",
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        else
                        {
                            FileName = argFileName;
                            var tempAgents = new ObservableCollection<Agent>();

                            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Agent>));
                            try
                            {
                                TextReader reader = new StreamReader(FileName);
                                tempAgents = (ObservableCollection<Agent>)serializer.Deserialize(reader);
                                reader.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Unable to open file", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            Agents = tempAgents;
                        }
                    }));
            }
        }

        #endregion

        #region Color Commands

        ICommand _ColorCommand;
        public ICommand ColorCommand
        {
            get
            {
                return _ColorCommand ?? (_ColorCommand = new DelegateCommand<String>(
                        (String colorStr) =>
                        {
                            SolidColorBrush newBrush = SystemColors.WindowBrush; // Default color

                            try
                            {
                                if (colorStr != null)
                                {
                                    if (colorStr != "Default")
                                        newBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorStr));
                                }
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Unknown color name, default color is used", "Program error!");
                            }

                            Application.Current.Resources["xmlBrush"] = newBrush;
                        }));
            }
        }  

        #endregion
    }
}
