/*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.
  
    Copyright 2009 BabBot Team -
*/

using System;
using System.Collections.ObjectModel;
using SlimDX.Direct3D9;

namespace Dante.HUD
{
    public sealed class ResourceCollection : Collection<IResource>, IResource
    {
        private Device device;
        private ResourceMethod lastMethod = ResourceMethod.Release;

        #region IResource Members

        public void Initialize(Device managedDevice)
        {
            device = managedDevice;

            foreach (IResource resource in Items)
            {
                resource.Initialize(device);
            }

            lastMethod = ResourceMethod.Initialize;
        }

        public void LoadContent()
        {
            foreach (IResource resource in Items)
            {
                resource.LoadContent();
            }

            lastMethod = ResourceMethod.LoadContent;
        }

        public void UnloadContent()
        {
            foreach (IResource resource in Items)
            {
                resource.UnloadContent();
            }

            lastMethod = ResourceMethod.UnloadContent;
        }

        public void Dispose()
        {
            foreach (IResource resource in Items)
            {
                resource.Dispose();
            }

            lastMethod = ResourceMethod.Release;
            GC.SuppressFinalize(this);
        }

        #endregion

        protected override void ClearItems()
        {
            if (lastMethod == ResourceMethod.Initialize || lastMethod == ResourceMethod.UnloadContent)
            {
                Dispose();
            }
            else if (lastMethod == ResourceMethod.LoadContent)
            {
                UnloadContent();
                Dispose();
            }

            base.ClearItems();
        }

        protected override void InsertItem(int index, IResource item)
        {
            if (Contains(item))
            {
                throw new InvalidOperationException("Cannot add duplicates to the resource collection.");
            }

            if (lastMethod == ResourceMethod.Initialize || lastMethod == ResourceMethod.UnloadContent)
            {
                item.Initialize(device);
            }
            else if (lastMethod == ResourceMethod.LoadContent)
            {
                item.Initialize(device);
                item.LoadContent();
            }

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            IResource item = Items[index];

            if (lastMethod == ResourceMethod.Initialize || lastMethod == ResourceMethod.UnloadContent)
            {
                item.Dispose();
            }
            else if (lastMethod == ResourceMethod.LoadContent)
            {
                item.UnloadContent();
                item.Dispose();
            }

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, IResource item)
        {
            if (Contains(item))
            {
                throw new InvalidOperationException("Cannot add duplicates to the resource collection.");
            }

            if (lastMethod == ResourceMethod.Initialize || lastMethod == ResourceMethod.UnloadContent)
            {
                item.Initialize(device);
            }
            else if (lastMethod == ResourceMethod.LoadContent)
            {
                item.Initialize(device);
                item.LoadContent();
            }

            base.SetItem(index, item);
        }

        #region Nested type: ResourceMethod

        private enum ResourceMethod
        {
            Initialize,
            LoadContent,
            UnloadContent,
            Release
        }

        #endregion
    }
}