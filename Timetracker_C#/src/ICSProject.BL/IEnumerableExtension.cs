﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ICSProject.BL;

public static class EnumerableExtension
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> values)
        => new(values);
}
