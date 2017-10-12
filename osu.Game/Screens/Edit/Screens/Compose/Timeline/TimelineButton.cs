﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Screens.Edit.Screens.Compose.Timeline
{
    public class TimelineButton : CompositeDrawable
    {
        public Action Action;
        public readonly BindableBool Enabled = new BindableBool(true);

        public FontAwesome Icon
        {
            get { return button.Icon; }
            set { button.Icon = value; }
        }

        private readonly IconButton button;

        public TimelineButton()
        {
            InternalChild = button = new IconButton
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                IconColour = OsuColour.FromHex("555"),
                HoverColour = OsuColour.FromHex("3A3A3A"),
                FlashColour = OsuColour.FromHex("555"),
                Action = () => Action?.Invoke()
            };

            button.Enabled.BindTo(Enabled);
            Size = button.ButtonSize;
        }
    }
}
