using Fitamas.Core;
using Fitamas.Events;
using NativeFileDialogSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fitamas.UserInterface
{
    public class OpenFileDialog
    {
        private readonly object _lock = new object();

        private string path;
        private IReadOnlyList<string> paths;

        public string DefaultPath;
        public string Filter;
        public bool Multiselect;

        public string Path => path;
        public IReadOnlyList<string> Paths => paths;

        public MonoAction<OpenFileDialog> OnCompleted;
        public MonoAction<OpenFileDialog> OnCancelled;

        public void Show()
        {
            Task.Run(ShowAsync);
        }

        public async Task ShowAsync()
        {
            DialogResult result;
            if (Multiselect)
            {
                result = Dialog.FileOpenMultiple(Filter, DefaultPath);
            }
            else
            {
                result = Dialog.FileOpen(Filter, DefaultPath);
            }

            if (result.IsOk)
            {
                lock (_lock)
                {
                    path = result.Path;
                    paths = result.Paths;
                }

                OnCompleted?.Invoke(this);
            }
            else if (result.IsCancelled)
            {
                lock (_lock)
                {
                    path = null;
                    paths = null;
                }

                OnCancelled?.Invoke(this);
            }
            else if (result.IsError)
            {
                lock (_lock)
                {
                    path = null;
                    paths = null;
                }

                throw new Exception(result.ErrorMessage);
            }

            await Task.CompletedTask;
        }
    }
}
