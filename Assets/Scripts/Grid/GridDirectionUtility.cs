namespace HoneyBearExpress.Grid
{
    public static class GridDirectionUtility
    {
        public static GridPosition GetDirectionFromRotation(int rotationIndex)
        {
            return rotationIndex switch
            {
                0 => GridPosition.North,
                1 => GridPosition.East,
                2 => GridPosition.South,
                3 => GridPosition.West,
                _ => GridPosition.North
            };
        }
    }
}
