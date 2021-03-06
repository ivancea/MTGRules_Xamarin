﻿namespace MTGRules
{
    enum HistoryType
    {
        None,
        Search,
        Number,
        Key,
        Random
    }

    class HistoryItem
    {
        public HistoryType Type;
        public object Value;

        public double VerticalOffset;

        public HistoryItem(HistoryType type = HistoryType.None, object value = null, double verticalOffset = 0)
        {
            Type = type;
            Value = value;
            VerticalOffset = verticalOffset;
        }
    }
}
