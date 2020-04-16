﻿namespace LittleGarden.Core.Bus.Events
{
    public class Error : Event
    {
        public string Name { get; set; }

        public string Exception { get; set; }
        public string StackTrace { get; set; }
    }
}