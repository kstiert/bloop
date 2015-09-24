﻿using System;

namespace Bloop.Plugin.WebSearch
{
    [Serializable]
    public class WebSearch
    {
        public string Title { get; set; }
        public string ActionWord { get; set; }
        public string IconPath { get; set; }
        public string Url { get; set; }
        public bool Enabled { get; set; }
    }
}