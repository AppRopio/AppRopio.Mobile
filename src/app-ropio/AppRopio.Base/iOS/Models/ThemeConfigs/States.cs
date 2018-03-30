using System;
namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class States : ICloneable
    {
        public State Normal { get; set; }

        public State Highlighted { get; set; }

        public State Selected { get; set; }

        public State Disabled { get; set; }

        public State Invalid { get; set; }

        public object Clone()
        {
            return new States
            {
                Normal = (State)this.Normal?.Clone(),
                Highlighted = (State)this.Highlighted?.Clone(),
                Selected = (State)this.Selected?.Clone(),
                Disabled = (State)this.Disabled?.Clone(),
                Invalid = (State)this.Invalid?.Clone()
            };
        }
    }

    public class State : ICloneable
    {
        public Color Content { get; set; }

        public Color Background { get; set; }

        public object Clone()
        {
            return new State
            {
                Content = (Color)this.Content?.Clone(),
                Background = (Color)this.Background?.Clone()
            };
        }
    }
}
