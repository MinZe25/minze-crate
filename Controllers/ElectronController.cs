using System;
using System.Net;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace electronNetTest.Controllers
{
    public class ElectronController
    {
        public BrowserWindow MainWindow;
        public int PortNumber;

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
            var submenuFile = new[]
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
                },
                new MenuItem()
                {
                    Type = MenuType.separator
                },
                new MenuItem()
                {
                    Label = "Exit",
                    Type = MenuType.normal,
                    Click = Exit,
                    Accelerator = "Alt+F4"
                }
            };
            var file = new MenuItem
            {
                Label = "File",
                Type = MenuType.submenu,
                Submenu = submenuFile
            };
            var submenuAdvanced = new[]
            {
                new MenuItem()
                {
                    Label = "Change Modifier Angles",
                    Type = MenuType.normal,
                    Accelerator = "F2"
                }
            };
            var advanced = new MenuItem()
            {
                Label = "Advanced",
                Type = MenuType.submenu,
                Submenu = submenuAdvanced
            };
            var submenuBox = new[]
            {
                new MenuItem()
                {
                    Label = "Port",
                    Type = MenuType.submenu,
                    Submenu = new[]
                    {
                        new MenuItem()
                        {
                            Label = "Reload"
                        },
                        new MenuItem()
                        {
                            Type = MenuType.separator
                        }
                    }
                },
                new MenuItem()
                {
                    Label = "Upload configuration"
                }
            };
            var box = new MenuItem()
            {
                Label = "Box",
                Type = MenuType.submenu,
                Submenu = submenuBox
            };
            var submenuHelp = new[]
            {
                new MenuItem()
                {
                    Label = "About",
                    Accelerator = "F1",
                    Click = OpenNewWindow
                }
            };
            var help = new MenuItem()
            {
                Label = "Help",
                Type = MenuType.submenu,
                Submenu = submenuHelp,
            };
            var menu = new[]
            {
                file,
                advanced,
                box,
                help
            };
            MainWindow.SetMenu(menu);
            MainWindow.OnClosed += Exit;
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
            var options = new BrowserWindowOptions
            {
                Title = "Angles Configurator",
                TitleBarStyle = TitleBarStyle.customButtonsOnHover,
                DarkTheme = true,
                AutoHideMenuBar = true
            };
            Task.Run(() =>
                    Electron.WindowManager.CreateWindowAsync(options, $"http://localhost:{PortNumber}/home/privacy"))
                .ContinueWith(task =>
                {
                    var w = task.Result;
                    w.SetMenu(new []
                    {
                        new MenuItem()
                        {
                            Label = ""
                        }
                    });
                    w.SetMenuBarVisibility(false);
                });
        }

        private void Exit()
        {
            MainWindow?.Destroy();
            MainWindow = null;
            System.Environment.Exit(0);
        }

        public bool SetPortNumber(string url)
        {
            return int.TryParse(url.Substring(url.LastIndexOf(':') + 1), out PortNumber);
        }
    }
}