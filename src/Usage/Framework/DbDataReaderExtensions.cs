using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Usage.Framework
{
    internal static class DbDataReaderExtensions
    {
        public static IEnumerable<TRow> Read<TRow>(this DbDataReader reader) where TRow : new()
        {
            if (reader == null) 
                throw new ArgumentNullException("reader");
            using (reader)
            {
                if (reader.IsClosed) yield break;
                var moved = reader.Read();
                if (!moved) yield break;
                var properties = typeof (TRow).GetProperties();
                while (moved)
                {
                    var row = new TRow();
                    foreach (var property in properties)
                    {
                        var ordinal = reader.GetOrdinal(property.Name);
                        property.SetValue(row, reader.IsDBNull(ordinal) ? null : reader.GetValue(ordinal));
                    }
                    yield return row;
                    moved = reader.Read();
                }
            }
        }
    }
}