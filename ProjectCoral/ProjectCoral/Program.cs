using System;

namespace ProjectCoral
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ProjectCoralGame game = new ProjectCoralGame())
            {
                game.Run();
            }
        }
    }
#endif
}

