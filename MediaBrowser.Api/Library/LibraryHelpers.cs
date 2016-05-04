﻿using MediaBrowser.Controller;
using System;
using System.IO;
using System.Linq;
using CommonIO;

namespace MediaBrowser.Api.Library
{
    /// <summary>
    /// Class LibraryHelpers
    /// </summary>
    public static class LibraryHelpers
    {
        /// <summary>
        /// The shortcut file extension
        /// </summary>
        private const string ShortcutFileExtension = ".mblink";
        /// <summary>
        /// The shortcut file search
        /// </summary>
        private const string ShortcutFileSearch = "*" + ShortcutFileExtension;

        /// <summary>
        /// Deletes a shortcut from within a virtual folder, within either the default view or a user view
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="virtualFolderName">Name of the virtual folder.</param>
        /// <param name="mediaPath">The media path.</param>
        /// <param name="appPaths">The app paths.</param>
        /// <exception cref="System.IO.DirectoryNotFoundException">The media folder does not exist</exception>
        public static void RemoveMediaPath(IFileSystem fileSystem, string virtualFolderName, string mediaPath, IServerApplicationPaths appPaths)
        {
            if (string.IsNullOrWhiteSpace(mediaPath))
            {
                throw new ArgumentNullException("mediaPath");
            }

            var rootFolderPath = appPaths.DefaultUserViewsPath;
            var path = Path.Combine(rootFolderPath, virtualFolderName);

            if (!fileSystem.DirectoryExists(path))
            {
                throw new DirectoryNotFoundException(string.Format("The media collection {0} does not exist", virtualFolderName));
            }
            
            var shortcut = Directory.EnumerateFiles(path, ShortcutFileSearch, SearchOption.AllDirectories).FirstOrDefault(f => fileSystem.ResolveShortcut(f).Equals(mediaPath, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(shortcut))
            {
                fileSystem.DeleteFile(shortcut);
            }
        }
    }
}
