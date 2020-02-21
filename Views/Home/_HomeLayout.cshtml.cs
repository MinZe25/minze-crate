using System;
using System.Resources;
using electronNetTest.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace electronNetTest.Views.Home
{


    public class _HomeLayout : PageModel
    {
        public static string Message { get; private set; } = "PageModel in C#";

        public static ViewPage[] Pages { get; private set; } = new[]
        {
            new ViewPage()
            {
                controller = "Home",
                view = "Index"
            },
            new ViewPage()
            {
                controller = "Home",
                view = "Privacy"
            }
        };

        public void OnGet()
        {
            Console.WriteLine("?");
        }
    }
}