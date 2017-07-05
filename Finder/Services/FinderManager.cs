using Finder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finder.Services
{
    public class FinderManager
    {
        private readonly Action<string> Add;

        private readonly Action AddFoldersCount;

        const string ROOT_PATH = "C:\\";

        public FinderManager(Action<string> add, Action addFoldersCount)
        {
            Add = add;
            AddFoldersCount = addFoldersCount;
        }

        public void Execute(SearchModel search)
        {
            FindPath(ROOT_PATH, search);
        }

        private void FindPath(string path, SearchModel search)
        {
            DirectoryInfo d = new DirectoryInfo(path);
            try
            {
                Parallel.ForEach(d.GetFiles(search.Extension), item =>
                     {
                         if (search.Months.Contains(item.CreationTime.Month) && search.Year == item.CreationTime.Year)
                         {
                             Add(item.FullName);
                         }
                     });
                AddFoldersCount();
                Parallel.ForEach(d.GetDirectories(), item =>
                {
                    FindPath(item.FullName, search);
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                //Se não tem autorização, paciência
            }
        }
    }
}
