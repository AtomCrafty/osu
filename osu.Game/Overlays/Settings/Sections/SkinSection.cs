﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Skinning;
using OpenTK;

namespace osu.Game.Overlays.Settings.Sections
{
    public class SkinSection : SettingsSection
    {
        private SettingsDropdown<int> skinDropdown;

        public override string Header => "Skin";

        public override FontAwesome Icon => FontAwesome.fa_paint_brush;

        private SkinManager skins;

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager config, SkinManager skins)
        {
            this.skins = skins;

            FlowContent.Spacing = new Vector2(0, 5);
            Children = new Drawable[]
            {
                skinDropdown = new SettingsDropdown<int>(),
                new SettingsSlider<double, SizeSlider>
                {
                    LabelText = "Menu cursor size",
                    Bindable = config.GetBindable<double>(OsuSetting.MenuCursorSize),
                    KeyboardStep = 0.01f
                },
                new SettingsSlider<double, SizeSlider>
                {
                    LabelText = "Gameplay cursor size",
                    Bindable = config.GetBindable<double>(OsuSetting.GameplayCursorSize),
                    KeyboardStep = 0.01f
                },
                new SettingsCheckbox
                {
                    LabelText = "Adjust gameplay cursor size based on current beatmap",
                    Bindable = config.GetBindable<bool>(OsuSetting.AutoCursorSize)
                },
            };

            skins.ItemAdded += itemAdded;
            skins.ItemRemoved += itemRemoved;

            skinDropdown.Entries = skins.GetAllUsableSkins().Select(s => new KeyValuePair<string, int>(s.ToString(), s.ID));

            var skinBindable = config.GetBindable<int>(OsuSetting.Skin);

            // Todo: This should not be necessary when OsuConfigManager is databased
            if (skinDropdown.Entries.All(s => s.Value != skinBindable.Value))
                skinBindable.Value = 0;

            skinDropdown.Bindable = skinBindable;
        }

        private void itemRemoved(SkinInfo s) => skinDropdown.Entries = skinDropdown.Entries.Where(i => i.Value != s.ID);
        private void itemAdded(SkinInfo s) => skinDropdown.Entries = skinDropdown.Entries.Append(new KeyValuePair<string, int>(s.ToString(), s.ID));

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (skins != null)
            {
                skins.ItemAdded -= itemAdded;
                skins.ItemRemoved -= itemRemoved;
            }
        }

        private class SizeSlider : OsuSliderBar<double>
        {
            public override string TooltipText => Current.Value.ToString(@"0.##x");
        }
    }
}
