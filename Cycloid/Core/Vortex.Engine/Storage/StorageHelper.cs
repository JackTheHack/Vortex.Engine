using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using vortexWin.Engine.Xbox;
using vortexWin.Engine.Input;

namespace vortexWin.Engine.Storage
{
    public class StorageManager:IDisposable
    {
        static StorageManager instance;
        public static StorageManager Instance{get{return instance;}}
        public static event EventHandler StorageCreated;

        static void OnStorageCreated(StorageManager manager)
        {
            if(StorageCreated!=null)
                StorageCreated(manager,new EventArgs());
        }

        static StorageManager()
        {
            instance=new StorageManager();
            StorageDevice.DeviceChanged += new EventHandler<EventArgs>(StorageDevice_DeviceChanged);
        }

        static void StorageDevice_DeviceChanged(object sender, EventArgs e)
        {
            if (!Guide.IsVisible && GamePadController.Index.HasValue)
            {
                Instance.Initialize();
            }
        }

        StorageDevice Device;

        public StorageDevice DeviceInstance { get { return Device; } }

        public StorageManager()
        {
            
        }

        private static StorageContainer OpenContainer(StorageDevice storageDevice, string saveGameName)
        {
            IAsyncResult result = storageDevice.BeginOpenContainer(saveGameName, null, null);
            // ожидание завершения операции
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = storageDevice.EndOpenContainer(result);
            result.AsyncWaitHandle.Close();
            return container;
        }  

        public StorageContainer CreateContainer(string name)
        {
            return OpenContainer(DeviceInstance, name);
        }

        #region IDisposable Members

        public void  Dispose()
        {
 	        
        }

        #endregion

        public void Initialize()
        {
                GuideManager.Instance.ShowDeviceSelector(                
                    delegate(IAsyncResult result)
                    {
                        if (result.IsCompleted)
                        {
                            Device = StorageDevice.EndShowSelector(result);
                            OnStorageCreated(this);
                        }
                    },
                    this);
        }
    }
}