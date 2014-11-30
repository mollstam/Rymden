using System;
using System.Collections.Generic;

namespace Assets.Terminal
{
    public class MapScreen : ScreenBehahvior
    {
        private RoomType _roomType;

        public MapScreen(RoomType roomType)
        {
            _roomType = roomType;
        }

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
      |    | v  |  x: You are  |    | ^  | 
      | 2     3 |      here    | 5  |  6 | 
      |_  _|____|              |_  _|_  _| 
      |       4 |              |    7    | 
      |_________|              |_________| 
      /////|\\\\\              /////|\\\\\ 
     //////|\\\\\\            //////|\\\\\\

    1: Supplies  2: Greenhouse  3: Science Lab
        4: Engineering  5: Living Quarters
    6: Medical Bay  7: Dining Hall  8: Bridge".Replace(RoomTypeToChar(_roomType), "x"),

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

        private string RoomTypeToChar(RoomType type)
        {
            switch (type)
            {
                case RoomType.LivingQuarters: return "5";
                case RoomType.Bridge: return "8";
                case RoomType.DiningRoom: return "7";
                case RoomType.Medbay: return "6";
                case RoomType.ScienceLab: return "3";
                case RoomType.Greenhouse: return "2";
                case RoomType.Engineering: return "4";
            }

            throw new NotImplementedException("Unkown room type");
        }
    }
}
