using System.Collections.Generic;

namespace Grid
{
    
    /*
     *Since the handling of grid cells is already deeply engrained into the system
     * It would be very difficult to create a column system which allows the current functionality to continue working as is
     * therefore I am instead using a decorator pattern to handle column info
     * - Talon
     * 
     */
    
    public class GridColumnDecorator
    {
        private int _xIndex;
        private CellType _cellType;
        private int _captureCost;

        public GridColumnDecorator(int xIndex) {
            this._xIndex = xIndex;
        }
    }
}