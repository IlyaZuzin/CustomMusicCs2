using Microsoft.Win32;
using Files;

namespace CoreAPI
{
    public static class Core
    {
        public static bool setup()
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Выберите папку CS2",
                InitialDirectory = @"C:\"
            };
            if (dialog.ShowDialog() == true)
            {
                string selectedFolder = dialog.FolderName;
                string configPath = "\\csgo\\cfg\\gamestate_integration_core.cfg";
                selectedFolder += configPath;
                FileSystem.CopyFile("/config.cfg", selectedFolder);
            }
            
            return true;
        }
    }
}
    