﻿using System;

namespace Trulon
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Trulon
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Engine())
                game.Run();
        }
    }
}
