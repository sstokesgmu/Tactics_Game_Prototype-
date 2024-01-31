
//========= Sterling Stokes, All rights reserved. ============//
//
// Purpose: Is a class that handles the addition, subtraction, and equatable operations
// Notes: 
//===========================================================//

using System;

namespace Grid
{
    public struct GridPosition : IEquatable<GridPosition>
    {
        public int x;
        public int y;
        public int z;

        public GridPosition(int x, int y, int z) {
            this.x = x; this.y = y; this.z = z; }
        public override bool Equals(object obj){
            return obj is GridPosition position &&
                   x == position.x &&
                   y == position.y &&
                   z == position.z; }
        public override int GetHashCode() {
            return HashCode.Combine(x, y, z); }
        public override string ToString() {
            return $"x: {x};y: {y}; z: {z};"; }
        //logical operators
        public bool Equals(GridPosition other) {
            return this == other; }
        public static bool operator ==(GridPosition a, GridPosition b) {
            return a.x == b.x && a.y == b.y && a.z == b.z; }
        public static bool operator !=(GridPosition a, GridPosition b) {
            return !(a == b); }
        //Gridposition operators
        public static GridPosition operator +(GridPosition a, GridPosition b) {
            return new GridPosition(a.x + b.x, a.y + b.y, a.z + b.z); }
        public static GridPosition operator -(GridPosition a, GridPosition b) {
            return new GridPosition(a.x - b.x, a.y - b.y, a.z - b.z); }
    }
}


 

