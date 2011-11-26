using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace vortexWin.Engine.Xbox
{
    /// <summary>
    /// Manages UI for Guide
    /// </summary>
    public sealed class GuideManager
    {
        /// <summary>
        /// Manager instance
        /// </summary>
        public static GuideManager Instance { get; private set; }

        private GuideManager()
        { 
        }

        static GuideManager()
        {
            Instance = new GuideManager();
        }

        Queue<GuideRequest> requests = new Queue<GuideRequest>();

        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="title">Message box title</param>
        /// <param name="text">Message box text</param>
        /// <param name="buttons">Message box buttons text</param>
        /// <param name="focusButton">Focused button index</param>
        /// <param name="icon">Message box icon</param>
        /// <param name="callback">Callback handler</param>
        /// <param name="state">Additional info</param>
        public void ShowMessageBox(string title, string text, IEnumerable<string> buttons, int focusButton, MessageBoxIcon icon, AsyncCallback callback, object state)
        {
            AddRequest(new MessageBoxGuideRequest()
            {
                Title = title,
                Text = text, 
                Buttons = buttons,
                FocusedButton = focusButton,
                MessageBoxIcon = icon,
                Callback = callback,
                State = state
            });
        }

        /// <summary>
        /// Show select device window
        /// </summary>
        /// <param name="callback">Result handler</param>
        /// <param name="state">Additional info</param>
        public void ShowDeviceSelector(AsyncCallback callback, object state)
        {
            AddRequest(new DeviceSelectorGuideRequest()
            {
                Callback = callback,
                State = state
            });
        }

        /// <summary>
        /// Show marketplace window
        /// </summary>
        public void ShowMarketplace()
        {
            AddRequest(new MarketplaceGuideRequest());
        }

        /// <summary>
        /// Show input box UI 
        /// </summary>
        /// <param name="title">Input box title</param>
        /// <param name="description">Description text</param>
        /// <param name="defaultText">Default text for input box</param>
        /// <param name="callback">Result handler</param>
        /// <param name="state">Additional data</param>
        public void ShowInput(string title, string description, string defaultText, AsyncCallback callback, object state)
        {
            AddRequest(new InputGuideRequest()
            {
                Title = title,
                Description = description,
                DefaultText = defaultText,
                Callback = callback,
                State = state
            });
        }

        private void AddRequest(GuideRequest request)
        {
            lock (requests)
            {
                requests.Enqueue(request);
            }
        }

        /// <summary>
        /// Update cycle
        /// </summary>
        /// <param name="gameTime">Elapsed time</param>
        public void Update(GameTime gameTime)
        {
            //New guide popup can be shown only if Guide is not visible
            if (!Guide.IsVisible)
            {
                lock (requests)
                {
                    if (requests.Count > 0)
                    {
                        var request = requests.Dequeue();
                        request.Run();
                    }
                }
            }
        }
    }
}
