namespace AF.GigPlatform.Store.Config
{
    public static partial class Settings
    {
        public static Setting Secret = new Setting("y209fnyd0ms92ut0,9a2394td8,yp45utf,poi2erutp,0d1u98899u245cmp2h4st,39-cu 24t");
    }

    public class Setting
    {
        public Setting(string value) => Value = value;

        public string Value { get; }

        public bool Valid(string value) => value == Value;

        public static implicit operator Setting(string setting) => new Setting(setting);
        public static implicit operator string(Setting setting) => setting.Value;
    }

    public static class StringSettingValidationExtension
    {
        internal static bool ValidGigDataToken(this Setting value) => Settings.Secret.Valid(value);
    }
}