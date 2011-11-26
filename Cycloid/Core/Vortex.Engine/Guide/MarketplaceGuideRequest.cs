using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.GamerServices;
using vortexWin.Engine.Input;

namespace vortexWin.Engine.Xbox
{
    public class MarketplaceGuideRequest : GuideRequest
    {
        public MarketplaceGuideRequest()
        {
            Type = GuideRequestType.Marketplace;
        }

        public override void Run()
        {
            if (GamePadController.Index.HasValue)
            {
                Guide.ShowMarketplace(GamePadController.Index.Value);
            }
        }
    }
}
