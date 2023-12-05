﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Packages.tour_creator.Editor.Protocol
{

    [Serializable]
    public class State
    {
        public string id;
        public string title;
        public string url;
        public string croppedImageUrl;
        public string type;
        public string backgroundAudioId;
        public Quaternion pictureRotation;
        public List<StateLink> links;
        public List<GroupStateLink> groupLinks;
        public List<FieldItem> fieldItems;
        public List<ContentItem> contentItems;
    }
}
