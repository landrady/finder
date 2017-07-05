using Finder.Models;
using Finder.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Finder
{
    public partial class Form1 : Form
    {
        private readonly object _lock;
        private readonly object _lockDCount;
        private readonly Dictionary<string, string> _map;
        private readonly FinderManager _finderManager;
        private bool _lockSeach;

        public Form1()
        {
            InitializeComponent();
            _map = new Dictionary<string, string>();
            _finderManager = new FinderManager(Add, AddDirectoriesCount);
            this._lockSeach = false;
            _lock = new object();
            _lockDCount = new Object();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!_lockSeach)
            {
                SearchModel searchModel = new SearchModel();
                searchModel.Extension = txtExtension.Text;
                searchModel.Year = int.Parse(txtYear.Text);
                var months = txtMonth.Text.Split(',');
                searchModel.Months = new int[months.Count()];
                for (int i = 0; i < months.Count(); i++)
                {
                    searchModel.Months[i] = int.Parse(months[i]);
                }

                new TaskFactory().StartNew(() =>
                {
                    _lockSeach = true;
                    _finderManager.Execute(searchModel);
                    _lockSeach = false;
                });
            }
            else
            {
                MessageBox.Show("Buscando! Aguarde . . .");
            }
        }

        private void Add(string path)
        {
            lock (_lock)
            {
                string name = Path.GetFileName(path);
                string directory = Path.GetFullPath(path);

                lstFiles.Invoke(new MethodInvoker(delegate 
                    { 
                        this.lstFiles.Items.Add(name); 
                        this._map.Add(name, directory); 
                    }));
            }
        }

        private void AddDirectoriesCount()
        {
            lock (_lockDCount)
            {
                lblDirectoryCount.Invoke(new MethodInvoker(delegate {
                    int data = int.Parse(lblDirectoryCount.Text);
                    data++;
                    lblDirectoryCount.Text = data.ToString(); 
                }));
            }
        }

        private void lstFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Clipboard.SetText(_map[lstFiles.SelectedItems[0].ToString()]);
        }
    }
}
