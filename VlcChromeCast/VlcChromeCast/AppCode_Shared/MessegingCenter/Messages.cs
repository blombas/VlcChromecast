using System;
using System.Collections.Generic;
using System.Text;

public class StartLongRunningTaskMessage<T> where T:class
{
    public T TaskData { get; set; }
}

public class StopLongRunningTaskMessage { }

public class CancelledMessage { }