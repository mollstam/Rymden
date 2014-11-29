using System.Collections.Generic;

namespace Assets.Terminal
{
    public class MapScreen : ScreenBehahvior
    {
        public ScreenInfo CurrentInfo
        {
            get
            {
                return new ScreenInfo(
@"    Upper ___  deck         Lower  ___  deck 
         /   \                    /   \    
        _|   |_   ^ Ladder up    _|___|_  
       /   1   \ v Ladder down  / |     \   
      /_________\              /     8   \ 
      |    |    |              |  |_ ____| 
      |    |  v |              |    |  ^ | 
      | 2     3 |              | 5  |  6 | 
      |_  _|____|              |_  _|_  _| 
      |       4 |              |    7    | 
      |_________|              |_________| 
      /////|\\\\\              /////|\\\\\ 
     //////|\\\\\\            //////|\\\\\\

    1: Supplies  2: Greenhouse  3: Science Lab
        4: Engineering  5: Living Quarters
    6: Medical Bay  7: Dining Hall  8: Bridge",

                    new List<ScreenAction>
                    {
                        new ScreenAction("Back", () => null)
                    });
            }
        }

        public bool ShowMessages
        {
            get { return false; }
        }

        public bool ShowMap
        {
            get { return false; }
        }
    }
}
