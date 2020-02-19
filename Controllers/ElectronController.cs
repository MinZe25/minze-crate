using System;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace electronNetTest.Controllers
{
    public class ElectronController
    {
        public BrowserWindow MainWindow;

        public ElectronController()
        {
        }

        public void ConfigureElectron()
        {
            var options = new BrowserWindowOptions
            {
                Title = "MBox Configurator",
                TitleBarStyle = TitleBarStyle.customButtonsOnHover,
                DarkTheme = true
            };
            Task.Run(async () => await Electron.WindowManager.CreateWindowAsync(options))
                .ContinueWith(res => BuildMenu(res.Result));
        }

        private void BuildMenu(BrowserWindow window)
        {
            MainWindow = window;

            var submenu = new[]
            {
                new MenuItem
                {
                    Label = "Load Configuration",
                    Type = MenuType.normal,
                    Click = LoadConfiguration,
                    Accelerator = "CmdOrCtrl+O"
                },
                new MenuItem
                {
                    Label = "Save Configuration",
                    Type = MenuType.normal,
                    Click = SaveConfiguration,
                    Accelerator = "CmdOrCtrl+S"
                }
            };
            var file = new MenuItem
            {
                Label = "File",
                Type = MenuType.submenu,
                Submenu = submenu
            };
            var menu = new MenuItem[1];
            menu[0] = file;
            MainWindow.SetMenu(menu);
            MainWindow.OnClosed += () =>
            {
                MainWindow.Destroy();
                MainWindow = null;
                System.Environment.Exit(0);
            };
        }

        private void LoadConfiguration()
        {
            Task.Run(() =>
            {
                var options = new MessageBoxOptions("Load configuration was pressed")
                {
                    Title = "Title of pressed",
                    Type = MessageBoxType.question,
                    Detail = "This is the detail",
                    Buttons = new[] {"Okay", "Not Okay", "Okay boomer"}
                };

                return Electron.Dialog.ShowMessageBoxAsync(MainWindow, options);
            }).ContinueWith(res => Console.WriteLine(res.Result.Response));
        }

        private void SaveConfiguration()
        {
            Task.Run((() =>
            {
                var options = new SaveDialogOptions()
                {
                    Message = "Save the configuration file",
                    Title = "Save",
                    Filters = new[]
                    {
                        new FileFilter()
                        {
                            Name = "MinZeBox Configuration",
                            Extensions = new[] {"mbox"}
                        }
                    },
                    DefaultPath = "~/configuration.mbox"
                };
                return Electron.Dialog.ShowSaveDialogAsync(MainWindow, options);
            })).ContinueWith(task => Console.WriteLine(task.Result));
        }

        private void OpenNewWindow()
        {
            
        }
    }
}